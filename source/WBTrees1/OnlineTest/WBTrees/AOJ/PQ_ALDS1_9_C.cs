using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.AOJ
{
	// Test: https://onlinejudge.u-aizu.ac.jp/courses/lesson/1/ALDS1/9/ALDS1_9_C
	class PQ_ALDS1_9_C
	{
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var r = new List<int>();
			var set = new WBMultiSet<int>();

			string s;
			while ((s = Console.ReadLine()) != "end")
			{
				var q = s.Split();
				if (q[0] == "insert")
					set.Add(int.Parse(q[1]));
				else
					r.Add(set.RemoveLast().Item);
			}
			return string.Join("\n", r);
		}
	}
}
