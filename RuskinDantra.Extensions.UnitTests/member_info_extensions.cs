using System;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;

namespace RuskinDantra.Extensions.UnitTests
{
	public class member_info_extensions
	{
		private class TempAttribute : Attribute
		{
		}
		
		private class ClassWithAttribute
		{
			[Temp]
			private string _tempField = null;
		}

		[Test]
		public void should_return_true_if_field_is_decorated_with_it()
		{
			typeof(ClassWithAttribute).GetFields(BindingFlags.NonPublic|BindingFlags.Instance).Where(f => f.HasAttribute<TempAttribute>()).Should().HaveCount(1);
		}

		[Test]
		public void should_return_attribute_if_field_is_decorated_with_it()
		{
			typeof(ClassWithAttribute).GetField("_tempField", BindingFlags.NonPublic | BindingFlags.Instance).GetAttribute<TempAttribute>().Should().NotBeNull();
		}
	}
}