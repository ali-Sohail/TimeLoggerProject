using System.Collections.Generic;
using System.Linq;
using TimeLogger.Web.Models;

namespace TimeLogger.Web.Helpers
{
    public static class ExtensionMethods
    {
        public static IEnumerable<Profile> WithoutPasswords(this IEnumerable<Profile> profile)
        {
            return profile.Select(x => x.WithoutPassword());
        }

        public static Profile WithoutPassword(this Profile profile)
        {
            profile.Password = null;
            return profile;
        }
    }
}