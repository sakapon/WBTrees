using System;
using System.Collections.Generic;
using System.Linq;

// Node<T> をそのまま返す実装です。
namespace TreesLab.Core102
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

		// -, 0, + の領域に分かれているとき、0 となる最初のノード。
		public Node<T> GetFirstNode(Func<T, int> predicate)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			var d = predicate(Item);
			if (d == 0) return Left?.GetFirstNode(predicate) ?? this;
			else if (d > 0) return Left?.GetFirstNode(predicate);
			else return Right?.GetFirstNode(predicate);
		}

		// -, 0, + の領域に分かれているとき、0 となる最後のノード。
		public Node<T> GetLastNode(Func<T, int> predicate)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			var d = predicate(Item);
			if (d == 0) return Right?.GetLastNode(predicate) ?? this;
			else if (d < 0) return Right?.GetLastNode(predicate);
			else return Left?.GetLastNode(predicate);
		}

		public Node<T> GetPreviousNode() => Left?.GetLastNode() ?? GetPreviousAncestor();
		public Node<T> GetNextNode() => Right?.GetFirstNode() ?? GetNextAncestor();

		Node<T> GetPreviousAncestor()
		{
			if (Parent == null) return null;
			if (Parent.Right == this) return Parent;
			return Parent.GetPreviousAncestor();
		}

		Node<T> GetNextAncestor()
		{
			if (Parent == null) return null;
			if (Parent.Left == this) return Parent;
			return Parent.GetNextAncestor();
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

	/// <summary>
	/// Represents a binary search tree (multiset) sorted by <see cref="IComparer{T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of elements.</typeparam>
	/// <remarks>
	/// - First (Minimum)
	/// - Last (Maximum)
	/// - Previous (Predecessor)
	/// - Next (Successor)
	/// - Add (Insert)
	/// - Remove (Delete)
	/// </remarks>
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

		public IEnumerable<Node<T>> GetNodes()
		{
			for (var n = Root?.GetFirstNode(); n != null; n = n.GetNextNode())
			{
				yield return n;
			}
		}

		public IEnumerable<Node<T>> GetNodes(Func<T, bool> startPredicate, Func<T, bool> endPredicate)
		{
			if (startPredicate == null) throw new ArgumentNullException(nameof(startPredicate));
			if (endPredicate == null) throw new ArgumentNullException(nameof(endPredicate));

			for (var n = Root?.GetFirstNode(startPredicate); n != null && endPredicate(n.Item); n = n.GetNextNode())
			{
				yield return n;
			}
		}

		Node<T> GetFirstNode(T item) => Root?.GetFirstNode(x => Comparer.Compare(x, item));
		Node<T> GetLastNode(T item) => Root?.GetLastNode(x => Comparer.Compare(x, item));

		public bool Contains(T item) => GetFirstNode(item) != null;

		public Node<T> Add(T item)
		{
			var newNode = new Node<T> { Item = item };
			SetRoot(Add(Root, newNode));
			++Count;
			return newNode;
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

		// この実装では、引数で指定されたインスタンスが削除されるとは限りません。
		public void Remove(Node<T> node)
		{
			if (node == null) throw new ArgumentNullException(nameof(node));

			if (node.Left == null || node.Right == null)
			{
				var child = node.Left ?? node.Right;

				if (node.Parent == null)
					SetRoot(child);
				else if (node.Parent.Left == node)
					node.Parent.SetLeft(child);
				else
					node.Parent.SetRight(child);

				--Count;
			}
			else
			{
				var node2 = node.Right.GetFirstNode();
				node.Item = node2.Item;
				Remove(node2);
			}
		}
	}
}
