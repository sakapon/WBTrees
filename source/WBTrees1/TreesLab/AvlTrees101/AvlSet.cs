using System;
using System.Collections.Generic;
using System.Linq;

namespace TreesLab.AvlTrees101
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

		public int Height { get; private set; } = 1;
		public int LeftHeight => Left?.Height ?? 0;
		public int RightHeight => Right?.Height ?? 0;

		internal void UpdateHeight(bool recursive = false)
		{
			Height = Math.Max(LeftHeight, RightHeight) + 1;
			if (recursive) Parent?.UpdateHeight(true);
		}

		#region Get Node

		public Node<T> GetFirstNode() => Left?.GetFirstNode() ?? this;
		public Node<T> GetLastNode() => Right?.GetLastNode() ?? this;

		public Node<T> GetFirstNode(Func<T, bool> predicate)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			if (predicate(Item)) return Left?.GetFirstNode(predicate) ?? this;
			else return Right?.GetFirstNode(predicate);
		}

		public Node<T> GetLastNode(Func<T, bool> predicate)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			if (predicate(Item)) return Right?.GetLastNode(predicate) ?? this;
			else return Left?.GetLastNode(predicate);
		}

		public Node<T> GetFirstNode(T item, IComparer<T> comparer)
		{
			if (comparer == null) throw new ArgumentNullException(nameof(comparer));
			var d = comparer.Compare(item, Item);
			if (d == 0) return Left?.GetFirstNode(item, comparer) ?? this;
			else if (d < 0) return Left?.GetFirstNode(item, comparer);
			else return Right?.GetFirstNode(item, comparer);
		}

		public Node<T> GetLastNode(T item, IComparer<T> comparer)
		{
			if (comparer == null) throw new ArgumentNullException(nameof(comparer));
			var d = comparer.Compare(item, Item);
			if (d == 0) return Right?.GetLastNode(item, comparer) ?? this;
			else if (d > 0) return Right?.GetLastNode(item, comparer);
			else return Left?.GetLastNode(item, comparer);
		}

		public Node<T> GetPreviousNode() => Left?.GetLastNode() ?? GetPreviousAncestor();
		public Node<T> GetNextNode() => Right?.GetFirstNode() ?? GetNextAncestor();

		Node<T> GetPreviousAncestor() => Parent?.Right == this ? Parent : Parent?.GetPreviousAncestor();
		Node<T> GetNextAncestor() => Parent?.Left == this ? Parent : Parent?.GetNextAncestor();

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

	[System.Diagnostics.DebuggerDisplay(@"Count = {Count}")]
	public abstract class AvlSetBase<T> : IEnumerable<T>
	{
		public Node<T> Root { get; private set; }
		public int Count { get; protected set; }
		public IComparer<T> Comparer { get; }

		protected AvlSetBase(IComparer<T> comparer = null)
		{
			Comparer = comparer ?? Comparer<T>.Default;
		}

		// Call this method to update the Root object.
		protected void SetRoot(Node<T> node)
		{
			Root = node;
			if (node != null) node.Parent = null;
		}

		#region Initialize Tree

		public void Clear()
		{
			SetRoot(null);
			Count = 0;
		}

		protected void Initialize(IEnumerable<T> collection, bool distinct)
		{
			if (collection == null) throw new ArgumentNullException(nameof(collection));
			var items = (distinct ? collection.Distinct() : collection).ToArray();
			Array.Sort(items, Comparer);
			SetRoot(CreateSubtree(items, 0, items.Length));
			Count = items.Length;
		}

		static Node<T> CreateSubtree(T[] items, int l, int r)
		{
			if (r == l) return null;
			if (r == l + 1) return new Node<T> { Item = items[l] };

			var m = (l + r) / 2;
			var node = new Node<T> { Item = items[m] };
			node.SetLeft(CreateSubtree(items, l, m));
			node.SetRight(CreateSubtree(items, m + 1, r));
			node.UpdateHeight();
			return node;
		}

		#endregion

		#region Get Items

		public IEnumerator<T> GetEnumerator() => GetItems().GetEnumerator();
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetItems().GetEnumerator();
		public IEnumerable<T> GetItems()
		{
			for (var n = Root?.GetFirstNode(); n != null; n = n.GetNextNode())
			{
				yield return n.Item;
			}
		}

		public IEnumerable<T> GetItemsDescending()
		{
			for (var n = Root?.GetLastNode(); n != null; n = n.GetPreviousNode())
			{
				yield return n.Item;
			}
		}

		public IEnumerable<T> GetItems(Func<T, bool> startPredicate, Func<T, bool> endPredicate)
		{
			if (startPredicate == null) throw new ArgumentNullException(nameof(startPredicate));
			if (endPredicate == null) throw new ArgumentNullException(nameof(endPredicate));

			for (var n = Root?.GetFirstNode(startPredicate); n != null && endPredicate(n.Item); n = n.GetNextNode())
			{
				yield return n.Item;
			}
		}

		public IEnumerable<T> GetItemsDescending(Func<T, bool> startPredicate, Func<T, bool> endPredicate)
		{
			if (startPredicate == null) throw new ArgumentNullException(nameof(startPredicate));
			if (endPredicate == null) throw new ArgumentNullException(nameof(endPredicate));

			for (var n = Root?.GetLastNode(endPredicate); n != null && startPredicate(n.Item); n = n.GetPreviousNode())
			{
				yield return n.Item;
			}
		}

		#endregion

		Node<T> AssertNotEmpty() => Root ?? throw new InvalidOperationException("The container is empty.");

		public T GetFirst()
		{
			AssertNotEmpty();
			return Root.GetFirstNode().Item;
		}

		public T GetLast()
		{
			AssertNotEmpty();
			return Root.GetLastNode().Item;
		}

		public T GetFirst(Func<T, bool> predicate, T defaultValue = default(T))
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			var node = Root?.GetFirstNode(predicate);
			return node == null ? defaultValue : node.Item;
		}

		public T GetLast(Func<T, bool> predicate, T defaultValue = default(T))
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			var node = Root?.GetLastNode(predicate);
			return node == null ? defaultValue : node.Item;
		}

		public bool Contains(T item) => Root?.GetFirstNode(item, Comparer) != null;

		public bool Remove(T item)
		{
			var node = Root?.GetLastNode(item, Comparer);
			if (node == null) return false;

			Remove(node);
			return true;
		}

		public int RemoveItems(Func<T, bool> startPredicate, Func<T, bool> endPredicate)
		{
			if (startPredicate == null) throw new ArgumentNullException(nameof(startPredicate));
			if (endPredicate == null) throw new ArgumentNullException(nameof(endPredicate));

			var nodes = new List<Node<T>>();
			for (var n = Root?.GetLastNode(endPredicate); n != null && startPredicate(n.Item); n = n.GetPreviousNode())
				nodes.Add(n);

			foreach (var n in nodes) Remove(n);
			return nodes.Count;
		}

		#region Private Methods

		// Suppose t != null.
		protected void Remove(Node<T> t)
		{
			if (t.Left == null || t.Right == null)
			{
				var c = t.Left ?? t.Right;

				if (t.Parent == null)
					SetRoot(c);
				else if (t.Parent.Left == t)
					t.Parent.SetLeft(c);
				else
					t.Parent.SetRight(c);

				t.Parent?.UpdateHeight(true);
				--Count;
			}
			else
			{
				var t2 = t.Right.GetFirstNode();
				t.Item = t2.Item;
				Remove(t2);
			}
		}

		protected Node<T> Add(Node<T> node, T item, bool distinct)
		{
			if (node == null)
			{
				++Count;
				return new Node<T> { Item = item };
			}

			var d = Comparer.Compare(item, node.Item);
			if (distinct && d == 0) return node;

			if (d < 0) node.SetLeft(Add(node.Left, item, distinct));
			else node.SetRight(Add(node.Right, item, distinct));

			node = Balance(node);
			node.UpdateHeight();
			return node;
		}

		// Suppose t != null.
		static Node<T> Balance(Node<T> t)
		{
			var lrh = t.LeftHeight - t.RightHeight;
			if (lrh > 2 || lrh == 2 && t.Left.LeftHeight >= t.Left.RightHeight)
			{
				t = RotateToRight(t);
				t.Right.UpdateHeight();
			}
			else if (lrh < -2 || lrh == -2 && t.Right.LeftHeight <= t.Right.RightHeight)
			{
				t = RotateToLeft(t);
				t.Left.UpdateHeight();
			}
			return t;
		}

		// Suppose t != null.
		static Node<T> RotateToRight(Node<T> t)
		{
			var p = t.Left;
			t.SetLeft(p.Right);
			p.SetRight(t);
			return p;
		}

		// Suppose t != null.
		static Node<T> RotateToLeft(Node<T> t)
		{
			var p = t.Right;
			t.SetRight(p.Left);
			p.SetLeft(t);
			return p;
		}

		#endregion
	}

	public class AvlSet<T> : AvlSetBase<T>
	{
		public AvlSet(IComparer<T> comparer = null) : base(comparer) { }

		public void Initialize(IEnumerable<T> items) => Initialize(items, true);

		public bool Add(T item)
		{
			var c = Count;
			SetRoot(Add(Root, item, true));
			return Count != c;
		}

		public int AddItems(IEnumerable<T> items)
		{
			if (items == null) throw new ArgumentNullException(nameof(items));
			var c = Count;
			foreach (var x in items) SetRoot(Add(Root, x, true));
			return Count - c;
		}
	}

	public class AvlMultiSet<T> : AvlSetBase<T>
	{
		public AvlMultiSet(IComparer<T> comparer = null) : base(comparer) { }

		public void Initialize(IEnumerable<T> items) => Initialize(items, false);

		public void Add(T item)
		{
			SetRoot(Add(Root, item, false));
		}

		public void AddItems(IEnumerable<T> items)
		{
			if (items == null) throw new ArgumentNullException(nameof(items));
			foreach (var x in items) SetRoot(Add(Root, x, false));
		}

		public int RemoveAll(T item)
		{
			var nodes = new List<Node<T>>();
			for (var n = Root?.GetLastNode(x => Comparer.Compare(x, item) <= 0); n != null && Comparer.Compare(n.Item, item) == 0; n = n.GetPreviousNode())
				nodes.Add(n);

			foreach (var n in nodes) Remove(n);
			return nodes.Count;
		}
	}
}
