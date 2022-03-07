using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.YLC
{
	// Test: https://judge.yosupo.jp/problem/predecessor_problem
	class Set_PredecessorProblem
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static void Main()
		{
			var qc = Read()[1];
			var t = Console.ReadLine();

			var set = new WBSet<int>();
			set.Initialize(Enumerable.Range(0, t.Length).Where(i => t[i] == '1'), false);

			Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()) { AutoFlush = false });
			while (qc-- > 0)
			{
				var q = Read();

				if (q[0] == 0)
				{
					set.Add(q[1]);
				}
				else if (q[0] == 1)
				{
					set.Remove(q[1]);
				}
				else if (q[0] == 2)
				{
					Console.WriteLine(set.Contains(q[1]) ? 1 : 0);
				}
				else if (q[0] == 3)
				{
					Console.WriteLine(set.GetFirst(x => x >= q[1]).GetItemOrDefault(-1));
				}
				else
				{
					Console.WriteLine(set.GetLast(x => x <= q[1]).GetItemOrDefault(-1));
				}
			}
			Console.Out.Flush();
		}
	}
}
