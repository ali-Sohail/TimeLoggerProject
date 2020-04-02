using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TimeLogger.Web.Models
{
	public partial class User
	{
		public User()
		{
			Profile = new HashSet<Profile>();
		}

		public int UserId { get; set; }

		[Required]
		public string Username { get; set; }

		[Required]
		public string Password { get; set; }

		[Required]
		public string Email { get; set; }

		public string Phone { get; set; }

		public virtual ICollection<Profile> Profile { get; set; }
	}
}