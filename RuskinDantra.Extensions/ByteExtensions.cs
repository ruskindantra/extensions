using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace RuskinDantra.Extensions
{
	public static class ByteExtensions
	{
		[NotNull]
		public static string AsByteString([NotNull] this IEnumerable<byte> bytes, string prefix = "0x")
		{
			return string.Join(",", bytes.Select(b => prefix + b.ToString("X2")));
		}
	}
}