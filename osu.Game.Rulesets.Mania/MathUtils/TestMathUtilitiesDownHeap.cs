// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using NUnit.Framework;

namespace osu.Game.Rulesets.Mania.MathUtils
{
    [TestFixture]
    public class TestMath
    {
        [Test]
        public void TestDownHeap_Heapify()
        {
            // Arrange
            int[] array = { 5, 3, 8, 4, 2, 7 };
            IComparer<int> comparer = Comparer<int>.Default;

            // Act
            LegacySortHelper<int>.downHeap(array, 1, array.Length, 0, comparer);

            // Assert
            // Validate the array after heapifying
            Assert.AreEqual(8, array[0]); // Root should be the largest element
            Assert.AreEqual(3, array[1]); // Left child of the root
            Assert.AreEqual(7, array[2]); // Right child of the root
            Assert.AreEqual(4, array[3]); // Left child of the second level
            Assert.AreEqual(2, array[4]); // Right child of the second level
            Assert.AreEqual(5, array[5]);
        }

        [Test]
        public void TestDownHeap_NoHeapify()
        {
            // Arrange
            int[] array = { 8, 7, 6, 5, 4, 3 };
            IComparer<int> comparer = Comparer<int>.Default;

            // Act
            LegacySortHelper<int>.downHeap(array, 1, array.Length, 0, comparer);

            // Assert
            // Ensure no changes occurred as the array was already a valid heap
            Assert.AreEqual(8, array[0]);
            Assert.AreEqual(7, array[1]);
            Assert.AreEqual(6, array[2]);
            Assert.AreEqual(5, array[3]);
            Assert.AreEqual(4, array[4]);
            Assert.AreEqual(3, array[5]);
        }
    }
}
