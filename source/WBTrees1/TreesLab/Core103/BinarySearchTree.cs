using System;
using System.Collections.Generic;
using System.Linq;

// 再帰を使わない実装です。
namespace TreesLab.Core103
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

		public Node<T> GetFirstNode()
		{
			var node = this;
			while (node.Left != null) node = node.Left;
			return node;
		}

		public Node<T> GetLastNode()
		{
			var node = this;
			while (node.Right != null) node = node.Right;
			return node;
		}

		public Node<T> GetFirstNode(Func<T, bool> predicate)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			var node = this;
			Node<T> result = null;
			while (node != null)
			{
				if (predicate(node.Item))
				{
					result = node;
					node = node.Left;
				}
				else
				{
					node = node.Right;
				}
			}
			return result;
		}

		public Node<T> GetLastNode(Func<T, bool> predicate)
		{
			if (predicate == null) throw new ArgumentNullException(nameof(predicate));
			var node = this;
			Node<T> result = null;
			while (node != null)
			{
				if (predicate(node.Item))
				{
					result = node;
					node = node.Right;
				}
				else
				{
					node = node.Left;
				}
			}
			return result;
		}

		public Node<T> GetPreviousNode() => Left?.GetLastNode() ?? GetPreviousAncestor();
		public Node<T> GetNextNode() => Right?.GetFirstNode() ?? GetNextAncestor();

		Node<T> GetPreviousAncestor()
		{
			var node = this;
			while (node.Parent != null && node.Parent.Left == node) node = node.Parent;
			return node.Parent;
		}

		Node<T> GetNextAncestor()
		{
			var node = this;
			while (node.Parent != null && node.Parent.Right == node) node = node.Parent;
			return node.Parent;
		}

		#endregion

		public void Walk(Action<Node<T>> preorder, Action<Node<T>> inorder, Action<Node<T>> postorder)
		{
			Dfs(this);
			void Dfs(Node<T> node)
			{
				if (node == null) return;
				preorder?.Invoke(node);
				Dfs(node.Left);
				inorder?.Invoke(node);
				Dfs(node.Right);
				postorder?.Invoke(node);
			}
		}
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

		public Node<T> Add(T item)
		{
			++Count;
			var newNode = new Node<T> { Item = item };

			if (Root == null)
			{
				SetRoot(newNode);
				return newNode;
			}

			for (var node = Root; ;)
			{
				if (Comparer.Compare(item, node.Item) < 0)
				{
					if (node.Left == null)
					{
						node.SetLeft(newNode);
						return newNode;
					}
					node = node.Left;
				}
				else
				{
					if (node.Right == null)
					{
						node.SetRight(newNode);
						return newNode;
					}
					node = node.Right;
				}
			}
		}

		// この実装では、引数で指定されたインスタンスが削除されます。
		public void Remove(Node<T> node)
		{
			if (node == null) throw new ArgumentNullException(nameof(node));

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
