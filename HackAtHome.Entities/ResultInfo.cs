using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HackAtHome.Entities
{
    public class ResultInfo
    {
        public Status Status { get; set; }
        public string Token { get; set; }
        public string FullName { get; set; }

    }

    public enum Status
    {
        Error = 0,
        Success = 1,
        InvalidUserOrNotInEvent = 2,
        OutOfDate = 3,
        AllSuccess = 999
    }
}
