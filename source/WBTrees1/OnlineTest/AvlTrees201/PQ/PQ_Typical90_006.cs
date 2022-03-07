using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.AvlTrees201;

namespace OnlineTest.AvlTrees201.PQ
{
	// Test: https://atcoder.jp/contests/typical90/tasks/typical90_f
	class PQ_Typical90_006
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static (int, int) Read2() { var a = Read(); return (a[0], a[1]); }
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var (n, k) = Read2();
			var s = Console.ReadLine();

			var cs = new char[k];

			// 安定ソート
			var map = new AvlMultiMap<char, int>();
			for (int i = 0; i < n - k; i++)
			{
				map.Add(s[i], i);
			}

			var t = -1;
			for (int i = n - k; i < n; i++)
			{
				map.Add(s[i], i);

				while (true)
				{
					var j = map.RemoveFirst().Item.Value;
					if (j < t) continue;

					cs[i - n + k] = s[t = j];
					break;
				}
			}

			return new string(cs);
		}
	}
}
