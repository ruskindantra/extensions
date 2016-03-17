using System;
using FluentAssertions;
using NUnit.Framework;

namespace RuskinDantra.Extensions.UnitTests
{
	[TestFixture]
	public class string_extensions
	{
		[Test]
		public void should_flatten_string_with_new_line_characters()
		{
			var stringWithNewLines = "A string with" + Environment.NewLine + " carriage return";
			stringWithNewLines.Flatten().Should().Be("A string with carriage return");
		}
	}
}