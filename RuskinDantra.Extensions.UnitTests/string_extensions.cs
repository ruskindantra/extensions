using System;
using System.Security;
using FluentAssertions;
using Xunit;

namespace RuskinDantra.Extensions.UnitTests
{
	public class string_extensions
	{
		[Fact]
		public void should_flatten_string_with_new_line_characters()
		{
			var stringWithNewLines = "A string with" + Environment.NewLine + " carriage return";
			stringWithNewLines.Flatten().Should().Be("A string with carriage return");
		}

        [Fact]
	    public void should_convert_to_secure_string()
	    {
	        var value = "ruskin.dantra";
	        SecureString valueAsSecureString = value.ToSecureString();
	        valueAsSecureString.Length.Should().Be(value.Length);
	    }
	}
}