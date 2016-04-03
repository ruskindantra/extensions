using System.Linq;
using System.Reflection;

namespace RuskinDantra.Extensions
{
	public static class MemberInfoExtensions
	{
		public static TAttribute GetAttribute<TAttribute>(this MemberInfo member, bool inherit = true)
		{
			return (TAttribute)member.GetCustomAttributes(typeof(TAttribute), inherit).SingleOrDefault();
		}

		public static bool HasAttribute<TAttribute>(this MemberInfo member, bool inherit = true)
		{
			return member.GetCustomAttributes(typeof(TAttribute), inherit).Length > 0;
		}
	}
}