using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBPQ;

namespace OnlineTest.WBPQ
{
	// Test: https://atcoder.jp/contests/past202004-open/tasks/past202004_l
	class PQ_PAST202004_L
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static (int, int, int) Read3() { var a = Read(); return (a[0], a[1], a[2]); }
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var (n, k, d) = Read3();
			var a = Read();

			var r = new int[k];

			// 安定ソート
			var q = new WBPriorityQueue<int, int>(i => a[i]);
			int qi = 0, qi_end = n - k * d;
			var ai = -d;

			for (int i = 0; i < k; i++)
			{
				qi_end += d;
				for (; qi < qi_end; qi++) q.Push(qi);

				if (q.Count == 0) return -1;
				while (q.GetFirst().Value < ai + d) q.PopFirst();

				ai = q.PopFirst().Value;
				r[i] = a[ai];
			}

			return string.Join(" ", r);
		}
	}
}
