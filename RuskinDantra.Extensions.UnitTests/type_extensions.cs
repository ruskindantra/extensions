using FluentAssertions;
using NUnit.Framework;

namespace RuskinDantra.Extensions.UnitTests
{
	[TestFixture]
    public class type_extensions
    {
		[Test]
	    public void has_interface_should_return_true_if_type_implements_interface()
		{
			var type = typeof (test_class);
			type.HasInterface<itest_interface>().Should().BeTrue();
		}

		[Test]
		public void has_interface_should_return_false_if_type_does_not_implement_interface()
		{
			var type = typeof(test_class_no_interface);
			type.HasInterface<itest_interface>().Should().BeFalse();
		}

		[Test]
		public void get_interface_should_not_return_null_if_interface_exists()
		{
			var type = typeof(test_class);
			type.GetInterface<itest_interface>().Should().NotBeNull();
		}

		[Test]
		public void get_interface_should_return_null_if_no_interface_exists()
		{
			var type = typeof(test_class_no_interface);
			type.GetInterface<itest_interface>().Should().BeNull();
		}
	}

	public interface itest_interface
	{
		
	}

	public class test_class : itest_interface
	{
		
	}

	public class test_class_no_interface
	{
		
	}
}
