using System;
using System.Collections.Generic;
using System.Linq;

namespace WBTrees
{
	/// <summary>
	/// Represents a list by a weight-balanced binary tree, which can be accessed by index.
	/// </summary>
	/// <typeparam name="T">The type of the items.</typeparam>
	[System.Diagnostics.DebuggerDisplay(@"Count = {Count}")]
	public class WBList<T> : IEnumerable<T>
	{
		#region Properties

		public Node<T> Root { get; private set; }
		public int Count => Root?.Count ?? 0;

		// Call this method to update the Root object.
		protected void SetRoot(Node<T> node)
		{
			Root = node;
			if (node != null) node.Parent = null;
		}

		#endregion

		#region Initialize Tree

		public void Clear() => SetRoot(null);

		public void Initialize(IEnumerable<T> items)
		{
			if (items == null) throw new ArgumentNullException(nameof(items));
			var a = items as T[] ?? items.ToArray();
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

		#endregion

		#region Get/Remove Node (by Index)

		public Node<T> GetAt(int index) => Root?.GetAt(index);
		public Node<T> RemoveAt(int index) => RemoveNode(GetAt(index));

		public T this[int index]
		{
			get
			{
				var node = GetAt(index) ?? throw new ArgumentOutOfRangeException(nameof(index));
				return node.Item;
			}
			set
			{
				var node = GetAt(index) ?? throw new ArgumentOutOfRangeException(nameof(index));
				node.Item = value;
			}
		}

		#endregion

		#region Insert Item(s)

		public Node<T> Insert(int index, T item) => InsertNode(index, item);
		public Node<T> Prepend(T item) => InsertNode(0, item);
		public Node<T> Add(T item) => InsertNode(Count, item);

		public int InsertItems(int index, IEnumerable<T> items)
		{
			if (items == null) throw new ArgumentNullException(nameof(items));
			var c = Count;
			foreach (var x in items) InsertNode(index++, x);
			return Count - c;
		}
		public int PrependItems(IEnumerable<T> items) => InsertItems(0, items);
		public int AddItems(IEnumerable<T> items) => InsertItems(Count, items);

		#endregion

		#region Protected Methods

		protected Node<T> InsertNode(int index, T item)
		{
			if (index < 0) throw new ArgumentOutOfRangeException(nameof(index));
			if (index > Count) throw new ArgumentOutOfRangeException(nameof(index));

			if (Root == null)
			{
				SetRoot(new Node<T> { Item = item });
				return Root;
			}

			Node<T> node = Root, newNode;
			while (true)
			{
				var d = index - node.LeftCount - 1;
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
					index = d;
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
