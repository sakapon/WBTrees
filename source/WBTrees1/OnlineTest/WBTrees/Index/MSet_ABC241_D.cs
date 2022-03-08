using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.Index
{
	// Test: https://atcoder.jp/contests/abc241/tasks/abc241_d
	class MSet_ABC241_D
	{
		static long[] ReadL() => Array.ConvertAll(Console.ReadLine().Split(), long.Parse);
		static void Main()
		{
			var qc = int.Parse(Console.ReadLine());
			var qs = Array.ConvertAll(new bool[qc], _ => ReadL());

			var set = new WBMultiSet<long>();

			Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()) { AutoFlush = false });
			foreach (var q in qs)
			{
				if (q[0] == 1)
				{
					set.Add(q[1]);
				}
				else if (q[0] == 2)
				{
					var i = set.GetFirstIndex(v => v > q[1]);
					Console.WriteLine(set.GetAt(i - (int)q[2]).GetItemOrDefault(-1));
				}
				else
				{
					var i = set.GetLastIndex(v => v < q[1]);
					Console.WriteLine(set.GetAt(i + (int)q[2]).GetItemOrDefault(-1));
				}
			}
			Console.Out.Flush();
		}
	}
}
