using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.PQ
{
	// Test: https://atcoder.jp/contests/abc191/tasks/abc191_e
	class PQ_ABC191_E
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static (int, int) Read2() { var a = Read(); return (a[0], a[1]); }
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var (n, m) = Read2();
			var es = Array.ConvertAll(new bool[m], _ => Read());

			var spp = new SppWeightedGraph(n + 1);
			spp.AddEdges(es, true);
			var map = spp.GetMap();

			return string.Join("\n", Enumerable.Range(1, n).Select(GetCost));

			long GetCost(int ev)
			{
				var d = SppWeightedGraph.Dijkstra(n + 1, v => v == 0 ? map[ev] : map[v], 0, ev);
				if (d[ev] == long.MaxValue) return -1;
				return d[ev];
			}
		}
	}
}
