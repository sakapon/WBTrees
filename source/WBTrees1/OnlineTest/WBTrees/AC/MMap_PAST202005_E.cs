using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.AC
{
	// Test: https://atcoder.jp/contests/past202005-open/tasks/past202005_e
	class MMap_PAST202005_E
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static (int, int, int) Read3() { var a = Read(); return (a[0], a[1], a[2]); }
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var (n, m, qc) = Read3();
			var es = Array.ConvertAll(new bool[m], _ => Read());
			var c = Read().Prepend(0).ToArray();

			var map = new WBMultiMap<int, int>();
			foreach (var e in es)
			{
				map.Add(e[0], e[1]);
				map.Add(e[1], e[0]);
			}

			var r = new int[qc];
			for (int i = 0; i < qc; i++)
			{
				var s = Read();
				r[i] = c[s[1]];

				if (s[0] == 1)
				{
					foreach (var y in map[s[1]])
						c[y] = c[s[1]];
				}
				else
				{
					c[s[1]] = s[2];
				}
			}
			return string.Join("\n", r);
		}
	}
}
