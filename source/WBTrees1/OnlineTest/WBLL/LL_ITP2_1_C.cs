using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBLL;

namespace OnlineTest.WBLL
{
	// Test: https://onlinejudge.u-aizu.ac.jp/courses/lesson/8/ITP2/1/ITP2_1_C
	class LL_ITP2_1_C
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var qc = int.Parse(Console.ReadLine());

			var l = new WBLinkedList<int>();
			var node = l.Add(0);

			while (qc-- > 0)
			{
				var q = Read();
				if (q[0] == 0)
				{
					node = l.AddBefore(node, q[1]);
				}
				else if (q[0] == 1)
				{
					if (q[1] >= 0)
						while (q[1]-- > 0)
							node = node.GetNext();
					else
						while (q[1]++ < 0)
							node = node.GetPrevious();
				}
				else
				{
					var t = node;
					node = node.GetNext();
					l.Remove(t);
				}
			}

			l.RemoveLast();
			return string.Join("\n", l);
		}
	}
}
