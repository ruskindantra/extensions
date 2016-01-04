using System.Linq;
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

		[Test]
		public void get_base_types_should_return_empty_list_if_class_has_no_base_class()
		{
			var type = typeof(test_class_no_interface);
			type.GetBaseTypes().Count().Should().Be(1); // we expect System.Object
			type.GetBaseTypes().ElementAt(0).Should().Be(typeof(object));
		}

		[Test]
		public void get_base_types_should_return_empty_list_if_class_has_no_base_class_and_an_interface()
		{
			var type = typeof(test_class);
			type.GetBaseTypes().Count().Should().Be(1); // we expect System.Object
			type.GetBaseTypes().ElementAt(0).Should().Be(typeof(object));
		}

		[Test]
		public void get_base_types_should_return_list_of_base_classes_if_class_extends_base_class()
		{
			var type = typeof(test_class_extends);
			type.GetBaseTypes().Count().Should().Be(2); // we expect System.Object
			type.GetBaseTypes().Where(t => t != typeof(object)).ElementAt(0).Should().Be(typeof(test_class_no_interface));
		}

		[Test]
		public void get_base_types_should_return_list_of_base_classes_if_class_extends_base_class_which_extends_another_class()
		{
			var type = typeof(test_class_extends_again);
			type.GetBaseTypes().Count().Should().Be(3); // we expect System.Object
			type.GetBaseTypes().Where(t => t != typeof(object)).Should().Contain(typeof(test_class_no_interface));
			type.GetBaseTypes().Where(t => t != typeof(object)).Should().Contain(typeof(test_class_extends));
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

	public class test_class_extends : test_class_no_interface
	{
		
	}

	public class test_class_extends_again : test_class_extends
	{

	}
}
