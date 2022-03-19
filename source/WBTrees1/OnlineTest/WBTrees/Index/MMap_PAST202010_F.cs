using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.Index
{
	// Test: https://atcoder.jp/contests/past202010-open/tasks/past202010_f
	class MMap_PAST202010_F
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static (int, int) Read2() { var a = Read(); return (a[0], a[1]); }
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var (n, k) = Read2();
			var ss = Array.ConvertAll(new bool[n], _ => Console.ReadLine());

			var map = new WBMultiMap<int, string>();
			map.Initialize(ss.GroupBy(s => s).Select(g => (g.Count(), g.Key)));

			var (count, r) = map.GetAt(map.Count - k).Item;
			return map.GetCount(count) == 1 ? r : "AMBIGUOUS";
		}
	}
}
