using FluentAssertions;
using NUnit.Framework;

namespace RuskinDantra.Extensions.UnitTests
{
	public class byte_extensions
	{
		[TestCase(new byte[] { 0x01 }, "0x", "0x01")]
		[TestCase(new byte[] { 1 }, "0x", "0x01")]
		[TestCase(new byte[] { }, "0x", "")]
		[TestCase(new byte[] { 16 }, "0x", "0x10")]
		[TestCase(new byte[] { 17 }, "0x", "0x11")]
		[TestCase(new byte[] { 1, 2 }, "0x", "0x01,0x02")]
		[TestCase(new byte[] { 1, 2 }, "0x", "0x01,0x02")]
		[TestCase(new byte[] { 1, 2 }, "", "01,02")]
		[TestCase(new byte[] { 100, 200 }, "0x", "0x64,0xC8")]
		public void should_return_correct_string_representation_for_byte_array(byte[] value, string prefix, string expected)
		{
			value.AsByteString(prefix).Should().Be(expected);
		}
	}
}