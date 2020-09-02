using System;

namespace TimeLogger.Web.Models
{
	public partial class DayLog/*	: Realms.RealmObject*/
	{
        public int Id { get; set; }
        public int EmpId { get; set; }
        public DateTimeOffset InTime { get; set; }
        public DateTimeOffset? OutTime { get; set; }
        public string Description { get; set; }

        public virtual Profile Emp { get; set; }
    }
}
