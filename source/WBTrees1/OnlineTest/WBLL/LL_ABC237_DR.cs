using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBLL;

namespace OnlineTest.WBLL
{
	// Test: https://atcoder.jp/contests/abc237/tasks/abc237_d
	class LL_ABC237_DR
	{
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var n = int.Parse(Console.ReadLine());
			var s = Console.ReadLine();

			var l = new WBLinkedList<int> { n };

			for (int i = n - 1; i >= 0; i--)
				if (s[i] == 'L')
					l.Add(i);
				else
					l.Prepend(i);

			return string.Join(" ", l);
		}
	}
}
