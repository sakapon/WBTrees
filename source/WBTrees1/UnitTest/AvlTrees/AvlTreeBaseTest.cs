using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab;
using TreesLab.AvlTrees;
using Xunit;

namespace UnitTest.AvlTrees
{
	public class AvlTreeBaseTest
	{
		static readonly Random random = new();
		static int[] CreateValues(int count, int max) => Array.ConvertAll(new bool[count], _ => random.Next(max));
		static KeyValuePair<int, int>[] CreateItems(int count, int max) => Array.ConvertAll(new bool[count], _ => new KeyValuePair<int, int>(random.Next(max), random.Next(max)));

		[Fact]
		public void Initialize_Set()
		{
			var n = 200000;
			var expected = CreateValues(n, 130000).Distinct().ToArray();

			var set = new AvlSet<int>();
			Assert.Equal(0, set.Count);
			set.Initialize(expected);
			Assert.Equal(expected.Length, set.Count);

			Array.Sort(expected);
			Assert.Equal(expected, set);
			Assert.Equal(expected, set.GetItems());
			Array.Reverse(expected);
			Assert.Equal(expected, set.GetItemsDescending());
		}

		[Fact]
		public void Initialize_Map()
		{
			var n = 100000;
			var expected = CreateValues(n, 70000).Distinct().Select(key => new KeyValuePair<int, int>(key, random.Next(1000000))).ToArray();

			var map = new AvlMap<int, int>();
			Assert.Equal(0, map.Count);
			map.Initialize(expected);
			Assert.Equal(expected.Length, map.Count);

			expected = expected.OrderBy(p => p.Key).ToArray();
			Assert.Equal(expected, map);
			Assert.Equal(expected, map.GetItems());
			Array.Reverse(expected);
			Assert.Equal(expected, map.GetItemsDescending());
		}

		[Fact]
		public void Initialize_MultiSet()
		{
			var n = 100000;
			var expected = CreateValues(n, 100);

			var set = new AvlMultiSet<int>();
			Assert.Equal(0, set.Count);
			set.Initialize(expected);
			Assert.Equal(expected.Length, set.Count);

			Array.Sort(expected);
			Assert.Equal(expected, set);
			Assert.Equal(expected, set.GetItems());
			Array.Reverse(expected);
			Assert.Equal(expected, set.GetItemsDescending());
		}

		[Fact]
		public void Initialize_MultiMap()
		{
			var n = 50000;
			var expected = CreateItems(n, 100);

			var map = new AvlMultiMap<int, int>();
			Assert.Equal(0, map.Count);
			map.Initialize(expected);
			Assert.Equal(expected.Length, map.Count);

			// stable sort
			expected = expected.OrderBy(p => p.Key).ToArray();
			Assert.Equal(expected, map);
			Assert.Equal(expected, map.GetItems());
			Array.Reverse(expected);
			Assert.Equal(expected, map.GetItemsDescending());
		}

		[Fact]
		public void Initialize_Exception_Set()
		{
			var a = new[] { 1, -1 };
			var comparer = ComparerHelper<int>.Create(Math.Abs);
			var set = new AvlSet<int>(comparer);
			Assert.Throws<ArgumentException>(() => set.Initialize(a));
		}

		[Fact]
		public void Initialize_Exception_Map()
		{
			var d = new Dictionary<int, int> { { 1, 2 }, { -1, 3 } };
			var comparer = ComparerHelper<int>.Create(Math.Abs);
			var map = new AvlMap<int, int>(comparer);
			Assert.Throws<ArgumentException>(() => map.Initialize(d));
		}
	}
}
