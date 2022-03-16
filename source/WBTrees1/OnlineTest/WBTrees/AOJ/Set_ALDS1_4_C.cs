using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.AOJ
{
	// Test: https://onlinejudge.u-aizu.ac.jp/courses/lesson/1/ALDS1/4/ALDS1_4_C
	class Set_ALDS1_4_C
	{
		static void Main()
		{
			var qc = int.Parse(Console.ReadLine());

			var c = StringComparer.Ordinal;
			var set = new WBSet<string>(c);

			Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()) { AutoFlush = false });
			while (qc-- > 0)
			{
				var q = Console.ReadLine().Split();
				if (q[0] == "insert")
					set.Add(q[1]);
				else
					Console.WriteLine(set.Contains(q[1]) ? "yes" : "no");
			}
			Console.Out.Flush();
		}
	}
}
