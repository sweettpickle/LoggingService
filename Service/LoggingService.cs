using ApiLogging.DAL;
using ApiLogging.Models.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLogging.Service
{
    public class LoggingService
    {
        protected DbSet<Logging> Logging { get; set; }
        protected DbSet<LoggingLevel> LoggingLevel { get; set; }
        protected DbSet<ActionSource> ActionSource { get; set; }
        protected LoggingContext LoggingContext { get; }

        public LoggingService(LoggingContext context)
        {
            LoggingContext = context;
            Logging = context.Set<Logging>();
            LoggingLevel = context.Set<LoggingLevel>();
            ActionSource = context.Set<ActionSource>();
        }

        public async Task<Logging> Add(Models.Request.Logging logging)
        {
            //так как источник действия это скорее всего какой-то сервис, который необходимо логировать,
            //то вместо текста лучше передавать его идентификатор, потому что это безопаснее,
            //проверка на существование записи по первичному ключу так же будет быстрее, нежели по текствому полю
            var actionSource = ActionSource.FirstOrDefault(x => x.Name.ToLower().Equals(logging.ActionSource.ToLower()));
            if (actionSource is null)
            {
                actionSource = new ActionSource()
                {
                    Name = logging.ActionSource
                };
                ActionSource.Add(actionSource);
            }

            var level = LoggingLevel.FirstOrDefault(x => x.Id.Equals(logging.Level));

            var loggingDb = new Logging()
            {
                StartDate = logging.StartDate,
                EndDate = logging.EndDate,
                Text = logging.Text,
                Level = level,
                ActionSource = actionSource
            };

            Logging.Add(loggingDb);
            await LoggingContext.SaveChangesAsync();

            return loggingDb;
        }

        public async Task<int> Delete([FromQuery] List<Guid> id)
        {
            var deleteLogging = Logging.Where(x => id.Any(y => y.Equals(x.Id))).ToList();

            if (deleteLogging.Any())
            {
                Logging.RemoveRange(deleteLogging);
                await LoggingContext.SaveChangesAsync();
            }

            return deleteLogging.Count();
        }

        public async Task<IEnumerable<Logging>> Search(DateTime? startDate, DateTime? EndDate, Guid? level, string text)
        {
            var loggings = Logging.Where(x => x.StartDate >= startDate && x.EndDate <= EndDate
                                            || x.Level.Id.Equals(level) || (text != null && x.Text.ToLower().Equals(text.ToLower())))
                            .Include(x => x.Level).Include(x => x.ActionSource).ToList();

            return loggings;
        }

        public async Task<LoggingStatistic> GetStatistic()
        {
            var count = Logging.Count();
            var recordsForEachLoggingLevel = Logging.GroupBy(x => x.Level.Name).Select(x => new { name = x.Key, count = x.Count() }).ToDictionary(x => x.name, x => x.count);
            var recordsForEachSourceAction = Logging.GroupBy(x => x.ActionSource.Name).Select(x => new { name = x.Key, count = x.Count() }).ToDictionary(x => x.name, x => x.count);

            var loggingStatistic = new LoggingStatistic()
            {
                NumberOfSavedRecords = count,
                NumberOfRecordsForEachLoggingLevel = recordsForEachLoggingLevel,
                NumberOfRecordsForEachSourceAction = recordsForEachSourceAction
            };

            return loggingStatistic;
        }
    }
}
