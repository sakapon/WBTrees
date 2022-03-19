using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.Sort
{
	// Test: https://atcoder.jp/contests/past202112-open/tasks/past202112_d
	class Sort_PAST202112_D
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var n = int.Parse(Console.ReadLine());
			var a = Read();
			var b = Read();

			var comparer = ComparerHelper<int>.Create(i => a[i - 1] + b[i - 1], true, i => a[i - 1], true);
			var set = new WBMultiSet<int>(comparer);
			//set.Initialize(Enumerable.Range(1, n));
			set.AddItems(Enumerable.Range(1, n));

			return string.Join(" ", set);
		}
	}
}
