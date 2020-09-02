using System;
using System.ComponentModel.DataAnnotations;

namespace TimeLogger.Web.Models
{
	public partial class DayLog
	{
		public int Id { get; set; }

		[Required]
		public int EmpId { get; set; }

		[Required]
		public DateTimeOffset InTime { get; set; }

		public DateTimeOffset? OutTime { get; set; }
		public string Description { get; set; }

		public virtual Profile Emp { get; set; }
	}
}