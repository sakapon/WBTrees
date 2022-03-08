using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.YLC
{
	// Test: https://judge.yosupo.jp/problem/associative_array
	class Map_AssociativeArray
	{
		static long[] ReadL() => Array.ConvertAll(Console.ReadLine().Split(), long.Parse);
		static void Main()
		{
			var qc = int.Parse(Console.ReadLine());

			var map = new WBMap<long, long>();

			Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()) { AutoFlush = false });
			while (qc-- > 0)
			{
				var q = ReadL();

				if (q[0] == 0)
				{
					map[q[1]] = q[2];
				}
				else
				{
					Console.WriteLine(map.Get(q[1]).GetValueOrDefault());
				}
			}
			Console.Out.Flush();
		}
	}
}
