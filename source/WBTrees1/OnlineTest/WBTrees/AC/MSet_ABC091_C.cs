using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.AC
{
	// Test: https://atcoder.jp/contests/abc091/tasks/arc092_a
	class MSet_ABC091_C
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var n = Read()[0];
			var ab = new bool[n].Select(_ => Read()).Select(a => (a[0], a[1], 0));
			var cd = new bool[n].Select(_ => Read()).Select(a => (a[0], a[1], 1));

			var qs = ab.Concat(cd).OrderBy(p => p);
			var set = new WBMultiSet<int>();

			foreach (var (x, y, t) in qs)
			{
				if (t == 0)
				{
					set.Add(y);
				}
				else
				{
					set.RemoveLast(v => v <= y);
				}
			}
			return n - set.Count;
		}
	}
}
