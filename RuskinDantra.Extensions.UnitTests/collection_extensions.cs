using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace RuskinDantra.Extensions.UnitTests
{
    [TestFixture]
    public class collection_extensions
    {
        [TestCase(new[] { 1, 1, 1, 3, 3 }, 3, new[] { 0 })]
        [TestCase(new[] { 0, 1, 1, 2, 3, 3, 3 }, 3, new[] { 4 })]
        [TestCase(new[] { 0, 1, 1, 1, 1, 1, 2, 3, 3, 3 }, 4, new[] { 1 })]
        [TestCase(new[] { 0, 1, 2, 3, 4, 5 }, 2, new int[0])]
        [TestCase(new int[0], 2, new int[0])]
        public void should_return_index_of_repeated_int_array_with_default_equality_comparer(int[] testCase, int repeatCount, int[] expectedRepeats)
        {
            var indexesOfRepeats = testCase.IndexesOfRepeats(repeatCount: repeatCount).ToList();
            indexesOfRepeats.Should().HaveCount(expectedRepeats.Length);
            indexesOfRepeats.Should().BeEquivalentTo(expectedRepeats);
        }

        [TestCase(new[] { 1, 1, 3, 3 }, new[] { 0, 2 })]
        [TestCase(new[] { 0, 1, 1, 2, 3, 3 }, new[] { 1, 4 })]
        [TestCase(new[] { 0, 1, 2, 3, 4, 5 }, new int[0])]
        [TestCase(new int[0], new int[0])]
        public void should_return_index_of_repeated_int_array_with_default_equality_comparer(int[] testCase, int[] expectedRepeats)
        {
            var indexesOfRepeats = testCase.IndexesOfRepeats().ToList();
            indexesOfRepeats.Should().HaveCount(expectedRepeats.Length);
            indexesOfRepeats.Should().BeEquivalentTo(expectedRepeats);
        }

        [TestCase(new[] { "1", "1", "3", "3" }, new[] { 0, 2 })]
        [TestCase(new[] { "0", "1", "1", "2", "3", "3" }, new[] { 1, 4 })]
        [TestCase(new[] { "0", "1", "2", "3", "4", "5" }, new int[0])]
        [TestCase(new string[0], new int[0])]
        public void should_return_index_of_repeated_string_array_with_default_equality_comparer(string[] testCase, int[] expectedRepeats)
        {
            var indexesOfRepeats = testCase.IndexesOfRepeats().ToList();
            indexesOfRepeats.Should().HaveCount(expectedRepeats.Length);
            indexesOfRepeats.Should().BeEquivalentTo(expectedRepeats);
        }

        [TestCase(new[] { "a", "a", "b", "B" }, new[] { 0, 2 })]
        [TestCase(new[] { "X", "X", "Y", "y" }, new[] { 0, 2 })]
        [TestCase(new[] { "a", "B", "C", "d" }, new int[0])]
        public void should_return_index_of_repeated_string_array_with_custom_equality_comparer(string[] testCase, int[] expectedRepeats)
        {
            var indexesOfRepeats = testCase.IndexesOfRepeats(equalityComparer:(lhs, rhs) => string.Compare(lhs, rhs, StringComparison.InvariantCultureIgnoreCase) == 0).ToList();
            indexesOfRepeats.Should().HaveCount(expectedRepeats.Length);
            indexesOfRepeats.Should().BeEquivalentTo(expectedRepeats);
        }

        [Test]
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

        [Test]
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

        [Test]
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