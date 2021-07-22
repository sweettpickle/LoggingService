using LoggingService.DAL;
using LoggingService.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LoggingService.Controllers
{
    [ApiController]
    public class LoggingStatisticController : ControllerBase
    {
        protected LoggingContext LoggingContext { get; }

        public LoggingStatisticController(LoggingContext context)
        {
            LoggingContext = context;
        }

        [HttpGet("LoggingStatistic")]
        public async Task<IActionResult> Execute()
        {
            var count = LoggingContext.Logging.Count();
            var recordsForEachLoggingLevel = LoggingContext.Logging.GroupBy(x => x.Level.Name).Select(x => new { name = x.Key, count = x.Count() }).ToDictionary(x => x.name, x => x.count);
            var recordsForEachSourceAction = LoggingContext.Logging.GroupBy(x => x.ActionSource.Name).Select(x => new { name = x.Key, count = x.Count() }).ToDictionary(x => x.name, x => x.count);

            var response = new LoggingStatistic()
            {
                NumberOfSavedRecords = count,
                NumberOfRecordsForEachLoggingLevel = recordsForEachLoggingLevel,
                NumberOfRecordsForEachSourceAction = recordsForEachSourceAction
            };
            return Ok(response);
        }
    }
}
