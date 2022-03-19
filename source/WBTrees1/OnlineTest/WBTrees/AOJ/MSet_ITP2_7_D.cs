using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.AOJ
{
	// Flush による出力よりも高速です。
	// Test: https://onlinejudge.u-aizu.ac.jp/courses/lesson/8/ITP2/7/ITP2_7_D
	class MSet_ITP2_7_D
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var qc = int.Parse(Console.ReadLine());

			var r = new List<int>();
			var set = new WBMultiSet<int>();

			while (qc-- > 0)
			{
				var q = Read();
				if (q[0] == 0)
				{
					set.Add(q[1]);
					r.Add(set.Count);
				}
				else if (q[0] == 1)
				{
					r.Add(set.GetCount(q[1]));
				}
				else if (q[0] == 2)
				{
					set.RemoveAll(q[1]);
				}
				else
				{
					r.AddRange(set.GetItems(x => x >= q[1], x => x <= q[2]));
				}
			}

			return string.Join("\n", r);
		}
	}
}
