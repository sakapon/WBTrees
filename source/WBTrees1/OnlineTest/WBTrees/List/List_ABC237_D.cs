using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.List
{
	// Test: https://atcoder.jp/contests/abc237/tasks/abc237_d
	class List_ABC237_D
	{
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var n = int.Parse(Console.ReadLine());
			var s = Console.ReadLine();

			var l = new WBList<int> { 0 };
			var li = 0;

			for (int i = 0; i < n; i++)
			{
				if (s[i] == 'R') li++;
				l.Insert(li, i + 1);
			}
			return string.Join(" ", l);
		}
	}
}
