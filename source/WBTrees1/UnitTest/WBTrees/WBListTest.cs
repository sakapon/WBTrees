using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;
using Xunit;

namespace UnitTest.WBTrees
{
	public class WBListTest
	{
		static readonly Random random = new();
		static int[] CreateValues(int count, int max) => Array.ConvertAll(new bool[count], _ => random.Next(max));

		#region From IndexedTreeTest

		[Fact]
		public void Initialize()
		{
			var n = 200000;
			var expected = CreateValues(n, n);

			var wl = new WBList<int>();
			Assert.Equal(0, wl.Count);
			wl.Initialize(expected);
			Assert.Equal(expected.Length, wl.Count);

			Assert.Equal(expected, wl);
			Assert.Equal(expected, wl.GetItems());
			Array.Reverse(expected);
			Assert.Equal(expected, wl.GetItemsDescending());

			wl.Clear();
			Assert.Equal(0, wl.Count);
			Assert.Equal(Array.Empty<int>(), wl);
		}

		[Fact]
		public void Initialize_IEnumerable()
		{
			var n = 200000;
			var expected = CreateValues(n, n);

			var wl = new WBList<int>();
			Assert.Equal(0, wl.Count);
			wl.Initialize(expected.Select(x => x));
			Assert.Equal(expected.Length, wl.Count);

			Assert.Equal(expected, wl);
			Assert.Equal(expected, wl.GetItems());
			Array.Reverse(expected);
			Assert.Equal(expected, wl.GetItemsDescending());

			wl.Clear();
			Assert.Equal(0, wl.Count);
			Assert.Equal(Array.Empty<int>(), wl);
		}

		[Fact]
		public void GetItems_ByIndex()
		{
			var n = 1000;
			var a = CreateValues(n, 1000);

			var wl = new WBList<int>();
			wl.Initialize(a);

			for (int k = 0; k < 1000; k++)
			{
				var (i1, i2) = (random.Next(-10, n + 10), random.Next(-10, n + 10));
				var j1 = Math.Clamp(i1, 0, n);
				var j2 = Math.Clamp(i2, 0, n);
				j1 = Math.Min(j1, j2);

				var expected = a[j1..j2];
				Assert.Equal(expected, wl.GetItems(i1, i2));
				Array.Reverse(expected);
				Assert.Equal(expected, wl.GetItemsDescending(i1, i2));
			}
		}

		[Fact]
		public void RemoveItems_ByIndex()
		{
			var n = 1000;
			var l = CreateValues(n, 1000).ToList();

			var wl = new WBList<int>();
			wl.Initialize(l);

			for (int k = 0; k < 1000; k++)
			{
				var (i1, i2) = (random.Next(-10, n + 10), random.Next(-10, n + 10));
				var j1 = Math.Clamp(i1, 0, l.Count);
				var j2 = Math.Clamp(i2, 0, l.Count);
				j1 = Math.Min(j1, j2);

				var count = j2 - j1;
				l.RemoveRange(j1, count);
				Assert.Equal(count, wl.RemoveItems(i1, i2));
				Assert.Equal(l, wl);
			}
		}

		[Fact]
		public void GetRemove_FirstLast()
		{
			var n = 1000; // even
			var l = CreateValues(n, 1000).ToList();

			var wl = new WBList<int>();
			wl.Initialize(l);

			while (l.Count > 0)
			{
				Assert.Equal(l[0], wl.GetFirst().Item);
				Assert.Equal(l[0], wl.RemoveFirst().Item);
				l.RemoveAt(0);
				Assert.Equal(l.Count, wl.Count);
				Assert.Equal(l, wl);

				Assert.Equal(l[^1], wl.GetLast().Item);
				Assert.Equal(l[^1], wl.RemoveLast().Item);
				l.RemoveAt(l.Count - 1);
				Assert.Equal(l.Count, wl.Count);
				Assert.Equal(l, wl);
			}

			Assert.Null(wl.GetFirst());
			Assert.Null(wl.RemoveFirst());
			Assert.Null(wl.GetLast());
			Assert.Null(wl.RemoveLast());
		}

