using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBLL;

namespace OnlineTest.WBLL
{
	// Test: https://atcoder.jp/contests/abc120/tasks/abc120_c
	class LL_ABC120_C
	{
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var s = Console.ReadLine();

			var l = new WBLinkedList<char>();
			l.Initialize(s);

			for (var n = l.GetFirst(); n != null;)
			{
				var p = n.GetPrevious();
				var t = n;
				n = n.GetNext();

				if (p != null && p.Item != t.Item)
				{
					l.Remove(p);
					l.Remove(t);
				}
			}
			return s.Length - l.Count;
		}
	}
}
