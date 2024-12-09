using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using StudentPerformanceSystem.Models;
using StudentPerformanceSystem.Data;

namespace StudentPerformanceSystem.ETL
{
    public class Worker : BackgroundService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;
        private readonly string _apiKey;
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _scopeFactory;

        public Worker(ILogger<Worker> logger, IHttpClientFactory httpClientFactory, IOptions<ThirdPartyApiOptions> options, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
            _httpClient = httpClientFactory.CreateClient();
            _baseUrl = options.Value.BaseUrl;
            _apiKey = options.Value.ApiKey;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Starting data sync.");

                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<StudentPerformanceContext>();
                        await FetchAndSyncDataAsync<Class>(context, "api/classes");
                        await FetchAndSyncDataAsync<School>(context, "api/schools");
                        await FetchAndSyncDataAsync<Score>(context, "api/scores");
                        await FetchAndSyncDataAsync<Student>(context, "api/students");
                        await FetchAndSyncDataAsync<Teacher>(context, "api/teachers");
                        await FetchAndSyncDataAsync<Test>(context, "api/tests");
                    }

                    _logger.LogInformation("Data sync completed.");

                    await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while syncing data.");
                }
            }
        }

        private async Task FetchAndSyncDataAsync<T>(StudentPerformanceContext context, string endpoint) where T : class
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _apiKey);
            var response = await _httpClient.GetAsync($"{_baseUrl}{endpoint}");

            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<IEnumerable<T>>(jsonResponse);

                if (data != null)
                {
                    foreach (var item in data)
                    {
                        var existingItem = await context.Set<T>().FindAsync(GetPrimaryKeyValues(context, item));
                        if (existingItem != null)
                        {
                            context.Entry(existingItem).CurrentValues.SetValues(item);
                        }
                        else
                        {
                            await context.Set<T>().AddAsync(item);
                        }
                    }

                    await context.SaveChangesAsync();
                }
            }
            else
            {
                _logger.LogWarning("Failed to retrieve data from {Endpoint}: {StatusCode}", endpoint, response.StatusCode);
            }
        }

        private object[] GetPrimaryKeyValues(StudentPerformanceContext context, object entity)
        {
            var entityType = context.Model.FindEntityType(entity.GetType());
            var primaryKey = entityType.FindPrimaryKey();
            var keyValues = new object[primaryKey.Properties.Count];

            for (int i = 0; i < primaryKey.Properties.Count; i++)
            {
                keyValues[i] = entity.GetType().GetProperty(primaryKey.Properties[i].Name).GetValue(entity);
            }

            return keyValues;
        }
    }
}
