using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLogging.DAL
{
    public class ActionSource
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }

        public ActionSource()
        {
            Id = Guid.NewGuid();
        }
    }
}
