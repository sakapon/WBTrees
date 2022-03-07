﻿using System;
using System.Collections.Generic;
using System.Linq;
using TreesLab.Core;

namespace OnlineTest.Core
{
	// Test: https://onlinejudge.u-aizu.ac.jp/courses/lesson/1/ALDS1/8/ALDS1_8_C
	class BST_ALDS1_8_C
	{
		static void Main()
		{
			var n = int.Parse(Console.ReadLine());

			var set = new BinarySearchTree<int>();

			Console.SetOut(new System.IO.StreamWriter(Console.OpenStandardOutput()) { AutoFlush = false });
			while (n-- > 0)
			{
				var q = Console.ReadLine().Split();
				if (q[0] == "insert")
				{
					var v = int.Parse(q[1]);
					set.Add(v);
				}
				else if (q[0] == "find")
				{
					var v = int.Parse(q[1]);
					Console.WriteLine(set.Contains(v) ? "yes" : "no");
				}
				else if (q[0] == "delete")
				{
					var v = int.Parse(q[1]);
					set.Remove(v);
				}
				else
				{
					Console.WriteLine(" " + string.Join(" ", set));
					set.Root?.Walk(v => Console.Write($" {v.Item}"), null, null);
					Console.WriteLine();
				}
			}
			Console.Out.Flush();
		}
	}
}
