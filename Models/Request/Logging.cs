using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LoggingService.Models.Request
{
    public class Logging
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Text { get; set; }
        public Guid Level { get; set; }
        public string ActionSource { get; set; } = string.Empty;
    }
}
