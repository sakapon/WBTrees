using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.Index
{
	// Test: https://atcoder.jp/contests/arc033/tasks/arc033_3
	class MSet_ARC033_C
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static (int, int) Read2() { var a = Read(); return (a[0], a[1]); }
		static void Main()
		{
			var qc = int.Parse(Console.ReadLine());

			var set = new WBMultiSet<int>();

			Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()) { AutoFlush = false });
			while (qc-- > 0)
			{
				var (t, x) = Read2();
				if (t == 1)
				{
					set.Add(x);
				}
				else
				{
					Console.WriteLine(set.RemoveAt(x - 1).Item);
				}
			}
			Console.Out.Flush();
		}
	}
}
