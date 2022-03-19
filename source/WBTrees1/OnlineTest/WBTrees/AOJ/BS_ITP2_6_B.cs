using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.AOJ
{
	// Test: https://onlinejudge.u-aizu.ac.jp/courses/lesson/8/ITP2/6/ITP2_6_B
	class BS_ITP2_6_B
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			Console.ReadLine();
			var a = Read();
			Console.ReadLine();
			var b = Read();

			var set = new WBSet<int>();
			set.Initialize(a, true);
			return b.All(set.Contains) ? 1 : 0;
		}
	}
}
