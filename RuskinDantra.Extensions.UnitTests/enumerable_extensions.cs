using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace RuskinDantra.Extensions.UnitTests
{
	public class enumerable_extensions
	{
		[Fact]
		public void should_throw_if_collection_is_null()
		{
			IEnumerable<int> collection = null;

			Action replaceAction = () => collection.Replace(1, 2);
			replaceAction.Should().Throw<ArgumentNullException>();
		}

		[Fact]
		public void should_return_same_collection_if_collection_is_empty()
		{
			IEnumerable<int> collection = new int[0]{};

			var replacedCollection = collection.Replace(1, 2);
			replacedCollection.Should().AllBeEquivalentTo(collection);
		}

		[Fact]
		public void should_return_same_collection_if_collection_does_not_contain_item()
		{
		    IEnumerable<int> collection = new[] {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};

			int newItem = 555;
            collection.Should().NotContain(i => i == newItem);

			int existingItemWhichShouldNotExist = 100;
		    collection.Should().NotContain(i => i == existingItemWhichShouldNotExist);

            var replacedCollection = collection.Replace(existingItemWhichShouldNotExist, newItem);
			replacedCollection.Should().AllBeEquivalentTo(collection);
		}

		[Fact]
		public void should_return_collection_with_new_item_appended()
		{
		    IEnumerable<int> collection = new[] { 1, 2, 3 };

            int newItem = 555;
            collection.Should().NotContain(i => i == newItem);

			int existingItem = collection.ElementAt(1);
			var replacedCollection = collection.Replace(existingItem, newItem);
			replacedCollection.Last().Should().Be(newItem);
		}

		[Fact]
		public void should_return_collection_with_new_item_in_correct_index_position()
		{
		    IEnumerable<int> collection = new[] { 1, 2, 3 };

            int newItem = 555;
		    collection.Should().NotContain(i => i == newItem);

            var index = 1;
			int existingItem = collection.ElementAt(index);
			var replacedCollection = collection.Replace(existingItem, newItem, true);
			replacedCollection.ElementAt(index).Should().Be(newItem);
		}
	}
}