using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.YLC
{
	// Test: https://judge.yosupo.jp/problem/shortest_path
	class PQ_ShortestPath
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static (int, int, int, int) Read4() { var a = Read(); return (a[0], a[1], a[2], a[3]); }
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var (n, m, s, t) = Read4();
			var es = Array.ConvertAll(new bool[m], _ => Read());

			var spp = new SppWeightedGraph(n);
			spp.AddEdges(es, true);
			var (d, from) = spp.Dijkstra(s, t);
			if (d[t] == long.MaxValue) return -1;

			var path = GetPathVertexes(from, t);
			return $"{d[t]} {path.Length - 1}\n" + string.Join("\n", Enumerable.Range(0, path.Length - 1).Select(i => $"{path[i]} {path[i + 1]}"));
		}

		static int[] GetPathVertexes(int[] from, int ev)
		{
			var path = new Stack<int>();
			for (var v = ev; v != -1; v = from[v])
				path.Push(v);
			return path.ToArray();
		}
	}

	public class SppWeightedGraph
	{
		public struct Edge
		{
			public static readonly Edge Invalid = new Edge(-1, -1, long.MinValue);

			public int From, To;
			public long Cost;
			public Edge(int from, int to, long cost) { From = from; To = to; Cost = cost; }
			public Edge(int[] e) : this(e[0], e[1], e[2]) { }
			public Edge(long[] e) : this((int)e[0], (int)e[1], e[2]) { }
			public Edge GetReverse() => new Edge(To, From, Cost);
		}

		static readonly Edge[] EmptyEdges = new Edge[0];

		public int VertexesCount { get; }
		// map[v] が null である可能性があります。
		List<Edge>[] map;

		public SppWeightedGraph(int n)
		{
			VertexesCount = n;
			map = new List<Edge>[n];
		}

		public Edge[][] GetMap() => Array.ConvertAll(map, l => l?.ToArray() ?? EmptyEdges);

		public void AddEdge(int from, int to, long cost, bool directed) => AddEdge(new Edge(from, to, cost), directed);
		public void AddEdge(Edge e, bool directed)
		{
			var l = map[e.From] ?? (map[e.From] = new List<Edge>());
			l.Add(e);

			if (directed) return;
			l = map[e.To] ?? (map[e.To] = new List<Edge>());
			l.Add(e.GetReverse());
		}

		public void AddEdges(IEnumerable<int[]> es, bool directed)
		{
			foreach (var e in es) AddEdge(new Edge(e), directed);
		}
		public void AddEdges(IEnumerable<long[]> es, bool directed)
		{
			foreach (var e in es) AddEdge(new Edge(e), directed);
		}
		public void AddEdges(IEnumerable<Edge> es, bool directed)
		{
			foreach (var e in es) AddEdge(e, directed);
		}

		public (long[] d, int[] from) Dijkstra(int sv, int ev = -1) => Dijkstra(VertexesCount, v => map[v]?.ToArray() ?? EmptyEdges, sv, ev);

		// 終点を指定しないときは、-1 を指定します。
		public static (long[] d, int[] from) Dijkstra(int n, Func<int, Edge[]> nexts, int sv, int ev = -1)
		{
			var costs = Array.ConvertAll(new bool[n], _ => long.MaxValue);
			var from = Array.ConvertAll(costs, _ => -1);
			var q = new WBMultiMap<long, int>();
			costs[sv] = 0;
			q.Add(0, sv);

			while (q.Count > 0)
			{
				var (c, v) = q.RemoveFirst().Item;
				if (v == ev) break;
				if (costs[v] < c) continue;

				foreach (var e in nexts(v))
				{
					var (nv, nc) = (e.To, c + e.Cost);
					if (costs[nv] <= nc) continue;
					costs[nv] = nc;
					from[nv] = v;
					q.Add(nc, nv);
				}
			}
			return (costs, from);
		}
	}
}
