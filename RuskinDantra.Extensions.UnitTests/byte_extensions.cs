
using FluentAssertions;
using Xunit;

namespace RuskinDantra.Extensions.UnitTests
{
	public class byte_extensions
	{
        [Theory]
		[InlineData(new byte[] { 0x01 }, "0x", "0x01")]
		[InlineData(new byte[] { 1 }, "0x", "0x01")]
		[InlineData(new byte[] { }, "0x", "")]
		[InlineData(new byte[] { 16 }, "0x", "0x10")]
		[InlineData(new byte[] { 17 }, "0x", "0x11")]
		[InlineData(new byte[] { 1, 2 }, "0x", "0x01,0x02")]
		[InlineData(new byte[] { 1, 2 }, "0x", "0x01,0x02")]
		[InlineData(new byte[] { 1, 2 }, "", "01,02")]
		[InlineData(new byte[] { 100, 200 }, "0x", "0x64,0xC8")]
		public void should_return_correct_string_representation_for_byte_array(byte[] value, string prefix, string expected)
		{
			value.AsByteString(prefix).Should().Be(expected);
		}
	}
}