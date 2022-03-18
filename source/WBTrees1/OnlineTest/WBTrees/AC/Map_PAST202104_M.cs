using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.AC
{
	// Test: https://atcoder.jp/contests/past202104-open/tasks/past202104_m
	class Map_PAST202104_M
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static (int, int, int) Read3() { var a = Read(); return (a[0], a[1], a[2]); }
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var n = int.Parse(Console.ReadLine());
			var a = Read();
			var qc = int.Parse(Console.ReadLine());

			var d = a.GroupBy(x => x).ToDictionary(g => g.Key, g => g.LongCount());
			var sum = d.Values.Sum(c => c * (c - 1) / 2);

			void Add(int x, int delta)
			{
				var c = d.GetValueOrDefault(x);
				sum -= c * (c - 1) / 2;
				c += delta;
				sum += c * (c - 1) / 2;
				d[x] = c;
			}

			var sums = new long[qc];
			var map = new WBMap<int, int>();
			map.Initialize(GetIntervals(), true);

			IEnumerable<(int, int)> GetIntervals()
			{
				for (var (l, r) = (0, 1); r <= n; r++)
				{
					if (r == n || a[r] != a[r - 1])
					{
						yield return (l, r);
						l = r;
					}
				}
			}

			for (int i = 0; i < qc; i++)
			{
				var (L, R, X) = Read3();
				L--;

				// ŠÖ˜A‚·‚é‹æŠÔ‚ð‘S‚Äœ‚«‚Ü‚·B
				var ranges = map.GetItems(p => L < p.Value, p => p.Key < R).ToArray();
				map.RemoveItems(p => L < p.Value, p => p.Key < R);

				var (ll, _) = ranges[0];
				var (rl, rr) = ranges[^1];
				var lx = a[ll];
				var rx = a[rl];

				foreach (var (l, r) in ranges)
				{
					Add(a[l], -(r - l));
				}

				Add(lx, L - ll);
				Add(X, R - L);
				Add(rx, rr - R);

				a[L] = X;
				if (R < rr) a[R] = rx;

				if (ll < L) map.Add(ll, L);
				map.Add(L, R);
				if (R < rr) map.Add(R, rr);

				sums[i] = sum;
			}
			return string.Join("\n", sums);
		}
	}
}
