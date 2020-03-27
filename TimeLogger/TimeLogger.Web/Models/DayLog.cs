using System;
using System.Collections.Generic;

namespace TimeLogger.Web.Models
{
    public partial class DayLog
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime InTime { get; set; }
        public DateTime? OutTime { get; set; }
        public string Description { get; set; }
    }
}
