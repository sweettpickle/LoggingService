using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiLogging.Models.Response
{
    public class Base 
    {
        public string Message { get; set; }

        public Base(string message)
        {
            Message = message;
        }
    }
}
