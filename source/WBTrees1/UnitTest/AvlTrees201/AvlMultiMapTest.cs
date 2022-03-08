using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.AvlTrees201;
using Xunit;

namespace UnitTest.AvlTrees201
{
	public class AvlMultiMapTest
	{
		static readonly Random random = new();
		static int[] CreateValues(int count, int max) => Array.ConvertAll(new bool[count], _ => random.Next(max));
		static KeyValuePair<int, int>[] CreateItems(int count, int max) => Array.ConvertAll(new bool[count], _ => new KeyValuePair<int, int>(random.Next(max), random.Next(max)));

		[Fact]
		public void AddRemove()
		{
			var n = 500;
			var a = CreateItems(n, 500);

			var map = new AvlMultiMap<int, int>();
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

			var map = new AvlMultiMap<int, int>();
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
		public void Time_Add()
		{
			var n = 200000;
			var a = CreateItems(n, 10);

			var map = new AvlMultiMap<int, int>();
			foreach (var (k, v) in a) map.Add(k, v);
		}

		[Fact]
		public void Time_Remove()
		{
			var n = 200000;
			var a = CreateItems(n, 10);

			var map = new AvlMultiMap<int, int>();
			map.Initialize(a);
			foreach (var (k, v) in a) map.RemoveFirst(k);
		}
	}
}
