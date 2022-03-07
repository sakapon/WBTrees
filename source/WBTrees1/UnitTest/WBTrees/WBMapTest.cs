using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;
using Xunit;

namespace UnitTest.WBTrees
{
	public class WBMapTest
	{
		static readonly Random random = new();
		static int[] CreateValues(int count, int max) => Array.ConvertAll(new bool[count], _ => random.Next(max));
		static KeyValuePair<int, int>[] CreateItems(int count, int max) => Array.ConvertAll(new bool[count], _ => new KeyValuePair<int, int>(random.Next(max), random.Next(max)));

		[Fact]
		public void Initialize_Tuple()
		{
			var n = 100000;
			var expected = CreateValues(n, 70000).Distinct().Select(key => new KeyValuePair<int, int>(key, random.Next(1000000))).ToArray();

			var map = new WBMap<int, int>();
			map.Initialize(expected.Select(p => (p.Key, p.Value)));

			expected = expected.OrderBy(p => p.Key).ToArray();
			Assert.Equal(expected.Length, map.Count);
			Assert.Equal(expected, map);
		}

		[Fact]
		public void AddItems_Tuple()
		{
			var n = 100000;
			var a = CreateItems(n, 70000);
			var expected = a.GroupBy(p => p.Key).Select(g => g.First()).ToArray();

			var map = new WBMap<int, int>();
			Assert.Equal(expected.Length, map.AddItems(a.Select(p => (p.Key, p.Value))));

			expected = expected.OrderBy(p => p.Key).ToArray();
			Assert.Equal(expected.Length, map.Count);
			Assert.Equal(expected, map);
		}

		[Fact]
		public void AddRemove()
		{
			var n = 1000;
			var a = CreateItems(n, 1000);

			var d = new Dictionary<int, int>();
			var map = new WBMap<int, int>();
			Assert.Equal(0, map.Count);

			foreach (var (k, v) in a)
			{
				Assert.Equal(d.TryAdd(k, v), map.Add(k, v).Exists());
				Assert.Equal(d.Count, map.Count);
				Assert.Equal(d.OrderBy(p => p.Key), map);
			}
			foreach (var (k, v) in a)
			{
				Assert.Equal(d.Remove(k), map.Remove(k).Exists());
				Assert.Equal(d.Count, map.Count);
				Assert.Equal(d.OrderBy(p => p.Key), map);
			}
		}

		[Fact]
		public void GetIndex_ByItem()
		{
			var n = 100000;
			var max = 130000;
			var keys = CreateValues(n, max).Distinct().ToArray();
			var items = keys.Select(key => new KeyValuePair<int, int>(key, random.Next(1000000)));
			Array.Sort(keys);

			var map = new WBMap<int, int>();
			Assert.Equal(keys.Length, map.AddItems(items));

			var expected = new int[max];
			Array.Fill(expected, -1);
			for (int i = 0; i < keys.Length; i++)
				expected[keys[i]] = i;

			Assert.Equal(expected, Enumerable.Range(0, max).Select(map.GetIndex));
		}

		[Fact]
		public void GetCount()
		{
			var n = 100000;
			var max = 70000;
			var keys = CreateValues(n, max).Distinct().ToArray();
			var items = keys.Select(key => new KeyValuePair<int, int>(key, random.Next(1000000))).ToArray();

			var hs = new HashSet<int>(keys);
			var map = new WBMap<int, int>();
			map.Initialize(items);

			for (int k = 0; k < max; k++)
			{
				Assert.Equal(hs.Contains(k), map.ContainsKey(k));
				Assert.Equal(hs.Contains(k) ? 1 : 0, map.GetCount(k));
			}
		}

		[Fact]
		public void GetSet()
		{
			var n = 1000;
			var max = 100;
			var a = CreateItems(n, max);

			var d = new Dictionary<int, int>();
			var map = new WBMap<int, int>();
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
					var node = map.Get(i);
					Assert.Equal(containsKey, node.Exists());
					if (containsKey) Assert.Equal(d[i], node.Item.Value);
					if (containsKey) Assert.Equal(d[i], map[i]);
					Assert.Equal(node, map.GetFirst(i));
					Assert.Equal(node, map.GetLast(i));
				}
			}
		}

		[Fact]
		public void GetSet_Exception()
		{
			var n = 1000;
			var max = 100;
			var a = CreateValues(n, max);

			var map = new WBMap<int, int>();

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
			var a = CreateItems(n, 130000);

			var map = new WBMap<int, int>();
			foreach (var (k, v) in a) map.Add(k, v);
		}

		[Fact]
		public void Time_Remove()
		{
			var n = 200000;
			var a = CreateValues(n, 130000);

			var map = new WBMap<int, int>();
			map.Initialize(a.Distinct().Select(k => new KeyValuePair<int, int>(k, random.Next(1000000))));
			foreach (var k in a) map.Remove(k);
		}
	}
}
