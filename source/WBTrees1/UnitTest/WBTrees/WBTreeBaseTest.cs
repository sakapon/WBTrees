using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab;
using TreesLab.WBTrees;
using Xunit;

namespace UnitTest.WBTrees
{
	public class WBTreeBaseTest
	{
		static readonly Random random = new();
		static int[] CreateValues(int count, int max) => Array.ConvertAll(new bool[count], _ => random.Next(max));
		static KeyValuePair<int, int>[] CreateItems(int count, int max) => Array.ConvertAll(new bool[count], _ => new KeyValuePair<int, int>(random.Next(max), random.Next(max)));

		#region From IndexedTreeTest

		[Fact]
		public void GetItems_ByIndex()
		{
			var n = 1000;
			var a = CreateValues(n, 1000);
			Array.Sort(a);

			var set = new WBMultiSet<int>();
			set.Initialize(a, false);

			for (int k = 0; k < 1000; k++)
			{
				var (i1, i2) = (random.Next(-10, n + 10), random.Next(-10, n + 10));
				var j1 = Math.Clamp(i1, 0, n);
				var j2 = Math.Clamp(i2, 0, n);
				j1 = Math.Min(j1, j2);

				var expected = a[j1..j2];
				Assert.Equal(expected, set.GetItems(i1, i2));
				Array.Reverse(expected);
				Assert.Equal(expected, set.GetItemsDescending(i1, i2));
			}
		}

		[Fact]
		public void RemoveItems_ByIndex()
		{
			var n = 1000;
			var l = CreateValues(n, 1000).ToList();
			l.Sort();

			var set = new WBMultiSet<int>();
			set.Initialize(l, false);

			for (int k = 0; k < 1000; k++)
			{
				var (i1, i2) = (random.Next(-10, n + 10), random.Next(-10, n + 10));
				var j1 = Math.Clamp(i1, 0, l.Count);
				var j2 = Math.Clamp(i2, 0, l.Count);
				j1 = Math.Min(j1, j2);

				var count = j2 - j1;
				l.RemoveRange(j1, count);
				Assert.Equal(count, set.RemoveItems(i1, i2));
				Assert.Equal(l, set);
			}
		}

		[Fact]
		public void GetRemove_FirstLast()
		{
			var n = 1000; // even
			var l = CreateValues(n, 1000).ToList();
			l.Sort();

			var set = new WBMultiSet<int>();
			set.Initialize(l, false);

			while (l.Count > 0)
			{
				Assert.Equal(l[0], set.GetFirst().Item);
				Assert.Equal(l[0], set.RemoveFirst().Item);
				l.RemoveAt(0);
				Assert.Equal(l.Count, set.Count);
				Assert.Equal(l, set);

				Assert.Equal(l[^1], set.GetLast().Item);
				Assert.Equal(l[^1], set.RemoveLast().Item);
				l.RemoveAt(l.Count - 1);
				Assert.Equal(l.Count, set.Count);
				Assert.Equal(l, set);
			}

			Assert.Null(set.GetFirst());
			Assert.Null(set.RemoveFirst());
			Assert.Null(set.GetLast());
			Assert.Null(set.RemoveLast());
		}

		[Fact]
		public void GetRemove_At()
		{
			var n = 1000;
			var l = CreateValues(n, 1000).ToList();
			l.Sort();

			var set = new WBMultiSet<int>();
			set.Initialize(l, false);

			for (int c = n; c > 0; c--)
			{
				Assert.Equal(l.Count, set.Count);
				Assert.Equal(l, set);
				for (int i = 0; i < c; i++)
					Assert.Equal(l[i], set.GetAt(i).Item);

				for (int i = 0; i < 5; i++)
				{
					Assert.Null(set.GetAt(-1 - i));
					Assert.Null(set.RemoveAt(-1 - i));
					Assert.Null(set.GetAt(c + i));
					Assert.Null(set.RemoveAt(c + i));
				}

				var index = random.Next(c);
				Assert.Equal(l[index], set.RemoveAt(index).Item);
				l.RemoveAt(index);
			}

			Assert.Equal(l.Count, set.Count);
			Assert.Equal(l, set);
			Assert.Null(set.GetAt(0));
			Assert.Null(set.RemoveAt(0));
		}

