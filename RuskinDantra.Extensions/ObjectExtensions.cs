using System;
using JetBrains.Annotations;

namespace RuskinDantra.Extensions
{
	public static class ObjectExtensions
	{
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
				exceptionMessage = string.Format("Item <{0}> cannot be null", typeof(T).FullName);

			if (!obj.HasValue)
				throw new NullReferenceException(exceptionMessage);
		}
	}
}