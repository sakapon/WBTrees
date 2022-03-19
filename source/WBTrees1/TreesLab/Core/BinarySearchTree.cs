using System;
using System.Collections.Generic;
using System.Linq;

namespace TreesLab.Core
{
	/// <summary>
	/// Represents a node for binary search trees.
	/// </summary>
	/// <typeparam name="T">The type of the item.</typeparam>
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

	/// <summary>
	/// Represents a binary search tree (multiset) sorted by an <see cref="IComparer{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of elements.</typeparam>
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

		// Root を変更するには、このメソッドを経由します。
		void SetRoot(Node<T> node)
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

		public void Initialize(IEnumerable<T> collection)
		{
			if (collection == null) throw new ArgumentNullException(nameof(collection));
			var items = collection.ToArray();
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

		public void Add(T item)
		{
			var newNode = new Node<T> { Item = item };
			SetRoot(Add(Root, newNode));
			++Count;
		}

		public void AddItems(IEnumerable<T> items)
		{
			if (items == null) throw new ArgumentNullException(nameof(items));
			foreach (var x in items) Add(x);
		}

		public bool Remove(T item)
		{
			var node = Root?.GetLastNode(item, Comparer);
			if (node == null) return false;

			Remove(node);
			return true;
		}

		public int RemoveAll(T item)
		{
			var nodes = new List<Node<T>>();
			for (var n = Root?.GetLastNode(x => Comparer.Compare(x, item) <= 0); n != null && Comparer.Compare(n.Item, item) == 0; n = n.GetPreviousNode())
				nodes.Add(n);

			foreach (var n in nodes) Remove(n);
			return nodes.Count;
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

		Node<T> Add(Node<T> node, Node<T> newNode)
		{
			if (node == null) return newNode;

			if (Comparer.Compare(newNode.Item, node.Item) < 0)
				node.SetLeft(Add(node.Left, newNode));
			else
				node.SetRight(Add(node.Right, newNode));
			return node;
		}

		// この実装では、引数で指定されたインスタンスが削除されます。
		// Suppose node != null.
		void Remove(Node<T> node)
		{
			if (node.Left == null || node.Right == null)
			{
				UpdateChild(node, node.Left ?? node.Right);
			}
			else
			{
				var node2 = node.Right.GetFirstNode();
				if (node2 != node.Right)
				{
					UpdateChild(node2, node2.Right);
					node2.SetRight(node.Right);
				}
				UpdateChild(node, node2);
				node2.SetLeft(node.Left);
			}
			--Count;
		}

		// Suppose child != null.
		void UpdateChild(Node<T> child, Node<T> newChild)
		{
			var parent = child.Parent;
			if (parent == null)
				SetRoot(newChild);
			else if (parent.Left == child)
				parent.SetLeft(newChild);
			else
				parent.SetRight(newChild);
		}
	}
}
