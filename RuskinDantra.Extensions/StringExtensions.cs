using System;

namespace RuskinDantra.Extensions
{
	public static class StringExtensions
	{
		public static string Flatten(this string str)
		{
			return str.Replace(Environment.NewLine, "").Replace('\t', '\0');
		}
	}
}