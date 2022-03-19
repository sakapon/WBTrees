using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBPQ;

namespace OnlineTest.WBPQ
{
	// Test: https://atcoder.jp/contests/typical90/tasks/typical90_f
	class PQ_Typical90_006
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static (int, int) Read2() { var a = Read(); return (a[0], a[1]); }
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var (n, k) = Read2();
			var s = Console.ReadLine();

			var cs = new char[k];

			// 安定ソート
			var q = new WBPriorityQueue<int, char>(i => s[i]);
			for (int i = 0; i < n - k; i++)
			{
				q.Push(i);
			}

			var t = -1;
			for (int i = n - k; i < n; i++)
			{
				q.Push(i);
				while (q.GetFirst().Value < t) q.PopFirst();

				t = q.PopFirst().Value;
				cs[i - n + k] = s[t];
			}

			return new string(cs);
		}
	}
}
