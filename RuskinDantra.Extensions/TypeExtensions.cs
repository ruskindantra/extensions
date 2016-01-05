using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace RuskinDantra.Extensions
{
    public static class TypeExtensions
    {


	    public static IEnumerable<Type> AllImplementors(this Type type)
	    {
		    if (!type.IsInterface)
				throw new InvalidOperationException("Type has to be an interface");

			return AppDomain.CurrentDomain
				.GetAssemblies()
				.SelectMany(s => s.GetTypes())
				.Where(t => t != type)
				.Where(type.IsAssignableFrom);
		}

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
