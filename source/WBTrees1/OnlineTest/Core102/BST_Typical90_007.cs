using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.Core102;

namespace OnlineTest.Core102
{
	// Test: https://atcoder.jp/contests/typical90/tasks/typical90_g
	class BST_Typical90_007
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var n = int.Parse(Console.ReadLine());
			var a = Read();
			var q = int.Parse(Console.ReadLine());
			var b = Array.ConvertAll(new bool[q], _ => int.Parse(Console.ReadLine()));

			var set = new BinarySearchTree<int>();
			set.Initialize(a);
			set.Add(-1 << 30);
			set.Add(int.MaxValue);

			return string.Join("\n", b.Select(GetMin));

			int GetMin(int bv)
			{
				var av2 = set.Root.GetFirstNode(x => x >= bv);
				var av1 = av2.GetPreviousNode();
				return Math.Min(av2.Item - bv, bv - av1.Item);
			}
		}
	}
}
