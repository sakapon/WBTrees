using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.AvlTrees201;

namespace OnlineTest.AvlTrees201.PQ
{
	// Test: https://atcoder.jp/contests/abc223/tasks/abc223_d
	class PQ_ABC223_D
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static (int a, int b) Read2() { var a = Read(); return (a[0], a[1]); }
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var (n, m) = Read2();
			var es = Array.ConvertAll(new bool[m], _ => Read2());

			var r = new List<int>();
			var set = new AvlSet<int>();

			var map = Array.ConvertAll(new bool[n + 1], _ => new List<int>());
			var counts = new int[n + 1];
			foreach (var (a, b) in es)
			{
				map[a].Add(b);
				counts[b]++;
			}

			for (int i = 1; i <= n; i++)
			{
				if (counts[i] == 0)
				{
					set.Add(i);
				}
			}

			while (set.RemoveFirst().TryGetItem(out var v))
			{
				r.Add(v);

				foreach (var nv in map[v])
				{
					if (--counts[nv] == 0)
					{
						set.Add(nv);
					}
				}
			}

			if (r.Count < n) return -1;
			return string.Join(" ", r);
		}
	}
}
