using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.Indexed;
using Xunit;

namespace UnitTest.Indexed
{
	public class IndexedTreeTest
	{
		static readonly Random random = new Random();
		static int[] CreateValues(int count, int max) => Array.ConvertAll(new bool[count], _ => random.Next(max));

		[Fact]
		public void Initialize()
		{
			var n = 200000;
			var expected = CreateValues(n, n);

			var set = new IndexedTree<int>();
			Assert.Equal(0, set.Count);
			set.Initialize(expected);
			Assert.Equal(expected.Length, set.Count);

			Assert.Equal(expected, set);
			Assert.Equal(expected, set.GetItems());
			Array.Reverse(expected);
			Assert.Equal(expected, set.GetItemsDescending());

			set.Clear();
			Assert.Equal(0, set.Count);
			Assert.Equal(Array.Empty<int>(), set);
		}

		[Fact]
		public void Initialize_IEnumerable()
		{
			var n = 200000;
			var expected = CreateValues(n, n);

			var set = new IndexedTree<int>();
			Assert.Equal(0, set.Count);
			set.Initialize(expected.Select(x => x));
			Assert.Equal(expected.Length, set.Count);

			Assert.Equal(expected, set);
			Assert.Equal(expected, set.GetItems());
			Array.Reverse(expected);
			Assert.Equal(expected, set.GetItemsDescending());

			set.Clear();
			Assert.Equal(0, set.Count);
			Assert.Equal(Array.Empty<int>(), set);
		}

		[Fact]
		public void GetItems_ByIndex()
		{
			var n = 1000;
			var a = CreateValues(n, 1000);

			var set = new IndexedTree<int>();
			set.Initialize(a);

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

			var set = new IndexedTree<int>();
			set.Initialize(l);

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

			var set = new IndexedTree<int>();
			set.Initialize(l);

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

			var set = new IndexedTree<int>();
			set.Initialize(l);

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

			var set = new IndexedTree<int>();
			set.Initialize(a);

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

			var set = new IndexedTree<int>();
			set.Initialize(a);

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

			var set = new IndexedTree<int>();
			set.Initialize(a);

			for (int i = 0; i < n; i++)
				set.GetAt(i).GetIndex();
		}
	}
}
