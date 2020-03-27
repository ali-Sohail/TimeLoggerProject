using System;
using System.Collections.Generic;

namespace TimeLogger.Web.Models
{
    public partial class Profile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string EmpId { get; set; }
        public string Designation { get; set; }
        public double? Experience { get; set; }
    }
}
