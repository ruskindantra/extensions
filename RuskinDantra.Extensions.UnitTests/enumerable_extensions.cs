using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace RuskinDantra.Extensions.UnitTests
{
	public class enumerable_extensions
	{
		[Test]
		public void should_throw_if_collection_is_null()
		{
			IEnumerable<int> collection = null;

			Action replaceAction = () => collection.Replace(1, 2);
			replaceAction.ShouldThrow<ArgumentNullException>();
		}

		[Test]
		public void should_return_same_collection_if_collection_is_empty()
		{
			IEnumerable<int> collection = new int[0]{};

			var replacedCollection = collection.Replace(1, 2);
			replacedCollection.ShouldAllBeEquivalentTo(collection);
		}

		[Test]
		public void should_return_same_collection_if_collection_does_not_contain_item()
		{ 
			var fixture = new Fixture();
			IEnumerable<int> collection = fixture.CreateMany<int>();

			int newItem = 555;
			Assume.That(collection.Any(i => i == newItem), Is.False);

			int existingItemWhichShouldNotExist = fixture.Create<int>();
			Assume.That(collection.Any(i => i == existingItemWhichShouldNotExist), Is.False);

			var replacedCollection = collection.Replace(existingItemWhichShouldNotExist, newItem);
			replacedCollection.ShouldAllBeEquivalentTo(collection);
		}

		[Test]
		public void should_return_collection_with_new_item_appended()
		{
			var fixture = new Fixture();
			IEnumerable<int> collection = fixture.CreateMany<int>(3);

			int newItem = 555;
			Assume.That(collection.Any(i => i == newItem), Is.False);

			int existingItem = collection.ElementAt(1);
			var replacedCollection = collection.Replace(existingItem, newItem);
			replacedCollection.Last().Should().Be(newItem);
		}

		[Test]
		public void should_return_collection_with_new_item_in_correct_index_position()
		{
			var fixture = new Fixture();
			IEnumerable<int> collection = fixture.CreateMany<int>(3);

			int newItem = 555;
			Assume.That(collection.Any(i => i == newItem), Is.False);

			var index = 1;
			int existingItem = collection.ElementAt(index);
			var replacedCollection = collection.Replace(existingItem, newItem, true);
			replacedCollection.ElementAt(index).Should().Be(newItem);
		}
	}
}