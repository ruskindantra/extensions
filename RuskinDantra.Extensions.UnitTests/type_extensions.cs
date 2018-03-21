using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace RuskinDantra.Extensions.UnitTests
{
    public class type_extensions
    {
		private class TempAttribute : Attribute
		{
		}

		[Serializable]
		private class ClassWithAttribute
		{

		}

		[Fact]
		public void should_return_attribute_if_class_is_decorated_with_it()
		{
			typeof (ClassWithAttribute).HasAttribute<SerializableAttribute>().Should().BeTrue();
			typeof (ClassWithAttribute).HasAttribute<TempAttribute>().Should().BeFalse();

			typeof(ClassWithAttribute).GetAttribute<SerializableAttribute>().Should().NotBeNull();
			typeof(ClassWithAttribute).GetAttribute<TempAttribute>().Should().BeNull();
		}

		[Fact]
	    public void has_interface_should_return_true_if_type_implements_interface()
		{
			var type = typeof (test_class);
			type.HasInterface<itest_interface>().Should().BeTrue();
		}

		[Fact]
		public void has_interface_should_return_false_if_type_does_not_implement_interface()
		{
			var type = typeof(test_class_no_interface);
			type.HasInterface<itest_interface>().Should().BeFalse();
		}

		[Fact]
		public void get_interface_should_not_return_null_if_interface_exists()
		{
			var type = typeof(test_class);
			type.GetInterface<itest_interface>().Should().NotBeNull();
		}

		[Fact]
		public void get_interface_should_return_null_if_no_interface_exists()
		{
			var type = typeof(test_class_no_interface);
			type.GetInterface<itest_interface>().Should().BeNull();
		}

		[Fact]
		public void get_base_types_should_return_empty_list_if_class_has_no_base_class()
		{
			var type = typeof(test_class_no_interface);
			type.GetBaseTypes().Count().Should().Be(1); // we expect System.Object
			type.GetBaseTypes().ElementAt(0).Should().Be(typeof(object));
		}

		[Fact]
		public void get_base_types_should_return_empty_list_if_class_has_no_base_class_and_an_interface()
		{
			var type = typeof(test_class);
			type.GetBaseTypes().Count().Should().Be(1); // we expect System.Object
			type.GetBaseTypes().ElementAt(0).Should().Be(typeof(object));
		}

		[Fact]
		public void get_base_types_should_return_list_of_base_classes_if_class_extends_base_class()
		{
			var type = typeof(test_class_extends);
			type.GetBaseTypes().Count().Should().Be(2); // we expect System.Object
			type.GetBaseTypes().Where(t => t != typeof(object)).ElementAt(0).Should().Be(typeof(test_class_no_interface));
		}

		[Fact]
		public void get_base_types_should_return_list_of_base_classes_if_class_extends_base_class_which_extends_another_class()
		{
			var type = typeof(test_class_extends_again);
			type.GetBaseTypes().Count().Should().Be(3); // we expect System.Object
			type.GetBaseTypes().Where(t => t != typeof(object)).Should().Contain(typeof(test_class_no_interface));
			type.GetBaseTypes().Where(t => t != typeof(object)).Should().Contain(typeof(test_class_extends));
		}

		[Fact]
		public void all_implementors_should_return_all_type_which_implement_interface()
		{
			var allImplementors = typeof (itest_interface).AllImplementors();
			allImplementors.Should().HaveCount(3);
			allImplementors.Should().Contain(typeof (test_class));
			allImplementors.Should().Contain(typeof(test_class2));
			allImplementors.Should().Contain(typeof(test_class3));
		}

		[Fact]
		public void all_implementors_should_throw_if_called_against_a_non_interface()
		{
			Action allImplementorsAction = () => typeof(test_class).AllImplementors();
			allImplementorsAction.Should().Throw<InvalidOperationException>().WithMessage("Type has to be an interface");
		}

		[Fact]
		public void should_return_correct_type_definition_for_generic_list()
		{
			var list = new List<string>();
			Type[] genericTypes = list.GetType().GenericTypeArguments;
			genericTypes.Should().HaveCount(1);
		}

		[Fact]
		public void should_return_correct_type_definition_for_generic_class()
		{
			var genericClass = new generic_class<string>();
			Type[] genericTypes = genericClass.GetType().GenericTypeArguments;
			genericTypes.Should().HaveCount(1);
		}

		[Fact]
		public void should_return_null_type_definition_for_non_generic_class()
		{
			var str = "A";
			Type[] genericTypes = str.GetType().GenericTypeArguments;
			genericTypes.Should().HaveCount(0);
		}
	}

	public class generic_class<T>
	{
		
	}

	public interface itest_interface
	{
		
	}

	public class test_class : itest_interface
	{
		
	}

	public class test_class2 : itest_interface
	{

	}

	public class test_class3 : test_class
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
