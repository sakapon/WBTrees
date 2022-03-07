using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;
using Xunit;

namespace UnitTest.WBTrees
{
	public class WBSetTest
	{
		static readonly Random random = new();
		static int[] CreateValues(int count, int max) => Array.ConvertAll(new bool[count], _ => random.Next(max));

		[Fact]
		public void AddRemove()
		{
			var n = 1000;
			var a = CreateValues(n, 1000);

			var hs = new HashSet<int>();
			var set = new WBSet<int>();
			Assert.Equal(0, set.Count);

			foreach (var v in a)
			{
				Assert.Equal(hs.Add(v), set.Add(v).Exists());
				Assert.Equal(hs.Count, set.Count);
				Assert.Equal(hs.OrderBy(x => x), set);
			}
			foreach (var v in a)
			{
				Assert.Equal(hs.Remove(v), set.Remove(v));
				Assert.Equal(hs.Count, set.Count);
				Assert.Equal(hs.OrderBy(x => x), set);
			}
		}

		[Fact]
		public void GetIndex_ByItem()
		{
			var n = 100000;
			var max = 150000;
			var a = CreateValues(n, max).Distinct().ToArray();
			Array.Sort(a);

			var set = new WBSet<int>();
			Assert.Equal(a.Length, set.AddItems(a));

			var expected = new int[max];
			Array.Fill(expected, -1);
			for (int i = 0; i < a.Length; i++)
				expected[a[i]] = i;

			Assert.Equal(expected, Enumerable.Range(0, max).Select(set.GetIndex));
		}

		[Fact]
		public void GetCount()
		{
			var n = 200000;
			var max = 130000;
			var a = CreateValues(n, max).Distinct().ToArray();

			var hs = new HashSet<int>(a);
			var set = new WBSet<int>();
			set.Initialize(a);

			for (int v = 0; v < max; v++)
			{
				Assert.Equal(hs.Contains(v), set.Contains(v));
				Assert.Equal(hs.Contains(v) ? 1 : 0, set.GetCount(v));
			}
		}

		[Fact]
		public void Time_Add()
		{
			var n = 200000;
			var a = CreateValues(n, 130000);

			var set = new WBSet<int>();
			foreach (var v in a) set.Add(v);
		}

		[Fact]
		public void Time_Remove()
		{
			var n = 200000;
			var a = CreateValues(n, 130000);

			var set = new WBSet<int>();
			set.Initialize(a.Distinct());
			foreach (var v in a) set.Remove(v);
		}
	}
}
