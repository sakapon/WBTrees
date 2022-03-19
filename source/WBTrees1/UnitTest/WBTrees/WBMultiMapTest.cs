using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;
using Xunit;

namespace UnitTest.WBTrees
{
	public class WBMultiMapTest
	{
		static readonly Random random = new();
		static int[] CreateValues(int count, int max) => Array.ConvertAll(new bool[count], _ => random.Next(max));
		static KeyValuePair<int, int>[] CreateItems(int count, int max) => Array.ConvertAll(new bool[count], _ => new KeyValuePair<int, int>(random.Next(max), random.Next(max)));

		[Fact]
		public void AddRemove()
		{
			var n = 500;
			var a = CreateItems(n, 500);

			var map = new WBMultiMap<int, int>();
			Assert.Equal(0, map.Count);

			for (int c = 1; c <= n; c++)
			{
				Assert.Equal(a[c - 1], map.Add(a[c - 1]).Item);
				Assert.Equal(c, map.Count);
				Assert.Equal(a[..c].OrderBy(p => p.Key), map);
			}
			for (int c = 1; c <= n; c++)
			{
				Assert.Equal(a[c - 1], map.RemoveFirst(a[c - 1].Key).Item);
				Assert.Equal(n - c, map.Count);
				Assert.Equal(a[c..].OrderBy(p => p.Key), map);
			}
		}

		[Fact]
		public void RemoveAll()
		{
			var n = 500;
			var a = CreateItems(n, 500);

			var map = new WBMultiMap<int, int>();
			map.Initialize(a);
			var l = map.ToList();

			foreach (var (k, v) in a)
			{
				var expected = l.RemoveAll(p => p.Key == k);
				var actual = map.RemoveAll(k);
				Assert.Equal(expected, actual);
				Assert.Equal(l.Count, map.Count);
				Assert.Equal(l, map);
			}
			Assert.Equal(0, map.Count);
		}

		[Fact]
		public void GetFirstIndex_ByItem()
		{
			var n = 100000;
			var max = 130000;
			var keys = CreateValues(n, max);
			var items = keys.Select(key => new KeyValuePair<int, int>(key, random.Next(1000000)));
			Array.Sort(keys);

			var map = new WBMultiMap<int, int>();
			Assert.Equal(n, map.AddItems(items));

			var expected = new int[max];
			Array.Fill(expected, -1);
			for (int i = n - 1; i >= 0; i--)
				expected[keys[i]] = i;

			Assert.Equal(expected, Enumerable.Range(0, max).Select(map.GetFirstIndex));
		}

		[Fact]
		public void GetLastIndex_ByItem()
		{
			var n = 100000;
			var max = 130000;
			var keys = CreateValues(n, max);
			var items = keys.Select(key => new KeyValuePair<int, int>(key, random.Next(1000000)));
			Array.Sort(keys);

			var map = new WBMultiMap<int, int>();
			Assert.Equal(n, map.AddItems(items));

			var expected = new int[max];
			Array.Fill(expected, -1);
			for (int i = 0; i < n; i++)
				expected[keys[i]] = i;

			Assert.Equal(expected, Enumerable.Range(0, max).Select(map.GetLastIndex));
		}

		[Fact]
		public void GetCount()
		{
			var n = 100000;
			var max = 100000;
			var a = CreateItems(n, max);
			var expected = a.GroupBy(p => p.Key).ToDictionary(g => g.Key, g => g.Count());

			var map = new WBMultiMap<int, int>();
			map.Initialize(a);

			for (int k = 0; k < max; k++)
			{
				Assert.Equal(expected.ContainsKey(k), map.ContainsKey(k));
				Assert.Equal(expected.ContainsKey(k) ? expected[k] : 0, map.GetCount(k));
			}
		}

		[Fact]
		public void GetValues()
		{
			var n = 100000;
			var max = 100000;
			var a = CreateItems(n, max);
			var expected = a.ToLookup(p => p.Key, p => p.Value);

			var map = new WBMultiMap<int, int>();
			map.Initialize(a);

			for (int k = 0; k < max; k++)
			{
				Assert.Equal(expected[k], map[k]);
			}
		}

		[Fact]
		public void Time_Add()
		{
			var n = 200000;
			var a = CreateItems(n, 10);

			var map = new WBMultiMap<int, int>();
			foreach (var (k, v) in a) map.Add(k, v);
		}

		[Fact]
		public void Time_Remove()
		{
			var n = 200000;
			var a = CreateItems(n, 10);

			var map = new WBMultiMap<int, int>();
			map.Initialize(a);
			foreach (var (k, v) in a) map.RemoveFirst(k);
		}
	}
}
