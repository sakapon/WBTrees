using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.AvlTrees;
using Xunit;

namespace UnitTest.AvlTrees
{
	public class AvlSetTest
	{
		static readonly Random random = new();
		static int[] CreateValues(int count, int max) => Array.ConvertAll(new bool[count], _ => random.Next(max));

		[Fact]
		public void AddRemove()
		{
			var n = 1000;
			var a = CreateValues(n, 1000);

			var hs = new HashSet<int>();
			var set = new AvlSet<int>();
			Assert.Equal(0, set.Count);

			foreach (var v in a)
			{
				Assert.Equal(hs.Add(v), set.Add(v));
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
		public void Time_Add()
		{
			var n = 200000;
			var a = CreateValues(n, 100000);

			var set = new AvlSet<int>();
			foreach (var v in a) set.Add(v);
		}

		[Fact]
		public void Time_Remove()
		{
			var n = 200000;
			var a = CreateValues(n, 130000).Distinct().ToArray();

			var set = new AvlSet<int>();
			set.Initialize(a);
			foreach (var v in a) set.Remove(v);
		}
	}
}
