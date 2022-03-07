using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab;
using TreesLab.AvlTrees201;
using Xunit;

namespace UnitTest.AvlTrees201
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

		[Fact]
		public void FirstLast()
		{
			var n = 1000;
			var max = 1000;
			var l = new List<int>();
			var set = new AvlMultiSet<int>();

			int v;
			while (n-- > 0)
			{
				for (int k = 0; k < 3; k++)
				{
					l.Add(v = random.Next(max));
					Assert.Equal(v, set.Add(v).Item);
				}
				l.Sort();
				Assert.Equal(l, set);

				Assert.Equal(l[0], set.GetFirst().Item);
				Assert.Equal(l[0], set.RemoveFirst().Item);
				l.RemoveAt(0);
				Assert.Equal(l[^1], set.GetLast().Item);
				Assert.Equal(l[^1], set.RemoveLast().Item);
				l.RemoveAt(l.Count - 1);
				Assert.Equal(l, set);
			}
		}

		[Fact]
		public void FirstLast_Set()
		{
			var n = 1000;
			var max = 1000;
			var l = new List<int>();
			var set = new AvlSet<int>();

			int v;
			while (n-- > 0)
			{
				for (int k = 0; k < 3; k++)
				{
					if (l.Contains(v = random.Next(max)))
					{
						Assert.Null(set.Add(v));
					}
					else
					{
						l.Add(v);
						Assert.Equal(v, set.Add(v).Item);
					}
				}
				l.Sort();
				Assert.Equal(l, set);

				if (l.Count == 0)
				{
					Assert.Null(set.GetFirst());
					Assert.Null(set.RemoveFirst());
				}
				else
				{
					Assert.Equal(l[0], set.GetFirst().Item);
					Assert.Equal(l[0], set.RemoveFirst().Item);
					l.RemoveAt(0);
				}

				if (l.Count == 0)
				{
					Assert.Null(set.GetLast());
					Assert.Null(set.RemoveLast());
				}
				else
				{
					Assert.Equal(l[^1], set.GetLast().Item);
					Assert.Equal(l[^1], set.RemoveLast().Item);
					l.RemoveAt(l.Count - 1);
				}
				Assert.Equal(l, set);
			}
		}

		[Fact]
		public void FirstLast_Predicate()
		{
			var n = 1000;
			var max = 1000;
			var l = new List<int>();
			var set = new AvlMultiSet<int>();

			int v, i;
			while (n-- > 0)
			{
				for (int k = 0; k < 3; k++)
				{
					l.Add(v = random.Next(max));
					Assert.Equal(v, set.Add(v).Item);
				}
				l.Sort();
				Assert.Equal(l, set);

				if ((v = random.Next(max)) > l.Last())
				{
					Assert.Null(set.GetFirst(x => x >= v));
					Assert.Null(set.RemoveFirst(x => x >= v));
				}
				else
				{
					i = Enumerable.Range(0, l.Count).First(j => l[j] >= v);
					Assert.Equal(l[i], set.GetFirst(x => x >= v).Item);
					Assert.Equal(l[i], set.RemoveFirst(x => x >= v).Item);
					l.RemoveAt(i);
				}

				if ((v = random.Next(max)) <= l.First())
				{
					Assert.Null(set.GetLast(x => x < v));
					Assert.Null(set.RemoveLast(x => x < v));
				}
				else
				{
					i = Enumerable.Range(0, l.Count).Last(j => l[j] < v);
					Assert.Equal(l[i], set.GetLast(x => x < v).Item);
					Assert.Equal(l[i], set.RemoveLast(x => x < v).Item);
					l.RemoveAt(i);
				}
				Assert.Equal(l, set);
			}
		}
	}
}
