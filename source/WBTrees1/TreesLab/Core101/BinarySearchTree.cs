using System;
using System.Collections.Generic;

// 論理的な実現性を検証するための実装であり、.NET の設計ガイドラインには沿っていません。
// Node<T> に対して機能を実装する方法として、
// - 静的 (または拡張) メソッド
// - インスタンス メソッド
// のいずれも同等に実現できます。
// ここでは、静的メソッドで実装しています。
// ただし、インスタンス メソッドのほうが簡潔になります。
namespace TreesLab.Core101
{
	[System.Diagnostics.DebuggerDisplay(@"\{{Item}\}")]
	public class Node<T>
	{
		public T Item;
		public Node<T> Parent, Left, Right;
	}

	[System.Diagnostics.DebuggerDisplay(@"Count = {Count}")]
	public class BinarySearchTree<T> : IEnumerable<T>
	{
		public Node<T> Root { get; private set; }
		public int Count { get; private set; }
		public IComparer<T> Comparer { get; }

		public BinarySearchTree(IComparer<T> comparer = null)
		{
			Comparer = comparer ?? Comparer<T>.Default;
		}

		public void Clear()
		{
			Root = null;
			Count = 0;
		}

		#region Get Node

		static Node<T> GetFirstNode(Node<T> node)
		{
			if (node == null) return null;
			return GetFirstNode(node.Left) ?? node;
		}

		static Node<T> GetLastNode(Node<T> node)
		{
			if (node == null) return null;
			return GetLastNode(node.Right) ?? node;
		}

		static Node<T> GetFirstNode(Node<T> node, Func<T, bool> predicate)
		{
			if (node == null) return null;
			if (predicate(node.Item)) return GetFirstNode(node.Left, predicate) ?? node;
			else return GetFirstNode(node.Right, predicate);
		}

		static Node<T> GetLastNode(Node<T> node, Func<T, bool> predicate)
		{
			if (node == null) return null;
			if (predicate(node.Item)) return GetLastNode(node.Right, predicate) ?? node;
			else return GetLastNode(node.Left, predicate);
		}

		#endregion

		public Node<T> GetFirstNode()
		{
			return GetFirstNode(Root);
		}

		public Node<T> GetLastNode()
		{
			return GetLastNode(Root);
		}

		public Node<T> GetFirstNode(Func<T, bool> predicate)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			return GetFirstNode(Root, predicate);
		}

		public Node<T> GetLastNode(Func<T, bool> predicate)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			return GetLastNode(Root, predicate);
		}

		public IEnumerator<T> GetEnumerator() => GetItems().GetEnumerator();
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetItems().GetEnumerator();
		public IEnumerable<T> GetItems()
		{
			throw new NotImplementedException();
		}
	}
}
