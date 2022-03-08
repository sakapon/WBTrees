using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.Index
{
	// Test: https://atcoder.jp/contests/abc234/tasks/abc234_d
	class Set_ABC234_D
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static (int, int) Read2() { var a = Read(); return (a[0], a[1]); }
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var (n, k) = Read2();
			var p = Read();

			var r = new int[n - k + 1];
			var set = new WBSet<int>();

			for (int i = 1; i <= n; i++)
			{
				set.Add(p[i - 1]);
				if (i >= k) r[i - k] = set.GetAt(i - k).Item;
			}
			return string.Join("\n", r);
		}
	}
}