		[Fact]
		public void GetIndex()
		{
			var n = 1000;
			var a = CreateValues(n, 1000);
			Array.Sort(a);

			var set = new WBMultiSet<int>();
			set.Initialize(a, false);

			for (int c = n; c > 0; c--)
			{
				for (int i = 0; i < c; i++)
					Assert.Equal(i, set.GetAt(i).GetIndex());

				var index = random.Next(c);
				set.RemoveAt(index);
			}
		}

		[Fact]
		public void Time_GetRemoveAt()
		{
			var n = 200000;
			var a = CreateValues(n, 10000);
			Array.Sort(a);

			var set = new WBMultiSet<int>();
			set.Initialize(a, false);

			for (int c = n; c > 0; c--)
			{
				var index = random.Next(c);
				set.GetAt(index);
				set.RemoveAt(index);
			}
		}

		[Fact]
		public void Time_GetIndex()
		{
			var n = 200000;
			var a = CreateValues(n, 10000);
			Array.Sort(a);

			var set = new WBMultiSet<int>();
			set.Initialize(a, false);

			for (int i = 0; i < n; i++)
				set.GetAt(i).GetIndex();
		}

		#endregion

		[Fact]
		public void Initialize_Set()
		{
			var n = 200000;
			var expected = CreateValues(n, 130000).Distinct().ToArray();

			var set = new WBSet<int>();
			Assert.Equal(0, set.Count);
			set.Initialize(expected);
			Assert.Equal(expected.Length, set.Count);

			Array.Sort(expected);
			Assert.Equal(expected, set);
			Assert.Equal(expected, set.GetItems());
			Array.Reverse(expected);
			Assert.Equal(expected, set.GetItemsDescending());

			set.Clear();
			Assert.Equal(0, set.Count);
			Assert.Equal(Array.Empty<int>(), set);
		}

		[Fact]
		public void Initialize_Map()
		{
			var n = 100000;
			var expected = CreateValues(n, 70000).Distinct().Select(key => new KeyValuePair<int, int>(key, random.Next(1000000))).ToArray();

			var map = new WBMap<int, int>();
			Assert.Equal(0, map.Count);
			map.Initialize(expected);
			Assert.Equal(expected.Length, map.Count);

			expected = expected.OrderBy(p => p.Key).ToArray();
			Assert.Equal(expected, map);
			Assert.Equal(expected, map.GetItems());
			Array.Reverse(expected);
			Assert.Equal(expected, map.GetItemsDescending());

			map.Clear();
			Assert.Equal(0, map.Count);
			Assert.Equal(expected[..0], map);
		}

		[Fact]
		public void Initialize_MultiSet()
		{
			var n = 100000;
			var expected = CreateValues(n, 100);

			var set = new WBMultiSet<int>();
			Assert.Equal(0, set.Count);
			set.Initialize(expected);
			Assert.Equal(expected.Length, set.Count);

			Array.Sort(expected);
			Assert.Equal(expected, set);
			Assert.Equal(expected, set.GetItems());
			Array.Reverse(expected);
			Assert.Equal(expected, set.GetItemsDescending());

			set.Clear();
			Assert.Equal(0, set.Count);
			Assert.Equal(Array.Empty<int>(), set);
		}

		[Fact]
		public void Initialize_MultiMap()
		{
			var n = 50000;
			var expected = CreateItems(n, 100);

			var map = new WBMultiMap<int, int>();
			Assert.Equal(0, map.Count);
			map.Initialize(expected);
			Assert.Equal(expected.Length, map.Count);

			// stable sort
			expected = expected.OrderBy(p => p.Key).ToArray();
			Assert.Equal(expected, map);
			Assert.Equal(expected, map.GetItems());
			Array.Reverse(expected);
			Assert.Equal(expected, map.GetItemsDescending());

			map.Clear();
			Assert.Equal(0, map.Count);
			Assert.Equal(expected[..0], map);
		}

		[Fact]
		public void Initialize_Exception_Set()
		{
			var a = new[] { 1, -1 };
			var comparer = ComparerHelper<int>.Create(Math.Abs);
			var set = new WBSet<int>(comparer);
			Assert.Throws<ArgumentException>(() => set.Initialize(a));
		}

		[Fact]
		public void Initialize_Exception_Map()
		{
			var d = new Dictionary<int, int> { { 1, 2 }, { -1, 3 } };
			var comparer = ComparerHelper<int>.Create(Math.Abs);
			var map = new WBMap<int, int>(comparer);
			Assert.Throws<ArgumentException>(() => map.Initialize(d));
		}

