using System;
using System.Collections.Generic;
using System.Linq;

namespace WBTrees
{
	/// <summary>
	/// Represents a weight-balanced binary search tree, which can be accessed by index.
	/// </summary>
	/// <typeparam name="T">The type of the items.</typeparam>
	[System.Diagnostics.DebuggerDisplay(@"Count = {Count}")]
	public abstract class WBTreeBase<T> : IEnumerable<T>
	{
		#region Properties

		public Node<T> Root { get; private set; }
		public int Count => Root?.Count ?? 0;
		public IComparer<T> Comparer { get; }
		protected abstract bool IsDistinct { get; }

		// Call this method to update the Root object.
		protected void SetRoot(Node<T> node)
		{
			Root = node;
			if (node != null) node.Parent = null;
		}

		#endregion

		protected WBTreeBase(IComparer<T> comparer = null)
		{
			Comparer = comparer ?? Comparer<T>.Default;
		}

		#region Initialize Tree

		public void Clear() => SetRoot(null);

		public void Initialize(IEnumerable<T> items, bool assertsItems = true)
		{
			if (items == null) throw new ArgumentNullException(nameof(items));
			T[] a;
			if (assertsItems)
			{
				// stable sort
				a = items.OrderBy(x => x, Comparer).ToArray();
				if (IsDistinct)
					for (int i = 1; i < a.Length; ++i)
						if (Comparer.Compare(a[i - 1], a[i]) == 0) throw new ArgumentException("The keys must be unique.", nameof(items));
			}
			else
			{
				a = items as T[] ?? items.ToArray();
			}
			SetRoot(CreateSubtree(a, 0, a.Length));
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

		public IEnumerable<T> GetItems(Func<T, bool> startPredicate, Func<T, bool> endPredicate)
		{
			for (var n = Root?.GetFirst(startPredicate); n != null && (endPredicate?.Invoke(n.Item) ?? true); n = n.GetNext())
				yield return n.Item;
		}

		public IEnumerable<T> GetItemsDescending(Func<T, bool> startPredicate, Func<T, bool> endPredicate)
		{
			for (var n = Root?.GetLast(endPredicate); n != null && (startPredicate?.Invoke(n.Item) ?? true); n = n.GetPrevious())
				yield return n.Item;
		}

		public int RemoveItems(Func<T, bool> startPredicate, Func<T, bool> endPredicate)
		{
			var nodes = new List<Node<T>>();
			for (var n = Root?.GetFirst(startPredicate); n != null && (endPredicate?.Invoke(n.Item) ?? true); n = n.GetNext())
				nodes.Add(n);

			for (int i = nodes.Count - 1; i >= 0; --i) RemoveNode(nodes[i]);
			return nodes.Count;
		}

		#endregion

		#region Get/Remove Items (by Index)

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

		public Node<T> GetFirst(Func<T, bool> predicate) => Root?.GetFirst(predicate);
		public Node<T> GetLast(Func<T, bool> predicate) => Root?.GetLast(predicate);
		public Node<T> RemoveFirst(Func<T, bool> predicate) => RemoveNode(GetFirst(predicate));
		public Node<T> RemoveLast(Func<T, bool> predicate) => RemoveNode(GetLast(predicate));

		#endregion

		#region Get/Remove Node (by Index)

		public Node<T> GetAt(int index) => Root?.GetAt(index);
		public Node<T> RemoveAt(int index) => RemoveNode(GetAt(index));

		#endregion

		#region Get Index

		public int GetFirstIndex(Func<T, bool> predicate) => Root?.GetFirstIndex(predicate) ?? 0;
		public int GetLastIndex(Func<T, bool> predicate) => Root?.GetLastIndex(predicate) ?? -1;

		public int GetCount(Func<T, bool> startPredicate, Func<T, bool> endPredicate)
		{
			var c = GetLastIndex(endPredicate) - GetFirstIndex(startPredicate) + 1;
			return c >= 0 ? c : 0;
		}

		#endregion

		#region Add Item(s)

		public Node<T> Add(T item)
		{
			var c = Count;
			var node = AddOrGetNode(item);
			return Count != c ? node : null;
		}

		public int AddItems(IEnumerable<T> items)
		{
			if (items == null) throw new ArgumentNullException(nameof(items));
			var c = Count;
			foreach (var x in items) AddOrGetNode(x);
			return Count - c;
		}

		#endregion

		#region Protected Methods

		// No node will be added if IsDistinct and the specified item already exists.
		protected Node<T> AddOrGetNode(T item)
		{
			if (Root == null)
			{
				SetRoot(new Node<T> { Item = item });
				return Root;
			}

			Node<T> node = Root, newNode;
			while (true)
			{
				var d = Comparer.Compare(item, node.Item);
				if (IsDistinct && d == 0) return node;

				if (d < 0)
				{
					if (node.Left == null)
					{
						node.SetLeft(newNode = new Node<T> { Item = item });
						break;
					}
					node = node.Left;
				}
				else
				{
					if (node.Right == null)
					{
						node.SetRight(newNode = new Node<T> { Item = item });
						break;
					}
					node = node.Right;
				}
			}

			for (; node != null; node = node.Parent)
				node = Balance(node);
			return newNode;
		}

		// The specified instance will be removed from the tree.
		protected Node<T> RemoveNode(Node<T> node)
		{
			if (node == null) return null;

			Node<T> dirty;
			if (node.Left == null || node.Right == null)
			{
				UpdateChild(node, node.Left ?? node.Right);
				dirty = node.Parent;
			}
			else
			{
				var node2 = dirty = node.Right.GetFirst();
				if (node2 != node.Right)
				{
					dirty = node2.Parent;
					UpdateChild(node2, node2.Right);
					node2.SetRight(node.Right);
				}
				node2.SetLeft(node.Left);
				UpdateChild(node, node2);
			}
			dirty?.UpdateCount(true);
			return node;
		}

		#endregion

		#region Private Methods

		// Suppose node != null.
		void UpdateChild(Node<T> node, Node<T> newNode)
		{
			var parent = node.Parent;
			if (parent == null) SetRoot(newNode);
			else if (parent.Left == node) parent.SetLeft(newNode);
			else parent.SetRight(newNode);
		}

		// Suppose t != null.
		Node<T> Balance(Node<T> t)
		{
			var lc = t.LeftCount + 1;
			var rc = t.RightCount + 1;
			if (lc > 2 * rc)
			{
				t = RotateToRight(t);
			}
			else if (rc > 2 * lc)
			{
				t = RotateToLeft(t);
			}
			t.UpdateCount();
			return t;
		}

		// Suppose t != null.
		Node<T> RotateToRight(Node<T> t)
		{
			var p = t.Left;
			UpdateChild(t, p);
			t.SetLeft(p.Right);
			p.SetRight(t);
			t.UpdateCount();
			return p;
		}

		// Suppose t != null.
		Node<T> RotateToLeft(Node<T> t)
		{
			var p = t.Right;
			UpdateChild(t, p);
			t.SetRight(p.Left);
			p.SetLeft(t);
			t.UpdateCount();
			return p;
		}

		#endregion
	}
}
