using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace RuskinDantra.Extensions.DataStructures
{
	public abstract class XmlSerializableInterfaceList<T> : List<T>, IXmlSerializable
	{
		private readonly string _collectionItemName;
		private readonly string _itemName;

		private const string AssemblyQualifiedName = "AssemblyQualifiedName";

		protected XmlSerializableInterfaceList(string collectionItemName, string itemName)
		{
			if (!typeof (T).IsInterface)
			{
				throw new InvalidOperationException("This collection is designed to work over a collection of interfaces");
			}

			_collectionItemName = collectionItemName;
			_itemName = itemName;
		}

		public XmlSchema GetSchema()
		{
			return null;
		}

		public void ReadXml(XmlReader reader)
		{
			reader.ReadStartElement(_collectionItemName);
			while (reader.IsStartElement(_itemName))
			{
				string attributeValue = reader.GetAttribute(AssemblyQualifiedName);
				if (string.IsNullOrWhiteSpace(attributeValue))
					throw new InvalidOperationException($"Cannot read type without <{attributeValue}> defined");
				Type type = Type.GetType(attributeValue);

				if (type == null)
					throw new NullReferenceException($"Cannot find type <{attributeValue}>");

				XmlSerializer serial = new XmlSerializer(type);

				reader.ReadStartElement(_itemName);
				Add((T) serial.Deserialize(reader));
				reader.ReadEndElement();
			}
			reader.ReadEndElement();
		}

		public void WriteXml(XmlWriter writer)
		{
			foreach (var change in this)
			{
				writer.WriteStartElement(_itemName);
				writer.WriteAttributeString(AssemblyQualifiedName, change.GetType().AssemblyQualifiedName);
				XmlSerializer xmlSerializer = new XmlSerializer(change.GetType());
				xmlSerializer.Serialize(writer, change);
				writer.WriteEndElement();
			}
		}
	}
}
