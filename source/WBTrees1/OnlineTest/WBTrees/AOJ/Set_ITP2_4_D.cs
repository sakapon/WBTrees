using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.AOJ
{
	// Test: https://onlinejudge.u-aizu.ac.jp/courses/lesson/8/ITP2/4/ITP2_4_D
	class Set_ITP2_4_D
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			Console.ReadLine();
			var a = Read();

			var set = new WBSet<int>();
			set.AddItems(a);
			return string.Join(" ", set);
		}
	}
}
