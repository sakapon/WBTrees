using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBLL;

namespace OnlineTest.WBLL
{
	// Test: https://atcoder.jp/contests/abc148/tasks/abc148_d
	class LL_ABC148_D
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var n = int.Parse(Console.ReadLine());
			var a = Read();

			var l = new WBLinkedList<int>();
			l.Initialize(a);

			while (l.Count > 0 && l.GetFirst().Item != 1)
			{
				l.RemoveFirst();
			}
			if (l.Count == 0) return -1;

			for (Node<int> ln = l.GetFirst(), nn; (nn = ln.GetNext()) != null;)
			{
				if (nn.Item == ln.Item + 1)
				{
					ln = nn;
				}
				else
				{
					l.Remove(nn);
				}
			}

			return n - l.Count;
		}
	}
}
