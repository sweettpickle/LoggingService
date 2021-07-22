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
    public class LoggingSearchController : ControllerBase
    {
        protected LoggingContext LoggingContext { get; }

        public LoggingSearchController(LoggingContext context)
        {
            LoggingContext = context;
        }

        [HttpGet("LoggingSearch")]
        public async Task<IActionResult> Execute(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? EndDate,
        [FromQuery] Guid? level,
        [FromQuery] string text = "")
        {
            if (text is null)
                text = "";

            var response = LoggingContext.Logging.Where(x => x.StartDate >= startDate && x.EndDate <= EndDate
                                            || x.Level.Id.Equals(level) || x.Text.ToLower().Equals(text.ToLower()))
                            .Include(x => x.Level).Include(x => x.ActionSource).ToList();

            if (response is null)
                return NotFound(new Base("По данному фильтру ничего не найдено"));

            return Ok(response);
        }
    }
}
