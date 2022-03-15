using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.AC
{
	// Test: https://atcoder.jp/contests/typical90/tasks/typical90_g
	class MSet_Typical90_007
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var n = int.Parse(Console.ReadLine());
			var a = Read();
			var q = int.Parse(Console.ReadLine());
			var b = Array.ConvertAll(new bool[q], _ => int.Parse(Console.ReadLine()));

			Array.Sort(a);
			var set = new WBMultiSet<int>();
			set.Initialize(a, true);
			set.Add(-1 << 30);
			set.Add(int.MaxValue);

			return string.Join("\n", b.Select(GetMin));

			int GetMin(int bv)
			{
				var av2 = set.GetFirst(x => x >= bv);
				var av1 = av2.GetPrevious();
				return Math.Min(av2.Item - bv, bv - av1.Item);
			}
		}
	}
}