		[Fact]
		public void GetRemove_At()
		{
			var n = 1000;
			var l = CreateValues(n, 1000).ToList();

			var wl = new WBList<int>();
			wl.Initialize(l);

			for (int c = n; c > 0; c--)
			{
				Assert.Equal(l.Count, wl.Count);
				Assert.Equal(l, wl);
				for (int i = 0; i < c; i++)
					Assert.Equal(l[i], wl.GetAt(i).Item);

				for (int i = 0; i < 5; i++)
				{
					Assert.Null(wl.GetAt(-1 - i));
					Assert.Null(wl.RemoveAt(-1 - i));
					Assert.Null(wl.GetAt(c + i));
					Assert.Null(wl.RemoveAt(c + i));
				}

				var index = random.Next(c);
				Assert.Equal(l[index], wl.RemoveAt(index).Item);
				l.RemoveAt(index);
			}

			Assert.Equal(l.Count, wl.Count);
			Assert.Equal(l, wl);
			Assert.Null(wl.GetAt(0));
			Assert.Null(wl.RemoveAt(0));
		}

		[Fact]
		public void GetIndex()
		{
			var n = 1000;
			var a = CreateValues(n, 1000);

			var wl = new WBList<int>();
			wl.Initialize(a);

			for (int c = n; c > 0; c--)
			{
				for (int i = 0; i < c; i++)
					Assert.Equal(i, wl.GetAt(i).GetIndex());

				var index = random.Next(c);
				wl.RemoveAt(index);
			}
		}

		[Fact]
		public void Time_GetRemoveAt()
		{
			var n = 200000;
			var a = CreateValues(n, 10000);

			var wl = new WBList<int>();
			wl.Initialize(a);

			for (int c = n; c > 0; c--)
			{
				var index = random.Next(c);
				wl.GetAt(index);
				wl.RemoveAt(index);
			}
		}

		[Fact]
		public void Time_GetIndex()
		{
			var n = 200000;
			var a = CreateValues(n, 10000);

			var wl = new WBList<int>();
			wl.Initialize(a);

			for (int i = 0; i < n; i++)
				wl.GetAt(i).GetIndex();
		}

		#endregion

		[Fact]
		public void GetSet()
		{
			var n = 1000;
			var max = 1000;
			var a = CreateValues(n, max);

			var wl = new WBList<int>();
			wl.Initialize(a);
			var l = new List<int>(a);

			for (int k = 0; k < 1000; k++)
			{
				var (index, v) = (random.Next(n), random.Next(max));
				l[index] = v;
				wl[index] = v;
				Assert.Equal(l.Count, wl.Count);
				for (int i = 0; i < n; i++)
					Assert.Equal(l[i], wl[i]);
			}
		}

		[Fact]
		public void GetSet_Exception()
		{
			var n = 1000;
			var max = 1000;

			var wl = new WBList<int>();

			for (int c = 0, v; c < n; c++)
			{
				for (int i = 0; i < c; i++)
					v = wl[i];

				for (int i = 0; i < 5; i++)
				{
					Assert.Throws<ArgumentOutOfRangeException>(() => wl[-1 - i]);
					Assert.Throws<ArgumentOutOfRangeException>(() => wl[c + i]);
				}

				v = random.Next(max);
				wl.Add(v);
			}
		}

		[Fact]
		public void Insert()
		{
			var n = 1000;
			var max = 1000;

			var wl = new WBList<int>();
			var l = new List<int>();

			for (int c = 1; c <= n; c++)
			{
				var (index, v) = (random.Next(c), random.Next(max));
				l.Insert(index, v);
				Assert.Equal(v, wl.Insert(index, v).Item);
				Assert.Equal(l.Count, wl.Count);
				Assert.Equal(l, wl);
			}
		}

		[Fact]
		public void InsertItems()
		{
			var n = 1000;
			var max = 1000;

			var wl = new WBList<int>();
			var l = new List<int>();

			for (int k = 0; k < 1000; k++)
			{
				var index = random.Next(l.Count + 1);
				var a = CreateValues(random.Next(n - l.Count + 1), max);
				l.InsertRange(index, a);
				Assert.Equal(a.Length, wl.InsertItems(index, a));
				Assert.Equal(l.Count, wl.Count);
				Assert.Equal(l, wl);
			}
		}
	}
}
