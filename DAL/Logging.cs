using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LoggingService.DAL
{
    public class Logging
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Text { get; set; }
        public LoggingLevel Level { get; set; }
        public ActionSource ActionSource { get; set; }

        public Logging()
        {
            Id = Guid.NewGuid();
        }
    }
}
