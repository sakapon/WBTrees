using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.AOJ
{
	// Test: https://onlinejudge.u-aizu.ac.jp/courses/lesson/8/ITP2/1/ITP2_1_B
	class List_ITP2_1_B
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var qc = int.Parse(Console.ReadLine());

			var r = new List<int>();
			var l = new WBList<int>();

			while (qc-- > 0)
			{
				var q = Read();
				if (q[0] == 0)
				{
					if (q[1] == 0) l.Prepend(q[2]);
					else l.Add(q[2]);
				}
				else if (q[0] == 1)
				{
					r.Add(l[q[1]]);
				}
				else
				{
					if (q[1] == 0) l.RemoveFirst();
					else l.RemoveLast();
				}
			}

			return string.Join("\n", r);
		}
	}
}
