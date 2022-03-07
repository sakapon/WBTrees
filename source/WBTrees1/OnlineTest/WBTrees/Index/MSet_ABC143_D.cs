using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.Index
{
	// Test: https://atcoder.jp/contests/abc143/tasks/abc143_d
	class MSet_ABC143_D
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var n = int.Parse(Console.ReadLine());
			var l = Read();

			Array.Sort(l);
			var set = new WBMultiSet<int>();
			set.Initialize(l, false);

			var r = 0;
			for (int i = 0; i < n; i++)
				for (int j = i + 1; j < n; j++)
					r += set.GetLastIndex(x => x < l[i] + l[j]) - j;
			return r;
		}
	}
}
