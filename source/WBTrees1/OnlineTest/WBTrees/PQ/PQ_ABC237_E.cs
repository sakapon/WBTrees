using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.PQ
{
	// Test: https://atcoder.jp/contests/abc237/tasks/abc237_e
	class PQ_ABC237_E
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static (int u, int v) Read2() { var a = Read(); return (a[0], a[1]); }
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var (n, m) = Read2();
			var h = Read();
			var es = Array.ConvertAll(new bool[m], _ => Read2());

			var spp = new SppWeightedGraph(n + 1);

			foreach (var (u, v) in es)
			{
				var hu = h[u - 1];
				var hv = h[v - 1];

				spp.AddEdge(u, v, hu < hv ? hv - hu : 0, true);
				spp.AddEdge(v, u, hv < hu ? hu - hv : 0, true);
			}

			var d = spp.Dijkstra(1);
			return Enumerable.Range(1, n).Max(v => h[0] - h[v - 1] - d[v]);
		}
	}
}
