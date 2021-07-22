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
    //[ApiController]
    public class DeleteLoggingController : ControllerBase
    {
        protected LoggingContext LoggingContext { get; }

        public DeleteLoggingController(LoggingContext context)
        {
            LoggingContext = context;
        }

        [HttpDelete("Logging")]
        public async Task<IActionResult> Execute([FromQuery] List<Guid> id)
        {
            if (id.Count() == 0)
                return BadRequest(new Base("Не указаны идентификаторы в параметрах запроса"));

            var deleteLogging = LoggingContext.Logging.Where(x => id.Any(y => y.Equals(x.Id))).ToList();

            if (deleteLogging.Any())
            {
                LoggingContext.Logging.RemoveRange(deleteLogging);
                await LoggingContext.SaveChangesAsync();
            }

            return new JsonResult(new { count = deleteLogging.Count() });
        }
    }
}
