using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.Index
{
	// Test: https://atcoder.jp/contests/abc231/tasks/abc231_f
	class MSet_ABC231_F
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var n = int.Parse(Console.ReadLine());
			var a = Read();
			var b = Read();

			var ab = Enumerable.Range(0, n).Select(i => (a: a[i], b: b[i])).ToArray();
			var set = new WBMultiSet<int>();
			var r = 0L;

			foreach (var (_, v) in ab.OrderBy(p => p.a).ThenByDescending(p => p.b))
			{
				set.Add(v);
				r += set.GetCount(x => x >= v, null);
			}
			foreach (var g in ab.GroupBy(p => p))
			{
				var c = g.LongCount();
				r += c * (c - 1) / 2;
			}
			return r;
		}
	}
}
