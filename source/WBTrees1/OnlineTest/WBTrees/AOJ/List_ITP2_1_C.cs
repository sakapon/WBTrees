using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.AOJ
{
	// Test: https://onlinejudge.u-aizu.ac.jp/courses/lesson/8/ITP2/1/ITP2_1_C
	class List_ITP2_1_C
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var qc = int.Parse(Console.ReadLine());

			var l = new WBList<int>();
			var i = 0;

			while (qc-- > 0)
			{
				var q = Read();
				if (q[0] == 0)
					l.Insert(i, q[1]);
				else if (q[0] == 1)
					i += q[1];
				else
					l.RemoveAt(i);
			}

			return string.Join("\n", l);
		}
	}
}
