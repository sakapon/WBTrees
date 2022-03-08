using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.List
{
	// Test: https://atcoder.jp/contests/typical90/tasks/typical90_ar
	class List_Typical90_044
	{
		static int[] Read() => Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		static (int, int) Read2() { var a = Read(); return (a[0], a[1]); }
		static (int, int, int) Read3() { var a = Read(); return (a[0], a[1], a[2]); }
		static void Main()
		{
			var (n, qc) = Read2();
			var a = Read();

			var l = new WBList<int>();
			l.Initialize(a);

			Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()) { AutoFlush = false });
			while (qc-- > 0)
			{
				var (t, x, y) = Read3();
				x--;
				y--;

				if (t == 1)
				{
					(l[x], l[y]) = (l[y], l[x]);
				}
				else if (t == 2)
				{
					l.Prepend(l.RemoveLast().Item);
				}
				else
				{
					Console.WriteLine(l[x]);
				}
			}
			Console.Out.Flush();
		}
	}
}
