using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBLL;

namespace OnlineTest.WBLL
{
	// Test: https://atcoder.jp/contests/abc237/tasks/abc237_d
	class LL_ABC237_D
	{
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var n = int.Parse(Console.ReadLine());
			var s = Console.ReadLine();

			var l = new WBLinkedList<int>();
			var node = l.Add(0);

			for (int i = 1; i <= n; i++)
				if (s[i - 1] == 'L')
					node = l.AddBefore(node, i);
				else
					node = l.AddAfter(node, i);

			return string.Join(" ", l);
		}
	}
}
