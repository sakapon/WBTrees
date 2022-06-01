using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.AC
{
	// Test: https://atcoder.jp/contests/abc253/tasks/abc253_c
	class MSet_ABC253_C
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static void Main()
		{
			var qc = int.Parse(Console.ReadLine());
			var qs = Array.ConvertAll(new bool[qc], _ => Read());

			var set = new WBMultiSet<int>();

			Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()) { AutoFlush = false });
			foreach (var q in qs)
			{
				if (q[0] == 1)
				{
					set.Add(q[1]);
				}
				else if (q[0] == 2)
				{
					var x = q[1];
					var c = q[2];
					while (c-- > 0 && set.Contains(x)) set.Remove(x);
				}
				else
				{
					Console.WriteLine(set.GetLast().Item - set.GetFirst().Item);
				}
			}
			Console.Out.Flush();
		}
	}
}
