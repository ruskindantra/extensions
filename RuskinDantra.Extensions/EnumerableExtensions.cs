using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace RuskinDantra.Extensions
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<T> Replace<T>([NotNull] this IEnumerable<T> collection, T existingItem, T newItem, bool maintainIndex = false)
		{
			if (collection == null)
				throw new ArgumentNullException(nameof(collection), "Collection cannot be null");

			IList<T> collectionAsList = collection.ToList();
			if (!collectionAsList.Contains(existingItem))
				return collectionAsList;

			int indexOf = collectionAsList.IndexOf(existingItem);
			collectionAsList.Remove(existingItem);
			if (maintainIndex)
				collectionAsList.Insert(indexOf, newItem);
			else
				collectionAsList.Add(newItem);
			return collectionAsList;
		}
	}
}