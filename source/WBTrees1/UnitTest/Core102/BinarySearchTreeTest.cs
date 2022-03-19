using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.Core102;
using Xunit;

namespace UnitTest.Core102
{
	public class BinarySearchTreeTest
	{
		static readonly Random random = new Random();
		static int[] CreateValues(int count, int max) => Array.ConvertAll(new bool[count], _ => random.Next(max));

		[Fact]
		public void Initialize()
		{
			var n = 100000;
			var expected = CreateValues(n, 10);

			var set = new BinarySearchTree<int>();
			Assert.Equal(0, set.Count);

			set.Initialize(expected);
			Array.Sort(expected);
			Assert.Equal(expected.Length, set.Count);
			Assert.Equal(expected, set);

			Array.Reverse(expected);
			Assert.Equal(expected, set.GetItemsDescending());
		}

		[Fact]
		public void AddRemove()
		{
			var n = 1000;
			var a = CreateValues(n, 1000);

			var set = new BinarySearchTree<int>();
			Assert.Equal(0, set.Count);

			for (int c = 1; c <= n; c++)
			{
				set.Add(a[c - 1]);
				Assert.Equal(c, set.Count);
				Assert.Equal(a[..c].OrderBy(x => x), set);
			}
			for (int c = 1; c <= n; c++)
			{
				set.Remove(set.Root.GetFirstNode(x => x >= a[c - 1]));
				Assert.Equal(n - c, set.Count);
				Assert.Equal(a[c..].OrderBy(x => x), set);
			}
		}

		[Fact]
		public void Time_Add()
		{
			// 値の重複度が上がると、木の高さが大きくなります。
			var n = 200000;
			var a = CreateValues(n, 40000);

			var set = new BinarySearchTree<int>();
			foreach (var v in a) set.Add(v);
		}

		[Fact]
		public void Time_Remove()
		{
			var n = 200000;
			var a = CreateValues(n, 10000);

			var set = new BinarySearchTree<int>();
			set.Initialize(a);
			foreach (var v in a) set.Remove(set.Root.GetFirstNode(x => x >= v));
		}
	}
}
