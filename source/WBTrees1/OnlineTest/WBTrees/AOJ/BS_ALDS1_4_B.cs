using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.AOJ
{
	// Test: https://onlinejudge.u-aizu.ac.jp/courses/lesson/1/ALDS1/4/ALDS1_4_B
	class BS_ALDS1_4_B
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			Console.ReadLine();
			var s = Read();
			Console.ReadLine();
			var t = Read();

			var set = new WBMultiSet<int>();
			set.Initialize(s, true);
			return t.Count(set.Contains);
		}
	}
}
