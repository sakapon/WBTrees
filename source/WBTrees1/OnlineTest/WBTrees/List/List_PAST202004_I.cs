using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.List
{
	// Test: https://atcoder.jp/contests/past202004-open/tasks/past202004_i
	class List_PAST202004_I
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var n = int.Parse(Console.ReadLine());
			var a = Read();

			var r = Array.ConvertAll(a, _ => n);

			var l = new WBList<int>();
			l.Initialize(Enumerable.Range(0, a.Length));

			for (int k = 1; k < n; k++)
			{
				for (int i = 0; i < l.Count; i++)
				{
					var i0 = l[i];
					var i1 = l[i + 1];
					r[a[i0] < a[i1] ? i0 : i1] = k;
					l.RemoveAt(a[i0] < a[i1] ? i : i + 1);
				}
			}
			return string.Join("\n", r);
		}
	}
}
