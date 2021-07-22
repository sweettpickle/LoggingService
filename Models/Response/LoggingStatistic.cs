using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoggingService.Models.Response
{
    public class LoggingStatistic
    {
        public int NumberOfSavedRecords { get; set; }
        public Dictionary<string, int> NumberOfRecordsForEachLoggingLevel { get; set; }
        public Dictionary<string, int> NumberOfRecordsForEachSourceAction { get; set; }
    }
}
