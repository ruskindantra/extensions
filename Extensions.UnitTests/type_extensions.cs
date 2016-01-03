using FluentAssertions;
using NUnit.Framework;

namespace Extensions.UnitTests
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
