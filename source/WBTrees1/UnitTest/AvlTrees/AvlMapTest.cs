using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.AvlTrees;
using Xunit;

namespace UnitTest.AvlTrees
{
	public class AvlMapTest
	{
		static readonly Random random = new();
		static int[] CreateValues(int count, int max) => Array.ConvertAll(new bool[count], _ => random.Next(max));
		static KeyValuePair<int, int>[] CreateItems(int count, int max) => Array.ConvertAll(new bool[count], _ => new KeyValuePair<int, int>(random.Next(max), random.Next(max)));

		[Fact]
		public void AddRemove()
		{
			var n = 1000;
			var a = CreateItems(n, 1000);

			var d = new Dictionary<int, int>();
			var map = new AvlMap<int, int>();
			Assert.Equal(0, map.Count);

			foreach (var (k, v) in a)
			{
				Assert.Equal(d.TryAdd(k, v), map.Add(k, v));
				Assert.Equal(d.Count, map.Count);
				Assert.Equal(d.OrderBy(p => p.Key), map);
			}
			foreach (var (k, v) in a)
			{
				Assert.Equal(d.Remove(k), map.Remove(k));
				Assert.Equal(d.Count, map.Count);
				Assert.Equal(d.OrderBy(p => p.Key), map);
			}
		}

		[Fact]
		public void GetSet()
		{
			var n = 1000;
			var max = 500;
			var a = CreateItems(n, max);

			var d = new Dictionary<int, int>();
			var map = new AvlMap<int, int>();
			Assert.Equal(0, map.Count);

			foreach (var (k, v) in a)
			{
				d[k] = v;
				map[k] = v;
				Assert.Equal(d.Count, map.Count);

				for (int i = 0; i < max; i++)
				{
					var containsKey = d.ContainsKey(i);
					Assert.Equal(containsKey, map.ContainsKey(i));
					Assert.Equal(containsKey, map.TryGetValue(i, out var j));

					if (containsKey)
					{
						Assert.Equal(d[i], j);
						Assert.Equal(d[i], map[i]);
						Assert.Equal(d[i], map.GetValueOrDefault(i, -1));
					}
					else
					{
						Assert.Equal(0, j);
						Assert.Equal(-1, map.GetValueOrDefault(i, -1));
					}
				}
			}
		}

		[Fact]
		public void GetSet_Exception()
		{
			var n = 1000;
			var max = 100;
			var a = CreateValues(n, max);

			var map = new AvlMap<int, int>();

			foreach (var v in a)
			{
				map[v] = v;
				for (int i = 0; i < max; i++)
					if (!map.ContainsKey(i))
						Assert.Throws<KeyNotFoundException>(() => map[i]);
			}
		}

		[Fact]
		public void Time_Add()
		{
			var n = 200000;
			var a = CreateItems(n, 100000);

			var map = new AvlMap<int, int>();
			foreach (var (k, v) in a) map.Add(k, v);
		}

		[Fact]
		public void Time_Remove()
		{
			var n = 200000;
			var a = CreateValues(n, 130000).Distinct().Select(key => new KeyValuePair<int, int>(key, random.Next(1000000))).ToArray();

			var map = new AvlMap<int, int>();
			map.Initialize(a);
			foreach (var (k, v) in a) map.Remove(k);
		}
	}
}
