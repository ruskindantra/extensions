using System;

namespace RuskinDantra.Extensions
{
    public static class TypeExtensions
    {
		public static bool HasInterface<TInterface>(this Type type)
			where TInterface : class
		{
			return GetInterface<TInterface>(type) != null;
		}

		public static Type GetInterface<TInterface>(this Type type)
			where TInterface : class
		{
			return type.GetInterface(typeof(TInterface).Name);
		}
	}
}
