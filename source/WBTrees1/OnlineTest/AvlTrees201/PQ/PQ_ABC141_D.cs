using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.AvlTrees201;

namespace OnlineTest.AvlTrees201.PQ
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

			var set = new AvlMultiSet<long>();
			set.Initialize(a);
			while (m-- > 0) set.Add(set.RemoveLast().Item / 2);
			return set.Sum();
		}
	}
}
