using System;
using System.Collections.Generic;

namespace TimeLogger.Web.Models
{
    public partial class Profile
    {
        public Profile()
        {
            DayLog = new HashSet<DayLog>();
        }

        public int EmpId { get; set; }
        public string Name { get; set; }
        public DateTimeOffset? DateOfBirth { get; set; }
        public string Designation { get; set; }
        public double? Experience { get; set; }
        public string Token { get; set; }
        public int? UserId { get; set; }
        public string Password { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<DayLog> DayLog { get; set; }
    }
}