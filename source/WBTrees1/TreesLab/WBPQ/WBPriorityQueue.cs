using System;
using System.Collections.Generic;
using System.Linq;

namespace TreesLab.WBPQ
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

		public Node<T> GetFirstNode() => Left?.GetFirstNode() ?? this;
		public Node<T> GetLastNode() => Right?.GetLastNode() ?? this;
	}

	[System.Diagnostics.DebuggerDisplay(@"Count = {Count}")]
	public class WBPriorityQueue<T> : IEnumerable<T>
	{
		Node<T> Root;
		public int Count => Root?.Count ?? 0;
		public IComparer<T> Comparer { get; }

		public WBPriorityQueue(IComparer<T> comparer = null)
		{
			Comparer = comparer ?? Comparer<T>.Default;
		}

		// Root を変更するには、このメソッドを経由します。
		void SetRoot(Node<T> node)
		{
			Root = node;
			if (node != null) node.Parent = null;
		}

		public void Clear() => SetRoot(null);

		public void Initialize(IEnumerable<T> collection)
		{
			if (collection == null) throw new ArgumentNullException(nameof(collection));
			var items = collection.ToArray();
			Array.Sort(items, Comparer);
			SetRoot(CreateSubtree(items, 0, items.Length));
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

		public IEnumerator<T> GetEnumerator() => GetItems().GetEnumerator();
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetItems().GetEnumerator();
		public IEnumerable<T> GetItems()
		{
			var r = new List<T>();
			Dfs(Root);
			return r.ToArray();

			void Dfs(Node<T> node)
			{
				if (node == null) return;
				Dfs(node.Left);
				r.Add(node.Item);
				Dfs(node.Right);
			}
		}

		public IEnumerable<T> GetItemsDescending()
		{
			var r = new List<T>();
			Dfs(Root);
			return r.ToArray();

			void Dfs(Node<T> node)
			{
				if (node == null) return;
				Dfs(node.Right);
				r.Add(node.Item);
				Dfs(node.Left);
			}
		}

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

		public T PopFirst()
		{
			AssertNotEmpty();
			var node = Root.GetFirstNode();
			var item = node.Item;
			Remove(node);
			return item;
		}

		public T PopLast()
		{
			AssertNotEmpty();
			var node = Root.GetLastNode();
			var item = node.Item;
			Remove(node);
			return item;
		}

		// Suppose node != null.
		void Remove(Node<T> node)
		{
			if (node.Left == null || node.Right == null)
			{
				var child = node.Left ?? node.Right;

				var parent = node.Parent;
				if (parent == null)
					SetRoot(child);
				else if (parent.Left == node)
					parent.SetLeft(child);
				else
					parent.SetRight(child);

				parent?.UpdateCount(true);
			}
			else
			{
				var node2 = node.Right.GetFirstNode();
				node.Item = node2.Item;
				Remove(node2);
			}
		}

		public void Push(T item)
		{
			var newNode = new Node<T> { Item = item };
			SetRoot(Add(Root, newNode));
		}

		public void PushItems(IEnumerable<T> items)
		{
			if (items == null) throw new ArgumentNullException(nameof(items));
			foreach (var x in items) Push(x);
		}

		Node<T> Add(Node<T> node, Node<T> newNode)
		{
			if (node == null) return newNode;

			if (Comparer.Compare(newNode.Item, node.Item) < 0)
				node.SetLeft(Add(node.Left, newNode));
			else
				node.SetRight(Add(node.Right, newNode));

			node = Balance(node);
			node.UpdateCount();
			return node;
		}

		// Suppose t != null.
		static Node<T> Balance(Node<T> t)
		{
			var lc = t.LeftCount + 1;
			var rc = t.RightCount + 1;
			if (lc > 2 * rc)
			{
				t = RotateToRight(t);
				t.Right.UpdateCount();
			}
			else if (rc > 2 * lc)
			{
				t = RotateToLeft(t);
				t.Left.UpdateCount();
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
	}

	public class WBPriorityQueue<T, TKey> : WBPriorityQueue<KeyValuePair<TKey, T>>
	{
		readonly Func<T, TKey> KeySelector;

		public WBPriorityQueue(Func<T, TKey> keySelector, IComparer<TKey> comparer = null) : base(CreateComparer(comparer))
		{
			KeySelector = keySelector ?? throw new ArgumentNullException(nameof(keySelector));
		}

		static IComparer<KeyValuePair<TKey, T>> CreateComparer(IComparer<TKey> comparer)
		{
			comparer ??= Comparer<TKey>.Default;
			return Comparer<KeyValuePair<TKey, T>>.Create((x, y) => comparer.Compare(x.Key, y.Key));
		}

		public void Push(T item)
		{
			Push(new KeyValuePair<TKey, T>(KeySelector(item), item));
		}

		public void PushItems(IEnumerable<T> items)
		{
			if (items == null) throw new ArgumentNullException(nameof(items));
			foreach (var x in items) Push(x);
		}
	}
}
