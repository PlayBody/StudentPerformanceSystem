using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentPerformanceSystem.ETL
{
    public class ThirdPartyApiOptions
    {
        public required string BaseUrl { get; set; }
        public required string ApiKey { get; set; }
    }
}
