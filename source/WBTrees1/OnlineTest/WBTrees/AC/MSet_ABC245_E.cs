using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.AC
{
	// Test: https://atcoder.jp/contests/abc245/tasks/abc245_e
	class MSet_ABC245_E
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static (int, int) Read2() { var a = Read(); return (a[0], a[1]); }
		static void Main() => Console.WriteLine(Solve() ? "Yes" : "No");
		static bool Solve()
		{
			var (n, m) = Read2();
			var a = Read();
			var b = Read();
			var c = Read();
			var d = Read();

			var qs = a.Zip(b, (x, y) => (x, y, 0))
				.Concat(c.Zip(d, (x, y) => (x, y, 1)))
				.ToArray();
			Array.Sort(qs);

			var set = new WBMultiSet<int>();

			foreach (var (x, y, t) in qs)
			{
				if (t == 0)
				{
					set.Add(y);
				}
				else
				{
					set.RemoveLast(v => v <= y);
				}
			}
			return set.Count == 0;
		}
	}
}
