using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace RuskinDantra.Extensions.UnitTests
{
    public class collection_extensions
    {
        [Fact]
        public void remove_all_but_first_x_items_should_throw_exception_for_null_collection()
        {
            List<string> collection = null;
            Action removeAllButAction = () => collection.RemoveAllButFirstXItems();
            removeAllButAction.ShouldThrow<ArgumentNullException>();
        }

        [Fact]
        public void remove_all_but_first_x_items_should_throw_if_removing_more_items_than_collection_initially_contains()
        {
            var collection = new[] {"a", "b", "c", "d"};
            Action removeAllButAction = () => collection.RemoveAllButFirstXItems(collection.Count() + 1);
            removeAllButAction.ShouldThrow<InvalidOperationException>();
        }

        [Theory]
        [InlineData(new[] {1, 2, 3, 4}, 2, new[] { 1, 2})]
        [InlineData(new[] {1, 2, 3, 4}, 2, new[] { 1, 2})]
        [InlineData(new[] {1, 2, 3, 4}, 4, new[] { 1, 2, 3, 4})]
        public void remove_all_but_first_x_items_should_remove_items_as_instructed(int[] original, int itemsToKeep, int[] expected)
        {
            original.RemoveAllButFirstXItems(itemsToKeep).ShouldBeEquivalentTo(expected);
        }

        [Theory]
        [InlineData(new[] { 1, 1, 1, 3, 3 }, 3, new[] { 0 })]
        [InlineData(new[] { 0, 1, 1, 2, 3, 3, 3 }, 3, new[] { 4 })]
        [InlineData(new[] { 0, 1, 1, 1, 1, 1, 2, 3, 3, 3 }, 4, new[] { 1 })]
        [InlineData(new[] { 0, 1, 2, 3, 4, 5 }, 2, new int[0])]
        [InlineData(new int[0], 2, new int[0])]
        public void should_return_index_of_repeated_int_array_with_default_equality_comparer_repeat_count(int[] testCase, int repeatCount, int[] expectedRepeats)
        {
            var indexesOfRepeats = testCase.IndexesOfRepeats(repeatCount: repeatCount).ToList();
            indexesOfRepeats.Should().HaveCount(expectedRepeats.Length);
            indexesOfRepeats.Should().BeEquivalentTo(expectedRepeats);
        }

        [Theory]
        [InlineData(new[] { 1, 1, 3, 3 }, new[] { 0, 2 })]
        [InlineData(new[] { 0, 1, 1, 2, 3, 3 }, new[] { 1, 4 })]
        [InlineData(new[] { 0, 1, 2, 3, 4, 5 }, new int[0])]
        [InlineData(new int[0], new int[0])]
        public void should_return_index_of_repeated_int_array_with_default_equality_comparer(int[] testCase, int[] expectedRepeats)
        {
            var indexesOfRepeats = testCase.IndexesOfRepeats().ToList();
            indexesOfRepeats.Should().HaveCount(expectedRepeats.Length);
            indexesOfRepeats.Should().BeEquivalentTo(expectedRepeats);
        }

        [Theory]
        [InlineData(new[] { "1", "1", "3", "3" }, new[] { 0, 2 })]
        [InlineData(new[] { "0", "1", "1", "2", "3", "3" }, new[] { 1, 4 })]
        [InlineData(new[] { "0", "1", "2", "3", "4", "5" }, new int[0])]
        [InlineData(new string[0], new int[0])]
        public void should_return_index_of_repeated_string_array_with_default_equality_comparer(string[] testCase, int[] expectedRepeats)
        {
            var indexesOfRepeats = testCase.IndexesOfRepeats().ToList();
            indexesOfRepeats.Should().HaveCount(expectedRepeats.Length);
            indexesOfRepeats.Should().BeEquivalentTo(expectedRepeats);
        }

        [Theory]
        [InlineData(new[] { "a", "a", "b", "B" }, new[] { 0, 2 })]
        [InlineData(new[] { "X", "X", "Y", "y" }, new[] { 0, 2 })]
        [InlineData(new[] { "a", "B", "C", "d" }, new int[0])]
        public void should_return_index_of_repeated_string_array_with_custom_equality_comparer(string[] testCase, int[] expectedRepeats)
        {
            var indexesOfRepeats = testCase.IndexesOfRepeats(equalityComparer:(lhs, rhs) => string.Compare(lhs, rhs, StringComparison.InvariantCultureIgnoreCase) == 0).ToList();
            indexesOfRepeats.Should().HaveCount(expectedRepeats.Length);
            indexesOfRepeats.Should().BeEquivalentTo(expectedRepeats);
        }

        [Fact]
        public void should_return_index_of_repeated_object_array_with_default_equality_comparer()
        {
            var testCase1 = new test_class_default_comparer() { A = "a", B = 0 };
            var testCase2 = new test_class_default_comparer() { A = "b", B = 1 };
            var testCase3 = new test_class_default_comparer() { A = "c", B = 2 };

            // looks like 4 and 5 are equal but not according to the default comparer
            var testCase4 = new test_class_default_comparer() { A = "d", B = 3 };
            var testCase5 = new test_class_default_comparer() { A = "d", B = 3 };

            var testCases = new[] { testCase1, testCase2, testCase3, testCase4, testCase5, testCase5};

            var indexesOfRepeats = testCases.IndexesOfRepeats().ToList();
            indexesOfRepeats.Should().HaveCount(1);
            indexesOfRepeats.Should().BeEquivalentTo(new[] { 4 });
        }

        [Fact]
        public void should_return_index_of_repeated_object_array_with_custom_equality_comparer()
        {
            var testCase1 = new test_class_default_comparer() { A = "a", B = 0 };
            var testCase2 = new test_class_default_comparer() { A = "b", B = 1 };
            var testCase3 = new test_class_default_comparer() { A = "c", B = 2 };

            // looks like 4 and 5 are equal but not according to the default comparer, only according to a custom comparer
            var testCase4 = new test_class_default_comparer() { A = "d", B = 3 };
            var testCase5 = new test_class_default_comparer() { A = "d", B = 3 };

            var testCases = new[] {testCase1, testCase2, testCase3, testCase4, testCase5, testCase5};

            var indexesOfRepeats = testCases.IndexesOfRepeats(equalityComparer:(lhs, rhs) => lhs.A == rhs.A && lhs.B == rhs.B).ToList();
            indexesOfRepeats.Should().HaveCount(1);
            indexesOfRepeats.Should().BeEquivalentTo(new[] {3});
        }

        [Fact]
        public void should_return_index_of_repeated_object_array_with_custom_equality_comparer_only_lhs_to_rhs_not_other_way_around()
        {
            var testCase1 = new test_class_default_comparer() { A = "a", B = 0 };
            var testCase2 = new test_class_default_comparer() { A = "b", B = 1 };
            var testCase3 = new test_class_default_comparer() { A = "c", B = 2 };

            // looks like 4 and 5 are equal but not according to the default comparer, only according to a custom comparer
            var testCase4 = new test_class_default_comparer() { A = "d", B = 3 };
            var testCase5 = new test_class_default_comparer() { A = "e", B = 3 };

            var testCase6 = new test_class_default_comparer() { A = "z", B = 2 };

            var testCase7 = new test_class_default_comparer() { A = "e", B = 3 };
            var testCase8 = new test_class_default_comparer() { A = "d", B = 3 };

            var testCases = new[] { testCase1, testCase2, testCase3, testCase4, testCase5, testCase6, testCase7, testCase8 };

            var indexesOfRepeats = testCases.IndexesOfRepeats(equalityComparer: (lhs, rhs) => lhs.A == "d" && rhs.A == "e").ToList();
            indexesOfRepeats.Should().HaveCount(1);
            indexesOfRepeats.Should().BeEquivalentTo(new[] { 3 });
        }

        [Fact]
        public void should_return_index_of_repeated_object_array_with_builtin_equality_comparer()
        {
            var testCase1 = new test_class_builtin_comparer() { A = "a", B = 0 };
            var testCase2 = new test_class_builtin_comparer() { A = "b", B = 1 };
            var testCase3 = new test_class_builtin_comparer() { A = "c", B = 2 };
            var testCase4 = new test_class_builtin_comparer() { A = "d", B = 3 };
            var testCase5 = new test_class_builtin_comparer() { A = "d", B = 3 };

            var testCases = new[] { testCase1, testCase2, testCase3, testCase4, testCase5, testCase5 };

            var indexesOfRepeats = testCases.IndexesOfRepeats().ToList();
            indexesOfRepeats.Should().HaveCount(1);
            indexesOfRepeats.Should().BeEquivalentTo(new[] { 3 });
        }

        private class test_class_builtin_comparer
        {
            public string A { get; set; }

            public int B { get; set; }

            public override bool Equals(object obj)
            {
                return Equals((test_class_builtin_comparer) obj);
            }

            private bool Equals(test_class_builtin_comparer other)
            {
                return string.Equals(A, other.A) && B == other.B;
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((A?.GetHashCode() ?? 0)*397) ^ B;
                }
            }
        }

        private class test_class_default_comparer
        {
            public string A { get; set; }

            public int B { get; set; }
        }
    }
}