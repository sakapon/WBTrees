using System;
using System.Collections.Generic;
using System.Linq;

namespace TreesLab.AvlTrees
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
	public abstract class AvlTreeBase<T> : IEnumerable<T>
	{
		public Node<T> Root { get; private set; }
		public int Count { get; private set; }
		public IComparer<T> Comparer { get; }
		protected abstract bool IsDistinct { get; }

		protected AvlTreeBase(IComparer<T> comparer = null)
		{
			Comparer = comparer ?? Comparer<T>.Default;
		}

		// Use this method to update the Root object.
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

		public void Initialize(IEnumerable<T> items)
		{
			if (items == null) throw new ArgumentNullException(nameof(items));
			// stable sort
			var a = items.OrderBy(x => x, Comparer).ToArray();
			if (IsDistinct)
				for (int i = 1; i < a.Length; ++i)
					if (Comparer.Compare(a[i - 1], a[i]) == 0) throw new ArgumentException("The keys must be unique.", nameof(items));
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
				yield return n.Item;
		}

		public IEnumerable<T> GetItemsDescending()
		{
			for (var n = Root?.GetLastNode(); n != null; n = n.GetPreviousNode())
				yield return n.Item;
		}

		public IEnumerable<T> GetItems(Func<T, bool> startPredicate, Func<T, bool> endPredicate)
		{
			if (startPredicate == null) throw new ArgumentNullException(nameof(startPredicate));
			if (endPredicate == null) throw new ArgumentNullException(nameof(endPredicate));

			for (var n = Root?.GetFirstNode(startPredicate); n != null && endPredicate(n.Item); n = n.GetNextNode())
				yield return n.Item;
		}

		public IEnumerable<T> GetItemsDescending(Func<T, bool> startPredicate, Func<T, bool> endPredicate)
		{
			if (startPredicate == null) throw new ArgumentNullException(nameof(startPredicate));
			if (endPredicate == null) throw new ArgumentNullException(nameof(endPredicate));

			for (var n = Root?.GetLastNode(endPredicate); n != null && startPredicate(n.Item); n = n.GetPreviousNode())
				yield return n.Item;
		}

		#endregion

		#region Remove Items

		public int RemoveItems(Func<T, bool> startPredicate, Func<T, bool> endPredicate)
		{
			if (startPredicate == null) throw new ArgumentNullException(nameof(startPredicate));
			if (endPredicate == null) throw new ArgumentNullException(nameof(endPredicate));

			var nodes = new List<Node<T>>();
			for (var n = Root?.GetLastNode(endPredicate); n != null && startPredicate(n.Item); n = n.GetPreviousNode())
				nodes.Add(n);

			foreach (var n in nodes) RemoveNode(n);
			return nodes.Count;
		}

		#endregion

		#region Get Item

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

		#endregion

		#region Add Item(s)

		public bool Add(T item)
		{
			var c = Count;
			AddOrGetNode(item);
			return Count != c;
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
				++Count;
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
						++Count;
						break;
					}
					node = node.Left;
				}
				else
				{
					if (node.Right == null)
					{
						node.SetRight(newNode = new Node<T> { Item = item });
						++Count;
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
		protected bool RemoveNode(Node<T> node)
		{
			if (node == null) return false;

			Node<T> dirty;
			if (node.Left == null || node.Right == null)
			{
				UpdateChild(node, node.Left ?? node.Right);
				dirty = node.Parent;
			}
			else
			{
				var node2 = dirty = node.Right.GetFirstNode();
				if (node2 != node.Right)
				{
					dirty = node2.Parent;
					UpdateChild(node2, node2.Right);
					node2.SetRight(node.Right);
				}
				node2.SetLeft(node.Left);
				UpdateChild(node, node2);
			}
			dirty?.UpdateHeight(true);
			--Count;
			return true;
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
			var lrh = t.LeftHeight - t.RightHeight;
			if (lrh > 2 || lrh == 2 && t.Left.LeftHeight >= t.Left.RightHeight)
			{
				t = RotateToRight(t);
			}
			else if (lrh < -2 || lrh == -2 && t.Right.LeftHeight <= t.Right.RightHeight)
			{
				t = RotateToLeft(t);
			}
			t.UpdateHeight();
			return t;
		}

		// Suppose t != null.
		Node<T> RotateToRight(Node<T> t)
		{
			var p = t.Left;
			UpdateChild(t, p);
			t.SetLeft(p.Right);
			p.SetRight(t);
			t.UpdateHeight();
			return p;
		}

		// Suppose t != null.
		Node<T> RotateToLeft(Node<T> t)
		{
			var p = t.Right;
			UpdateChild(t, p);
			t.SetRight(p.Left);
			p.SetLeft(t);
			t.UpdateHeight();
			return p;
		}

		#endregion
	}

	public abstract class AvlSetBase<T> : AvlTreeBase<T>
	{
		protected AvlSetBase(IComparer<T> comparer = null) : base(comparer) { }
		public bool Contains(T item) => Root?.GetFirstNode(item, Comparer) != null;
		public bool Remove(T item) => RemoveNode(Root?.GetLastNode(item, Comparer));
	}

	public class AvlSet<T> : AvlSetBase<T>
	{
		protected override bool IsDistinct => true;
		public AvlSet(IComparer<T> comparer = null) : base(comparer) { }
	}

	public class AvlMultiSet<T> : AvlSetBase<T>
	{
		protected override bool IsDistinct => false;
		public AvlMultiSet(IComparer<T> comparer = null) : base(comparer) { }
		public int RemoveAll(T item) => RemoveItems(x => Comparer.Compare(x, item) >= 0, x => Comparer.Compare(x, item) <= 0);
	}

	public abstract class AvlMapBase<TKey, TValue> : AvlTreeBase<KeyValuePair<TKey, TValue>>
	{
		public IComparer<TKey> KeyComparer { get; }

		protected AvlMapBase(IComparer<TKey> keyComparer = null) : base(CreateComparer(keyComparer))
		{
			KeyComparer = keyComparer ?? Comparer<TKey>.Default;
		}

		static IComparer<KeyValuePair<TKey, TValue>> CreateComparer(IComparer<TKey> keyComparer)
		{
			if (keyComparer == null) keyComparer = Comparer<TKey>.Default;
			return Comparer<KeyValuePair<TKey, TValue>>.Create((x, y) => keyComparer.Compare(x.Key, y.Key));
		}

		protected Node<KeyValuePair<TKey, TValue>> GetFirstNode(TKey key) => Root?.GetFirstNode(new KeyValuePair<TKey, TValue>(key, default(TValue)), Comparer);
		protected Node<KeyValuePair<TKey, TValue>> GetLastNode(TKey key) => Root?.GetLastNode(new KeyValuePair<TKey, TValue>(key, default(TValue)), Comparer);

		public bool ContainsKey(TKey key) => GetFirstNode(key) != null;
		public bool Add(TKey key, TValue value) => Add(new KeyValuePair<TKey, TValue>(key, value));
	}

	public class AvlMap<TKey, TValue> : AvlMapBase<TKey, TValue>
	{
		protected override bool IsDistinct => true;
		public AvlMap(IComparer<TKey> keyComparer = null) : base(keyComparer) { }

		public bool TryGetValue(TKey key, out TValue value)
		{
			var node = GetFirstNode(key);
			value = node != null ? node.Item.Value : default(TValue);
			return node != null;
		}

		public TValue GetValueOrDefault(TKey key, TValue defaultValue = default(TValue)) => TryGetValue(key, out var value) ? value : defaultValue;

		public TValue this[TKey key]
		{
			get => TryGetValue(key, out var value) ? value : throw new KeyNotFoundException("The specified key is not found.");
			set
			{
				var item = new KeyValuePair<TKey, TValue>(key, value);
				AddOrGetNode(item).Item = item;
			}
		}

		public void SetItems(IEnumerable<KeyValuePair<TKey, TValue>> items)
		{
			if (items == null) throw new ArgumentNullException(nameof(items));
			foreach (var p in items) AddOrGetNode(p).Item = p;
		}

		public bool Remove(TKey key) => RemoveNode(GetLastNode(key));
	}

	public class AvlMultiMap<TKey, TValue> : AvlMapBase<TKey, TValue>
	{
		protected override bool IsDistinct => false;
		public AvlMultiMap(IComparer<TKey> keyComparer = null) : base(keyComparer) { }

		public bool TryGetFirstValue(TKey key, out TValue value)
		{
			var node = GetFirstNode(key);
			value = node != null ? node.Item.Value : default(TValue);
			return node != null;
		}

		public bool TryGetLastValue(TKey key, out TValue value)
		{
			var node = GetLastNode(key);
			value = node != null ? node.Item.Value : default(TValue);
			return node != null;
		}

		public TValue GetFirstValueOrDefault(TKey key, TValue defaultValue = default(TValue)) => TryGetFirstValue(key, out var value) ? value : defaultValue;
		public TValue GetLastValueOrDefault(TKey key, TValue defaultValue = default(TValue)) => TryGetLastValue(key, out var value) ? value : defaultValue;

		public IEnumerable<TValue> this[TKey key]
		{
			get
			{
				for (var n = Root?.GetFirstNode(p => KeyComparer.Compare(p.Key, key) >= 0); n != null && KeyComparer.Compare(n.Item.Key, key) == 0; n = n.GetNextNode())
					yield return n.Item.Value;
			}
		}

		public bool RemoveFirst(TKey key) => RemoveNode(GetFirstNode(key));
		public bool RemoveLast(TKey key) => RemoveNode(GetLastNode(key));
		public int RemoveAll(TKey key) => RemoveItems(p => KeyComparer.Compare(p.Key, key) >= 0, p => KeyComparer.Compare(p.Key, key) <= 0);
	}
}
