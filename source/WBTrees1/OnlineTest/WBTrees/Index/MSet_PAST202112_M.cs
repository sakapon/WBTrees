using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.Index
{
	// Test: https://atcoder.jp/contests/past202112-open/tasks/past202112_m
	class MSet_PAST202112_M
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static (int, int) Read2() { var a = Read(); return (a[0], a[1]); }
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var (n, qc) = Read2();
			var s = Console.ReadLine().Split();

			// ValueTuple に対する既定の Comparer よりも高速です。
			var comparer = ComparerHelper<(string name, int id)>.Create(p => p.name, false, p => p.id, false);
			var set = new WBSet<(string name, int id)>(comparer);
			set.Initialize(Enumerable.Range(0, n).Select(id => (s[id], id)));

			while (qc-- > 0)
			{
				var q = Console.ReadLine().Split();
				var x = int.Parse(q[0]) - 1;
				var (_, id) = set.RemoveAt(x).Item;
				set.Add((q[1], id));
			}

			return string.Join(" ", set.OrderBy(p => p.id).Select(p => p.name));
		}
	}
}
