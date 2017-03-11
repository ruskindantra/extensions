using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace RuskinDantra.Extensions
{
    public static class CollectionExtensions
    {
        public static IEnumerable<T> RemoveAllButFirstXItems<T>(this IEnumerable<T> collection, int itemsToKeep = 0)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            var list = collection.ToList();
            if (itemsToKeep == 0)
                list.Clear();
            else if (itemsToKeep > list.Count)
                throw new InvalidOperationException($"Collection only contains <{list.Count}> items and we cannot prune it down to <{itemsToKeep}>");
            else
                list.RemoveRange(itemsToKeep, list.Count - itemsToKeep);
            return list;
        }

        public static IEnumerable<int> IndexesOfRepeats<T>([NotNull] this IEnumerable<T> collection, int repeatCount = 2, [CanBeNull] Func<T, T, bool> equalityComparer = null)
        {
            var collectionAsList = collection as IList<T> ?? collection.ToList();
            if (!collectionAsList.Any())
                return new int[0];

            // if no equality comparer is given, use the default Equals operator
            if (equalityComparer == null)
                equalityComparer = (lhs, rhs) => lhs.Equals(rhs);

            var indexesOfRepeats = new List<int>();
            var currentRepeats = new List<T>();
            T itemSeen = default(T);

            int index = 0;
            int firstIndexOfRepeat = -1;
            foreach (var item in collectionAsList)
            {
                if (itemSeen != null && equalityComparer(itemSeen, item))
                {
                    currentRepeats.Add(itemSeen);
                    itemSeen = item;
                }
                else
                {
                    // lets check if repeats count is greater than the supplied parameter
                    if (currentRepeats.Count >= repeatCount)
                        indexesOfRepeats.Add(firstIndexOfRepeat);

                    itemSeen = item;
                    firstIndexOfRepeat = index;
                    currentRepeats.Clear();
                    currentRepeats.Add(itemSeen);
                }
                index++;
            }

            // lets check if we have any left over repeats
            if (currentRepeats.Count >= repeatCount)
                indexesOfRepeats.Add(firstIndexOfRepeat);

            return indexesOfRepeats;
        }
    }
}