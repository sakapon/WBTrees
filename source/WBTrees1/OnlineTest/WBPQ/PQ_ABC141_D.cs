using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBPQ;

namespace OnlineTest.WBPQ
{
	// Test: https://atcoder.jp/contests/abc141/tasks/abc141_d
	class PQ_ABC141_D
	{
		static long[] ReadL() => Array.ConvertAll(Console.ReadLine().Split(), long.Parse);
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var m = ReadL()[1];
			var a = ReadL();

			var q = new WBPriorityQueue<long>();
			q.Initialize(a);
			while (m-- > 0) q.Push(q.PopLast() / 2);
			return q.Sum();
		}
	}
}
