using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;
using Xunit;

namespace UnitTest.WBTrees
{
	public class WBMultiSetTest
	{
		static readonly Random random = new();
		static int[] CreateValues(int count, int max) => Array.ConvertAll(new bool[count], _ => random.Next(max));

		[Fact]
		public void AddRemove()
		{
			var n = 1000;
			var a = CreateValues(n, 1000);

			var set = new WBMultiSet<int>();
			Assert.Equal(0, set.Count);

			for (int c = 1; c <= n; c++)
			{
				Assert.Equal(a[c - 1], set.Add(a[c - 1]).Item);
				Assert.Equal(c, set.Count);
				Assert.Equal(a[..c].OrderBy(x => x), set);
			}
			for (int c = 1; c <= n; c++)
			{
				Assert.True(set.Remove(a[c - 1]));
				Assert.Equal(n - c, set.Count);
				Assert.Equal(a[c..].OrderBy(x => x), set);
			}
		}

		[Fact]
		public void RemoveAll()
		{
			var n = 1000;
			var a = CreateValues(n, 1000);

			var set = new WBMultiSet<int>();
			set.Initialize(a);
			var l = set.ToList();

			foreach (var v in a)
			{
				var expected = l.RemoveAll(x => x == v);
				var actual = set.RemoveAll(v);
				Assert.Equal(expected, actual);
				Assert.Equal(l.Count, set.Count);
				Assert.Equal(l, set);
			}
			Assert.Equal(0, set.Count);
		}

		[Fact]
		public void GetFirstIndex_ByItem()
		{
			var n = 100000;
			var max = 150000;
			var a = CreateValues(n, max);
			Array.Sort(a);

			var set = new WBMultiSet<int>();
			Assert.Equal(n, set.AddItems(a));

			var expected = new int[max];
			Array.Fill(expected, -1);
			for (int i = n - 1; i >= 0; i--)
				expected[a[i]] = i;

			Assert.Equal(expected, Enumerable.Range(0, max).Select(set.GetFirstIndex));
		}

		[Fact]
		public void GetLastIndex_ByItem()
		{
			var n = 100000;
			var max = 150000;
			var a = CreateValues(n, max);
			Array.Sort(a);

			var set = new WBMultiSet<int>();
			Assert.Equal(n, set.AddItems(a));

			var expected = new int[max];
			Array.Fill(expected, -1);
			for (int i = 0; i < n; i++)
				expected[a[i]] = i;

			Assert.Equal(expected, Enumerable.Range(0, max).Select(set.GetLastIndex));
		}

		[Fact]
		public void GetCount()
		{
			var n = 100000;
			var max = 100000;
			var a = CreateValues(n, max);
			var expected = a.GroupBy(x => x).ToDictionary(g => g.Key, g => g.Count());

			var set = new WBMultiSet<int>();
			set.Initialize(a);

			for (int v = 0; v < max; v++)
			{
				Assert.Equal(expected.ContainsKey(v), set.Contains(v));
				Assert.Equal(expected.ContainsKey(v) ? expected[v] : 0, set.GetCount(v));
			}
		}

		[Fact]
		public void Time_Add()
		{
			var n = 200000;
			var a = CreateValues(n, 10);

			var set = new WBMultiSet<int>();
			foreach (var v in a) set.Add(v);
		}

		[Fact]
		public void Time_Remove()
		{
			var n = 200000;
			var a = CreateValues(n, 10);

			var set = new WBMultiSet<int>();
			set.Initialize(a);
			foreach (var v in a) set.Remove(v);
		}
	}
}
