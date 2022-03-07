using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.AC
{
	// Test: https://atcoder.jp/contests/abc228/tasks/abc228_d
	class Set_ABC228_D
	{
		static long[] ReadL() => Array.ConvertAll(Console.ReadLine().Split(), long.Parse);
		static (long, long) Read2L() { var a = ReadL(); return (a[0], a[1]); }
		static void Main()
		{
			var qc = int.Parse(Console.ReadLine());

			const int n = 1 << 20;
			var a = new long[n];
			Array.Fill(a, -1);

			var set = new WBSet<int>();
			set.Initialize(Enumerable.Range(0, n), false);

			Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()) { AutoFlush = false });
			while (qc-- > 0)
			{
				var (t, x) = Read2L();
				var xi = x % n;

				if (t == 1)
				{
					var node = set.RemoveFirst(v => v >= xi) ?? set.RemoveFirst();
					a[node.Item] = x;
				}
				else
				{
					Console.WriteLine(a[xi]);
				}
			}
			Console.Out.Flush();
		}
	}
}
