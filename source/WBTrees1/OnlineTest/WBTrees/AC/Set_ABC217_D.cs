using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.AC
{
	// Test: https://atcoder.jp/contests/abc217/tasks/abc217_d
	class Set_ABC217_D
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static (int, int) Read2() { var a = Read(); return (a[0], a[1]); }
		static void Main()
		{
			var (l, qc) = Read2();

			var set = new WBSet<int> { 0, l };

			Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()) { AutoFlush = false });
			while (qc-- > 0)
			{
				var (c, x) = Read2();
				if (c == 1)
				{
					set.Add(x);
				}
				else
				{
					var n2 = set.GetFirst(v => v > x);
					var n1 = n2.GetPrevious();
					Console.WriteLine(n2.Item - n1.Item);
				}
			}
			Console.Out.Flush();
		}
	}
}
