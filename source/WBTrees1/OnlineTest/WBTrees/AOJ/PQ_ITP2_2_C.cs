using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.AOJ
{
	// Test: https://onlinejudge.u-aizu.ac.jp/courses/lesson/8/ITP2/2/ITP2_2_C
	class PQ_ITP2_2_C
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static (int, int) Read2() { var a = Read(); return (a[0], a[1]); }
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var (n, qc) = Read2();

			var r = new List<int>();
			var qs = Array.ConvertAll(new bool[n], _ => new WBMultiSet<int>());

			while (qc-- > 0)
			{
				var q = Read();
				if (q[0] == 0)
				{
					qs[q[1]].Add(q[2]);
				}
				else if (q[0] == 1)
				{
					if (qs[q[1]].GetLast().TryGetItem(out var v)) r.Add(v);
				}
				else
				{
					qs[q[1]].RemoveLast();
				}
			}

			return string.Join("\n", r);
		}
	}
}
