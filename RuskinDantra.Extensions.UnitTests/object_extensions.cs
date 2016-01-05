using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;
using RuskinDantra.Extensions.DataStructures;

namespace RuskinDantra.Extensions.UnitTests
{
	public class test_object
	{
		public string PropertyA { get; set; }
	}

	[XmlRoot("test_object")]
	public class test_object_with_attribute
	{
		[XmlAttribute]
		public string PropertyA { get; set; }
	}

	[XmlRoot("Changes")]
	public class test_collection : XmlSerializableInterfaceList<IChange>
	{
		public test_collection() : base("Changes", "Change")
		{
		}
	}

	public interface IChange
	{
		string PropertyA { get; set; }

		ChangeType ChangeType { get; }
	}

	public enum ChangeType
	{
		None,
		ChangeA,
		ChangeB
	}

	public abstract class Change : IChange
	{
		public string PropertyA { get; set; }

		public abstract ChangeType ChangeType { get; set; }
	}

	public class ChangeB : Change
	{
		public override ChangeType ChangeType
		{
			get { return ChangeType.ChangeB; }
			set { }
		}
	}

	public class ChangeA : Change
	{
		public override ChangeType ChangeType
		{
			get { return ChangeType.ChangeA; }
			set { }
		}
	}

	public class object_extensions
	{
		[Test]
		public void should_deserialize_string_into_collection()
		{
			var xml = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
			          "<Changes>\r\n" +
					  "  <Change AssemblyQualifiedName=\"RuskinDantra.Extensions.UnitTests.ChangeA, RuskinDantra.Extensions.UnitTests, Version = 1.0.0.0, Culture = neutral, PublicKeyToken = null\">\r\n" +
			          "    <ChangeA xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
			          "      <PropertyA>Change A</PropertyA>\r\n" +
			          "      <ChangeType>ChangeA</ChangeType>\r\n" +
			          "    </ChangeA>\r\n" +
			          "  </Change>\r\n" +
					  "  <Change AssemblyQualifiedName=\"RuskinDantra.Extensions.UnitTests.ChangeB, RuskinDantra.Extensions.UnitTests, Version = 1.0.0.0, Culture = neutral, PublicKeyToken = null\">\r\n" +
			          "    <ChangeB xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n" +
			          "      <PropertyA>Change B</PropertyA>\r\n" +
			          "      <ChangeType>ChangeB</ChangeType>\r\n" +
			          "    </ChangeB>\r\n" +
			          "  </Change>\r\n" +
			          "</Changes>";

			test_collection testCollection = xml.DeserializeFromXml<test_collection>(typeof (IChange));
			testCollection.Should().HaveCount(2);
			testCollection.ElementAt(0).Should().BeOfType<ChangeA>();
			testCollection.ElementAt(1).Should().BeOfType<ChangeB>();
		}

		[Test]
		public void should_serialize_collection_into_string()
		{
			var testCollection = new test_collection()
			{
				new ChangeA {PropertyA = "Change A"},
				new ChangeB {PropertyA = "Change B"},
			};
			
			string xml = testCollection.SerializeToXml(typeof(IChange));
			XElement actualXml = XElement.Parse(xml);
			actualXml.Name.LocalName.Should().Be("Changes");
			actualXml.Should().HaveElement("Change");

			IEnumerable<XElement> changes = from change in actualXml.Elements("Change") select change;
			changes.Should().HaveCount(2);

			var changeA = changes.SingleOrDefault(
				e =>
					e.Attribute("AssemblyQualifiedName") != null &&
					e.Attribute("AssemblyQualifiedName").Value == "RuskinDantra.Extensions.UnitTests.ChangeA, RuskinDantra.Extensions.UnitTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
			changeA.Should().NotBeNull();
			changeA.Element("ChangeA").Element("PropertyA").Should().HaveValue("Change A");
			changeA.Element("ChangeA").Element("ChangeType").Should().HaveValue("ChangeA");

			var changeB = changes.SingleOrDefault(
				e =>
					e.Attribute("AssemblyQualifiedName") != null &&
					e.Attribute("AssemblyQualifiedName").Value == "RuskinDantra.Extensions.UnitTests.ChangeB, RuskinDantra.Extensions.UnitTests, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
			changeB.Should().NotBeNull();
			changeB.Element("ChangeB").Element("PropertyA").Should().HaveValue("Change B");
			changeB.Element("ChangeB").Element("ChangeType").Should().HaveValue("ChangeB");
		}

		[Test]
		public void should_serialize_object_with_attribute_into_string()
		{
			var fixture = new Fixture();
			var testObject = fixture.Create<test_object_with_attribute>();
			testObject.PropertyA = "Some property";

			string xml = testObject.SerializeToXml();
			XElement actualXml = XElement.Parse(xml);
			actualXml.Name.LocalName.Should().Be("test_object");
			actualXml.Should().HaveAttribute("PropertyA", "Some property");
		}

		[Test]
		public void should_deserialize_string_with_attribute_into_object()
		{
			string xml = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
			             "<test_object xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" PropertyA=\"Some property\" />";

			var testObject = xml.DeserializeFromXml<test_object_with_attribute>();
			testObject.Should().NotBeNull();
			testObject.PropertyA.Should().Be("Some property");
		}

		[Test]
		public void should_deserialize_string_with_attribute_into_object_with_type_passed_in()
		{
			string xml = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n" +
						 "<test_object xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" PropertyA=\"Some property\" />";

			var testObject = xml.DeserializeFromXml<test_object_with_attribute>(typeof(test_object_with_attribute));
			testObject.Should().NotBeNull();
			testObject.PropertyA.Should().Be("Some property");
		}

		[Test]
		public void should_serialize_object_into_string()
		{
			var fixture = new Fixture();
			var testObject = fixture.Create<test_object>();
			testObject.PropertyA = "Some property";

			string xml = testObject.SerializeToXml();
			XElement actualXml = XElement.Parse(xml);
			actualXml.Name.LocalName.Should().Be("test_object");
			actualXml.Should().HaveElement("PropertyA").And.HaveValue("Some property");
		}

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

		[Test]
		public void should_be_able_to_serialize_a_standard_collection()
		{
			var simpleCollectionOfStrings = new List<string>
			{
				"A",
				"B"
			};

			var xml = simpleCollectionOfStrings.SerializeToXml();
			
			XElement actualXml = XElement.Parse(xml);

			IEnumerable<XElement> strings = from str in actualXml.Elements("string") select str;
			strings.Should().HaveCount(2);
			strings.Should().Contain(e => e.Value == "A");
			strings.Should().Contain(e => e.Value == "B");
		}

		[Test]
		public void should_be_able_to_deserialize_a_standard_collection()
		{
			var xml = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<ArrayOfString xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
			          "  <string>A</string>" +
			          "  <string>B</string>" +
			          "</ArrayOfString>";
			List<string> simpleColletionOfStrings = xml.DeserializeFromXml<List<string>>();
			
			simpleColletionOfStrings.Should().HaveCount(2);
			simpleColletionOfStrings.Should().Contain(e => e == "A");
			simpleColletionOfStrings.Should().Contain(e => e == "B");
		}
	}
}