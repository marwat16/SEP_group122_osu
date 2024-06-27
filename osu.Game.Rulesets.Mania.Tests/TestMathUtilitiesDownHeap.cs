// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using NUnit.Framework;
using osu.Game.Rulesets.Mania.MathUtils;

namespace osu.Game.Rulesets.Mania.Tests
{
    [TestFixture]
    public class TestMathUtilitiesDownHeap
    {
        [Test]
        public void DownHeap_BasicExecution()
        {
            var keys = new int[] { 3, 1, 2 };
            LegacySortHelper<int>.downHeap(keys, 1, 3, 0, Comparer<int>.Default);
            Assert.AreEqual(new int[] { 3, 1, 2 }, keys);
        }

        [Test]
        public void DownHeap_FirstChildComparisonTrue()
        {
            var keys = new int[] { 3, 1, 4, 2 };
            LegacySortHelper<int>.downHeap(keys, 1, 4, 0, Comparer<int>.Default);
            Assert.AreEqual(new int[] { 4, 1, 3, 2 }, keys);
        }

        [Test]
        public void DownHeap_FirstChildComparisonFalse()
        {
            var keys = new int[] { 4, 3, 1, 2 };
            LegacySortHelper<int>.downHeap(keys, 1, 4, 0, Comparer<int>.Default);
            Assert.AreEqual(new int[] { 4, 3, 1, 2 }, keys);
        }

        [Test]
        public void DownHeap_NoChildComparison()
        {
            var keys = new int[] { 4, 3, 2, 1 };
            LegacySortHelper<int>.downHeap(keys, 2, 4, 0, Comparer<int>.Default);
            Assert.AreEqual(new int[] { 4, 3, 2, 1 }, keys);
        }

        [Test]
        public void DownHeap_BreakCondition()
        {
            var keys = new int[] { 2, 3, 1 };
            LegacySortHelper<int>.downHeap(keys, 1, 3, 0, Comparer<int>.Default);
            Assert.AreEqual(new int[] { 3, 2, 1 }, keys);
        }


        [Test]
        public void DownHeap_NoBreakCondition()
        {
            var keys = new int[] { 3, 1, 2 };
            LegacySortHelper<int>.downHeap(keys, 1, 3, 0, Comparer<int>.Default);
            Assert.AreEqual(new int[] { 3, 1, 2 }, keys);
        }
    }
}
