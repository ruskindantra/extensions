using System;
using FluentAssertions;
using NUnit.Framework;

namespace RuskinDantra.Extensions.UnitTests
{
	public class object_extensions
	{
		[Test]
		public void null_object_should_throw_null_reference_exception()
		{
			object nullObject = null;
			Action throwIfNullAction = () => nullObject.ThrowIfNull();
			throwIfNullAction.ShouldThrow<NullReferenceException>().WithMessage("Item <System.Object> cannot be null");
		}

		[TestCase("")]
		[TestCase(null)]
		[TestCase("custom message")]
		[TestCase("    ")]
		public void null_object_should_throw_null_reference_exception_with_custom_message_if_provided(string customMessage)
		{
			if (string.IsNullOrWhiteSpace(customMessage))
				customMessage = "Item <System.Object> cannot be null";

			object nullObject = null;
			Action throwIfNullAction = () => nullObject.ThrowIfNull(customMessage);
			throwIfNullAction.ShouldThrow<NullReferenceException>().WithMessage(customMessage);
		}

		[TestCase("")]
		[TestCase(null)]
		[TestCase("custom message")]
		[TestCase("    ")]
		public void null_struct_should_throw_null_reference_exception_with_custom_message_if_provided(string customMessage)
		{
			if (string.IsNullOrWhiteSpace(customMessage))
				customMessage = "Item <System.Object> cannot be null";

			int? nullObject = null;
			Action throwIfNullAction = () => nullObject.ThrowIfNull(customMessage);
			throwIfNullAction.ShouldThrow<NullReferenceException>().WithMessage(customMessage);
		}
	}
}