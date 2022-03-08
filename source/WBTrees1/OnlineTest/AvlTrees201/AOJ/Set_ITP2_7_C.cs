using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.AvlTrees201;

namespace OnlineTest.AvlTrees201.AOJ
{
	// Flush による出力よりも高速です。
	// Test: https://onlinejudge.u-aizu.ac.jp/courses/lesson/8/ITP2/7/ITP2_7_C
	class Set_ITP2_7_C
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var qc = int.Parse(Console.ReadLine());

			var r = new List<int>();
			var set = new AvlSet<int>();

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
					r.Add(set.Contains(q[1]) ? 1 : 0);
				}
				else if (q[0] == 2)
				{
					set.Remove(q[1]);
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
