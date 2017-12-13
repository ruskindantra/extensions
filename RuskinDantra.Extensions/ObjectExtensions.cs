using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using JetBrains.Annotations;

namespace RuskinDantra.Extensions
{
	public static class ObjectExtensions
	{
		/// <summary>
		/// Serializes a given object into XML.  
		/// If the object is a collection of items then use the <param name="containingType">ContainingType</param> parameter to indicate the underlying object type
		/// </summary>
		/// <typeparam name="T">The type of object being serialized</typeparam>
		/// <param name="data">The instance of an object being serialized into XML</param>
		/// <param name="containingType">The type being contained within a collection</param>
		/// <param name="omitNsQualifications">Set this to true if you want the generated XML to omit XML namespace qualifications</param>
		/// <param name="settings">Any extra settings we need while serializing xml</param>
		/// <returns></returns>
		public static string SerializeToXml<T>(this T data, Type containingType = null, bool omitNsQualifications = false, XmlWriterSettings settings = null) where T : class
		{
			XmlSerializer serializer;
			if (containingType != null && containingType.IsInterface)
			{
				IEnumerable<Type> allTypes = containingType.AllImplementors();
				serializer = new XmlSerializer(typeof(T), allTypes.ToArray());
			}
			else
				serializer = new XmlSerializer(data.GetType());

			var stringBuilder = new StringBuilder();
			using (var writer = XmlWriter.Create(stringBuilder, settings))
			{
				if (omitNsQualifications)
				{
					XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
					xmlSerializerNamespaces.Add("", "");
					serializer.Serialize(writer, data, xmlSerializerNamespaces);
				}
				else
					serializer.Serialize(writer, data);
			}

			return stringBuilder.ToString();
		}

		public static T DeserializeFromXml<T>(this string objectData, Type containingType = null)
		{
			XmlSerializer serializer;
			if (containingType != null && containingType.IsInterface)
			{
				IEnumerable<Type> allTypes = containingType.AllImplementors();
				serializer = new XmlSerializer(typeof(T), allTypes.ToArray());
			}
			else
				serializer = new XmlSerializer(typeof(T));

			object result;
			using (TextReader reader = new StringReader(objectData))
			{
				result = serializer.Deserialize(reader);
			}

			return (T) result;
		}

		[ContractAnnotation("obj: null => halt")]
		public static void ThrowIfNull<T>([CanBeNull] this T obj, [CanBeNull] string message = null) where T : class
		{
			string exceptionMessage = message;

			if (string.IsNullOrWhiteSpace(exceptionMessage))
				exceptionMessage = $"Item <{typeof (T).FullName}> cannot be null";

			if (obj == null)
				throw new NullReferenceException(exceptionMessage);
		}

		[ContractAnnotation("obj: null => halt")]
		public static void ThrowIfNull<T>([CanBeNull] this T? obj, [CanBeNull] string message = null) where T : struct
		{
			string exceptionMessage = message;

			if (string.IsNullOrWhiteSpace(exceptionMessage))
				exceptionMessage = $"Item <{typeof (T).FullName}> cannot be null";

			if (!obj.HasValue)
				throw new NullReferenceException(exceptionMessage);
		}

	    [ContractAnnotation("obj: null => halt")]
        public static void ThrowIfArgumentNull<T>(this T obj, string paramName) where T : class
	    {
	        if ((object)obj == null)
	            throw new ArgumentNullException(paramName);
	    }
    }
}