		[Fact]
		public void GetRemoveItems_Predicate()
		{
			var n = 1000;
			var max = 1000;
			var a = CreateValues(n, max);

			var set = new WBMultiSet<int>();
			set.Initialize(a);
			Array.Sort(a);
			var l = new List<int>(a);

			for (int k = 0; k < 1000; k++)
			{
				var (v1, v2) = (random.Next(max), random.Next(max));
				var expected = Array.FindAll(a, x => x >= v1 && x <= v2);
				Assert.Equal(expected, set.GetItems(x => x >= v1, x => x <= v2));
				Array.Reverse(expected);
				Assert.Equal(expected, set.GetItemsDescending(x => x >= v1, x => x <= v2));
				Assert.Equal(expected.Length, set.GetCount(x => x >= v1, x => x <= v2));
			}
			for (int k = 0; k < 1000; k++)
			{
				var (v1, v2) = (random.Next(max), random.Next(max));
				var expected = l.RemoveAll(x => x >= v1 && x <= v2);
				Assert.Equal(expected, set.RemoveItems(x => x >= v1, x => x <= v2));
				Assert.Equal(l, set);
			}
		}

		[Fact]
		public void GetItems_Predicate_Null()
		{
			var n = 1000;
			var max = 1000;
			var a = CreateValues(n, max);

			var set = new WBMultiSet<int>();
			set.Initialize(a);
			Array.Sort(a);
			Assert.Equal(a, set.GetItems(null, null));

			for (int k = 0; k < 1000; k++)
			{
				var v = random.Next(max);
				var expected = Array.FindAll(a, x => x > v);
				Assert.Equal(expected, set.GetItems(x => x > v, null));
				Array.Reverse(expected);
				Assert.Equal(expected, set.GetItemsDescending(x => x > v, null));
				Assert.Equal(expected.Length, set.GetCount(x => x > v, null));
			}
			for (int k = 0; k < 1000; k++)
			{
				var v = random.Next(max);
				var expected = Array.FindAll(a, x => x < v);
				Assert.Equal(expected, set.GetItems(null, x => x < v));
				Array.Reverse(expected);
				Assert.Equal(expected, set.GetItemsDescending(null, x => x < v));
				Assert.Equal(expected.Length, set.GetCount(null, x => x < v));
			}
		}

		[Fact]
		public void GetRemove_FirstLast_Set()
		{
			var n = 1000;
			var max = 1000;
			var l = new List<int>();
			var set = new WBSet<int>();

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
		public void GetRemove_FirstLast_Predicate()
		{
			var n = 1000;
			var max = 1000;
			var l = new List<int>();
			var set = new WBMultiSet<int>();

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

		[Fact]
		public void GetFirstIndex_Predicate()
		{
			var n = 100000;
			var max = 150000;
			var a = CreateValues(n, max);
			Array.Sort(a);

			var set = new WBMultiSet<int>();
			Assert.Equal(n, set.AddItems(a));

			var expected = new int[max];
			Array.Fill(expected, n);
			for (int i = 0, v = 0; i < n; i++)
				while (v <= a[i]) expected[v++] = i;

			Assert.Equal(expected, Enumerable.Range(0, max).Select(v => set.GetFirstIndex(x => x >= v)));
		}

		[Fact]
		public void GetLastIndex_Predicate()
		{
			var n = 100000;
			var max = 150000;
			var a = CreateValues(n, max);
			Array.Sort(a);

			var set = new WBMultiSet<int>();
			Assert.Equal(n, set.AddItems(a));

			var expected = new int[max];
			Array.Fill(expected, -1);
			for (int i = n - 1, v = max - 1; i >= 0; i--)
				while (v >= a[i]) expected[v--] = i;

			Assert.Equal(expected, Enumerable.Range(0, max).Select(v => set.GetLastIndex(x => x <= v)));
		}

		[Fact]
		public void AddItems()
		{
			var n = 100000;
			var expected = CreateValues(n, 100);

			var set = new WBMultiSet<int>();
			Assert.Equal(n, set.AddItems(expected));
			Assert.Equal(expected.Length, set.Count);

			Array.Sort(expected);
			Assert.Equal(expected, set);
		}
	}
}
