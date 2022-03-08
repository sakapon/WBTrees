using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.WBTrees;

namespace OnlineTest.WBTrees.List
{
	// Test: https://atcoder.jp/contests/past202104-open/tasks/past202104_e
	class List_PAST202104_E
	{
		static void Main()
		{
			var n = int.Parse(Console.ReadLine());
			var s = Console.ReadLine();

			var l = new WBList<int>();

			Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()) { AutoFlush = false });
			for (int i = 0; i < n; i++)
			{
				var c = s[i];

				if (c == 'L')
				{
					l.Prepend(i + 1);
				}
				else if (c == 'R')
				{
					l.Add(i + 1);
				}
				else if (c < 'D')
				{
					var k = c - 'A';
					if (k < l.Count)
						Console.WriteLine(l.RemoveAt(k).Item);
					else
						Console.WriteLine("ERROR");
				}
				else
				{
					var k = c - 'D';
					if (k < l.Count)
						Console.WriteLine(l.RemoveAt(l.Count - 1 - k).Item);
					else
						Console.WriteLine("ERROR");
				}
			}
			Console.Out.Flush();
		}
	}
}
