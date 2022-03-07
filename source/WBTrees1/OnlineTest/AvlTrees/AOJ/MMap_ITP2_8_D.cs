using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.AvlTrees;

namespace OnlineTest.AvlTrees.AOJ
{
	// Test: https://onlinejudge.u-aizu.ac.jp/courses/lesson/8/ITP2/8/ITP2_8_D
	class MMap_ITP2_8_D
	{
		static void Main()
		{
			var qc = int.Parse(Console.ReadLine());

			var c = StringComparer.Ordinal;
			var map = new AvlMultiMap<string, string>(c);

			Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()) { AutoFlush = false });
			while (qc-- > 0)
			{
				var q = Console.ReadLine().Split();
				if (q[0] == "0")
					map.Add(q[1], q[2]);
				else if (q[0] == "1")
					foreach (var v in map[q[1]])
						Console.WriteLine(v);
				else if (q[0] == "2")
					map.RemoveAll(q[1]);
				else
					foreach (var p in map.GetItems(p => c.Compare(p.Key, q[1]) >= 0, p => c.Compare(p.Key, q[2]) <= 0))
						Console.WriteLine($"{p.Key} {p.Value}");
			}
			Console.Out.Flush();
		}
	}
}
