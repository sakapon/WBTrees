using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.AvlTrees201;
using Xunit;

namespace UnitTest.AvlTrees201
{
	public class AvlMultiSetTest
	{
		static readonly Random random = new();
		static int[] CreateValues(int count, int max) => Array.ConvertAll(new bool[count], _ => random.Next(max));

		[Fact]
		public void AddRemove()
		{
			var n = 1000;
			var a = CreateValues(n, 1000);

			var set = new AvlMultiSet<int>();
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

			var set = new AvlMultiSet<int>();
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
		public void Time_Add()
		{
			var n = 200000;
			var a = CreateValues(n, 10);

			var set = new AvlMultiSet<int>();
			foreach (var v in a) set.Add(v);
		}

		[Fact]
		public void Time_Remove()
		{
			var n = 200000;
			var a = CreateValues(n, 10);

			var set = new AvlMultiSet<int>();
			set.Initialize(a);
			foreach (var v in a) set.Remove(v);
		}
	}
}
