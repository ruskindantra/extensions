using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
		/// <returns></returns>
		public static string SerializeToXml<T>(this T data, Type containingType = null) where T : class
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
			using (var writer = new StringWriter(stringBuilder))
			{
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
	}
}