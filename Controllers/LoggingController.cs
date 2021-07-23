using ApiLogging.DAL;
using ApiLogging.Models.Response;
using ApiLogging.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLogging.Controllers
{
    [ApiController]
    public class LoggingController : ControllerBase
    {
        protected IServiceProvider ServiceProvider { get; }
        protected LoggingService LoggingService { get; }
        protected DbSet<LoggingLevel> LoggingLevel { get; set; }

        public LoggingController(IServiceProvider serviceProvider, LoggingService service, LoggingContext context)
        {
            ServiceProvider = serviceProvider;
            LoggingService = service;
            LoggingLevel = context.Set<LoggingLevel>();
        }

        [HttpPost("Logging")]
        public async Task<IActionResult> AddLoggingAsync(Models.Request.Logging request)
        {
            if (request.StartDate >= request.EndDate)
                return BadRequest(new Base("Поле StartDate не может быть больше EndDate"));

            if (string.IsNullOrEmpty(request.ActionSource))
                return BadRequest(new Base("Поле ActionSource обязательно для заполнения"));

            var level = LoggingLevel.FirstOrDefault(x => x.Id.Equals(request.Level));
            if (level is null)
                return NotFound(new Base("Такого уровня логирования не существует"));

            var response = await LoggingService.Add(request);
            if (response is null)
                return BadRequest();

            return new JsonResult(new { id = response.Id });
        }

        [HttpDelete("Logging")]
        public async Task<IActionResult> DeleteLoggingAsync([FromQuery] List<Guid> id)
        {
            if (id.Count() == 0)
                return BadRequest(new Base("Не указаны идентификаторы в параметрах запроса"));

            var count = await LoggingService.Delete(id);

            return new JsonResult(new { count = count });
        }

        [HttpGet("LoggingSearch")]
        public async Task<IActionResult> LoggingSearchAsync(
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? EndDate,
        [FromQuery] Guid? level,
        [FromQuery] string text = "")
        {
            var response = await LoggingService.Search(startDate, EndDate, level, text);

            if (!response.Any())
                return NotFound(new Base("По данному фильтру ничего не найдено"));

            return Ok(response);
        }

        [HttpGet("LoggingStatistic")]
        public async Task<IActionResult> LoggingStatisticAsync()
        {
            var response = await LoggingService.GetStatistic();
            return Ok(response);
        }
    }
}
