using System;
using System.Collections.Generic;
using System.Linq;

// A linked list by a self-balancing binary tree
namespace TreesLab.WBLL
{
	[System.Diagnostics.DebuggerDisplay(@"\{{Item}\}")]
	public class Node<T>
	{
		#region Properties

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

		#endregion

		#region Get Node

		public Node<T> GetPrevious() => Left?.GetLast() ?? GetPreviousAncestor();
		public Node<T> GetNext() => Right?.GetFirst() ?? GetNextAncestor();

		Node<T> GetPreviousAncestor() => Parent?.Right == this ? Parent : Parent?.GetPreviousAncestor();
		Node<T> GetNextAncestor() => Parent?.Left == this ? Parent : Parent?.GetNextAncestor();

		public Node<T> GetFirst() => Left?.GetFirst() ?? this;
		public Node<T> GetLast() => Right?.GetLast() ?? this;

		public Node<T> GetFirst(Func<T, bool> predicate)
		{
			if (predicate?.Invoke(Item) ?? true) return Left?.GetFirst(predicate) ?? this;
			else return Right?.GetFirst(predicate);
		}

		public Node<T> GetLast(Func<T, bool> predicate)
		{
			if (predicate?.Invoke(Item) ?? true) return Right?.GetLast(predicate) ?? this;
			else return Left?.GetLast(predicate);
		}

		#endregion

		public void Walk(Action<Node<T>> preorder, Action<Node<T>> inorder, Action<Node<T>> postorder)
		{
			preorder?.Invoke(this);
			Left?.Walk(preorder, inorder, postorder);
			inorder?.Invoke(this);
			Right?.Walk(preorder, inorder, postorder);
			postorder?.Invoke(this);
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

	[System.Diagnostics.DebuggerDisplay(@"Count = {Count}")]
	public class WBLinkedList<T> : IEnumerable<T>
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

		#region Get/Remove Node

		public Node<T> GetFirst() => Root?.GetFirst();
		public Node<T> GetLast() => Root?.GetLast();
		public Node<T> RemoveFirst() => RemoveNode(GetFirst());
		public Node<T> RemoveLast() => RemoveNode(GetLast());

		public Node<T> Remove(Node<T> node) => RemoveNode(node);

		#endregion

		#region Insert Item

		public Node<T> Prepend(T item) => InsertBefore(GetFirst(), item);
		public Node<T> Add(T item) => InsertAfter(GetLast(), item);

		public Node<T> AddBefore(Node<T> node, T item) => InsertBefore(node ?? throw new ArgumentNullException(nameof(node)), item);
		public Node<T> AddAfter(Node<T> node, T item) => InsertAfter(node ?? throw new ArgumentNullException(nameof(node)), item);

		#endregion

		#region Protected Methods

		protected Node<T> InsertBefore(Node<T> node, T item)
		{
			if (Root == null)
			{
				SetRoot(new Node<T> { Item = item });
				return Root;
			}

			Node<T> newNode;
			if (node.Left == null)
			{
				node.SetLeft(newNode = new Node<T> { Item = item });
			}
			else
			{
				node = node.Left;
				while (node.Right != null) node = node.Right;
				node.SetRight(newNode = new Node<T> { Item = item });
			}

			for (; node != null; node = node.Parent)
				node = Balance(node);
			return newNode;
		}

		protected Node<T> InsertAfter(Node<T> node, T item)
		{
			if (Root == null)
			{
				SetRoot(new Node<T> { Item = item });
				return Root;
			}

			Node<T> newNode;
			if (node.Right == null)
			{
				node.SetRight(newNode = new Node<T> { Item = item });
			}
			else
			{
				node = node.Right;
				while (node.Left != null) node = node.Left;
				node.SetLeft(newNode = new Node<T> { Item = item });
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
