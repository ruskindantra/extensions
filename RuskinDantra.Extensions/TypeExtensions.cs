using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace RuskinDantra.Extensions
{
    public static class TypeExtensions
    {
		public static bool HasInterface<TInterface>([NotNull] this Type type)
			where TInterface : class
		{
			return GetInterface<TInterface>(type) != null;
		}

		[CanBeNull]
		public static Type GetInterface<TInterface>([NotNull] this Type type)
			where TInterface : class
		{
			return type.GetInterface(typeof(TInterface).Name);
		}

		[NotNull]
		public static IEnumerable<Type> GetBaseTypes([NotNull] this Type type)
		{
			do
			{
				yield return type.BaseType;

				type = type.BaseType;
			} while (type != typeof(object) && type != null);
		}
	}
}
