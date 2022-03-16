using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.AOJ
{
	// Test: https://onlinejudge.u-aizu.ac.jp/courses/lesson/8/ITP2/6/ITP2_6_C
	class BS_ITP2_6_C
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			Console.ReadLine();
			var a = Read();
			var qc = int.Parse(Console.ReadLine());
			var qs = new bool[qc].Select(_ => int.Parse(Console.ReadLine()));

			var set = new WBMultiSet<int>();
			set.Initialize(a, true);
			return string.Join("\n", qs.Select(k => set.GetFirstIndex(x => x >= k)));
		}
	}
}
