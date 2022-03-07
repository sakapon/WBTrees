using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.Sort
{
	// Test: https://atcoder.jp/contests/past202012-open/tasks/past202012_d
	class Sort_PAST202012_D
	{
		static void Main() => Console.WriteLine(Solve());
		static object Solve()
		{
			var n = int.Parse(Console.ReadLine());
			var ss = Array.ConvertAll(new bool[n], _ => Console.ReadLine());

			var comparer = Comparer<string>.Create((x, y) =>
			{
				var x2 = x.TrimStart('0');
				var y2 = y.TrimStart('0');

				var d = x2.Length - y2.Length;
				if (d != 0) return d;
				d = x2.CompareTo(y2);
				if (d != 0) return d;
				return y.Length - x.Length;
			});

			var set = new WBMultiSet<string>(comparer);
			//set.Initialize(ss);
			set.AddItems(ss);

			return string.Join("\n", set);
		}
	}
}
