using System;
using System.Collections.Generic;
using System.Linq;

// WBList と WBMultiSet の共通部分です。
namespace TreesLab.Indexed
{
	[System.Diagnostics.DebuggerDisplay(@"\{{Item}\}")]
	public class Node<T>
	{
		public T Item { get; internal set; }
		public Node<T> Parent { get; internal set; }
		public Node<T> Left { get; private set; }
		public Node<T> Right { get; private set; }

		internal void SetLeft(Node<T> node)
		{
			Left = node;
			if (node != null) node.Parent = this;
		}

		internal void SetRight(Node<T> node)
		{
			Right = node;
			if (node != null) node.Parent = this;
		}

		public int Count { get; private set; } = 1;
		public int LeftCount => Left?.Count ?? 0;
		public int RightCount => Right?.Count ?? 0;

		internal void UpdateCount(bool recursive = false)
		{
			Count = LeftCount + RightCount + 1;
			if (recursive) Parent?.UpdateCount(true);
		}

		public Node<T> GetPrevious() => Left?.GetLast() ?? GetPreviousAncestor();
		public Node<T> GetNext() => Right?.GetFirst() ?? GetNextAncestor();

		Node<T> GetPreviousAncestor() => Parent?.Right == this ? Parent : Parent?.GetPreviousAncestor();
		Node<T> GetNextAncestor() => Parent?.Left == this ? Parent : Parent?.GetNextAncestor();

		public Node<T> GetFirst() => Left?.GetFirst() ?? this;
		public Node<T> GetLast() => Right?.GetLast() ?? this;

		// out of range: null
		public Node<T> GetAt(int index)
		{
			var d = index - LeftCount;
			if (d == 0) return this;
			else if (d < 0) return Left?.GetAt(index);
			else return Right?.GetAt(d - 1);
		}

		public int GetIndex()
		{
			if (Parent == null) return LeftCount;
			else if (Parent.Left == this) return Parent.GetIndex() - RightCount - 1;
			else return Parent.GetIndex() + LeftCount + 1;
		}
	}

	public static class NodeHelper
	{
		public static bool Exists<T>(this Node<T> node) => node != null;

		public static T GetItemOrDefault<T>(this Node<T> node, T defaultItem = default(T)) => node != null ? node.Item : defaultItem;
		public static bool TryGetItem<T>(this Node<T> node, out T item)
		{
			item = node != null ? node.Item : default(T);
			return node != null;
		}
	}

	/// <summary>
	/// Represents an indexed tree.
	/// </summary>
	/// <typeparam name="T">The type of the items.</typeparam>
	/// <remarks>
	/// - Get by Index
	/// - Remove by Index
	/// </remarks>
	[System.Diagnostics.DebuggerDisplay(@"Count = {Count}")]
	public class IndexedTree<T> : IEnumerable<T>
	{
		#region Properties

		public Node<T> Root { get; private set; }
		public int Count { get; private set; }

		// Call this method to update the Root object.
		void SetRoot(Node<T> node)
		{
			Root = node;
			if (node != null) node.Parent = null;
		}

		#endregion

		#region Initialize Tree

		public void Clear()
		{
			SetRoot(null);
			Count = 0;
		}

		public void Initialize(IEnumerable<T> items)
		{
			if (items == null) throw new ArgumentNullException(nameof(items));
			var a = items as T[] ?? items.ToArray();
			SetRoot(CreateSubtree(a, 0, a.Length));
			Count = a.Length;
		}

		static Node<T> CreateSubtree(T[] items, int l, int r)
		{
			if (r == l) return null;
			if (r == l + 1) return new Node<T> { Item = items[l] };

			var m = (l + r) / 2;
			var node = new Node<T> { Item = items[m] };
			node.SetLeft(CreateSubtree(items, l, m));
			node.SetRight(CreateSubtree(items, m + 1, r));
			node.UpdateCount();
			return node;
		}

		#endregion

		#region Get/Remove Items

		public IEnumerator<T> GetEnumerator() => GetItems().GetEnumerator();
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetItems().GetEnumerator();
		public IEnumerable<T> GetItems()
		{
			for (var n = Root?.GetFirst(); n != null; n = n.GetNext())
				yield return n.Item;
		}

		public IEnumerable<T> GetItemsDescending()
		{
			for (var n = Root?.GetLast(); n != null; n = n.GetPrevious())
				yield return n.Item;
		}

		public IEnumerable<T> GetItems(int startIndex, int endIndex)
		{
			if (startIndex < 0) startIndex = 0;
			for (var n = Root?.GetAt(startIndex); n != null && startIndex < endIndex; n = n.GetNext(), ++startIndex)
				yield return n.Item;
		}

		public IEnumerable<T> GetItemsDescending(int startIndex, int endIndex)
		{
			if (endIndex > Count) endIndex = Count;
			for (var n = Root?.GetAt(--endIndex); n != null && startIndex <= endIndex; n = n.GetPrevious(), --endIndex)
				yield return n.Item;
		}

		public int RemoveItems(int startIndex, int endIndex)
		{
			if (startIndex < 0) startIndex = 0;
			var nodes = new List<Node<T>>();
			for (var n = Root?.GetAt(startIndex); n != null && startIndex < endIndex; n = n.GetNext(), ++startIndex)
				nodes.Add(n);

			for (int i = nodes.Count - 1; i >= 0; --i) RemoveNode(nodes[i]);
			return nodes.Count;
		}

		#endregion

		#region Get/Remove Node

		public Node<T> GetFirst() => Root?.GetFirst();
		public Node<T> GetLast() => Root?.GetLast();
		public Node<T> RemoveFirst() => RemoveNode(GetFirst());
		public Node<T> RemoveLast() => RemoveNode(GetLast());

		public Node<T> GetAt(int index) => Root?.GetAt(index);
		public Node<T> RemoveAt(int index) => RemoveNode(GetAt(index));

		#endregion

		// この実装では、引数で指定されたインスタンスが削除されるとは限りません。
		Node<T> RemoveNode(Node<T> node)
		{
			if (node == null) return null;

			if (node.Left == null || node.Right == null)
			{
				var child = node.Left ?? node.Right;

				if (node.Parent == null)
					SetRoot(child);
				else if (node.Parent.Left == node)
					node.Parent.SetLeft(child);
				else
					node.Parent.SetRight(child);

				node.Parent?.UpdateCount(true);
				--Count;
				return node;
			}
			else
			{
				var node2 = node.Right.GetFirst();
				(node.Item, node2.Item) = (node2.Item, node.Item);
				return RemoveNode(node2);
			}
		}
	}
}
