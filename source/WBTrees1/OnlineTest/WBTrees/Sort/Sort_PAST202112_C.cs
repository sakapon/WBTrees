using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.Sort
{
	// Test: https://atcoder.jp/contests/past202112-open/tasks/past202112_c
	class Sort_PAST202112_C
	{
		static void Main()
		{
			var n = int.Parse(Console.ReadLine());

			var map = new WBMap<char, int>();

			for (int id = 1; id <= n; id++)
			{
				var pv = Console.ReadLine();
				if (pv[2..] == "AC") map.Add(pv[0], id);
			}

			foreach (var (_, id) in map)
			{
				Console.WriteLine(id);
			}
		}
	}
}
