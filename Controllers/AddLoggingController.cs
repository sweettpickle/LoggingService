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
    public class AddLoggingController : ControllerBase
    {
        protected LoggingContext LoggingContext { get; }

        public AddLoggingController(LoggingContext context)
        {
            LoggingContext = context;
        }

        [HttpPost("Logging")]
        public async Task<IActionResult> Execute(Models.Request.Logging request)
        {
            if (request.StartDate >= request.EndDate)
                return BadRequest(new Base("Поле StartDate не может быть больше EndDate"));

            var level = LoggingContext.LoggingLevel.FirstOrDefault(x => x.Id.Equals(request.Level));
            if (level is null)
                return NotFound(new Base("Такого уровня логирования не существует"));

            if (request.ActionSource is null)
                request.ActionSource = "";

            //так как источник действия это скорее всего какой-то сервис, который необходимо логировать,
            //то вместо текста лучше передавать его идентификатор, потому что это безопаснее,
            //проверка на существование записи по первичному ключу так же будет быстрее, нежели по текствому полю
            var actionSource = LoggingContext.ActionSource.FirstOrDefault(x => x.Name.ToLower().Equals(request.ActionSource.ToLower()));
            if (actionSource is null)
            {
                actionSource = new ActionSource()
                {
                    Name = request.ActionSource
                };
                LoggingContext.ActionSource.Add(actionSource);
            }

            var logging = new Logging()
            {
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                Text = request.Text,
                Level = level,
                ActionSource = actionSource
            };

            LoggingContext.Logging.Add(logging);
            await LoggingContext.SaveChangesAsync();

            return new JsonResult(new { id = logging.Id });
        }
    }
}
