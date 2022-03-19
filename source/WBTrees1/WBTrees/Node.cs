using System;
using System.Collections.Generic;
using System.Linq;

namespace WBTrees
{
	/// <summary>
	/// Represents a node of weight-balanced binary trees.
	/// </summary>
	/// <typeparam name="T">The type of the item.</typeparam>
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

		public Node<T> GetFirst(T item, IComparer<T> comparer)
		{
			if (comparer == null) comparer = Comparer<T>.Default;
			var d = comparer.Compare(item, Item);
			if (d == 0) return Left?.GetFirst(item, comparer) ?? this;
			else if (d < 0) return Left?.GetFirst(item, comparer);
			else return Right?.GetFirst(item, comparer);
		}

		public Node<T> GetLast(T item, IComparer<T> comparer)
		{
			if (comparer == null) comparer = Comparer<T>.Default;
			var d = comparer.Compare(item, Item);
			if (d == 0) return Right?.GetLast(item, comparer) ?? this;
			else if (d > 0) return Right?.GetLast(item, comparer);
			else return Left?.GetLast(item, comparer);
		}

		#endregion

		#region Get Node (by Index)

		// out of range: null
		public Node<T> GetAt(int index)
		{
			var d = index - LeftCount;
			if (d == 0) return this;
			else if (d < 0) return Left?.GetAt(index);
			else return Right?.GetAt(d - 1);
		}

		#endregion

		#region Get Index

		public int GetIndex()
		{
			if (Parent == null) return LeftCount;
			else if (Parent.Left == this) return Parent.GetIndex() - RightCount - 1;
			else return Parent.GetIndex() + LeftCount + 1;
		}

		// not found: Count
		public int GetFirstIndex(Func<T, bool> predicate)
		{
			if (predicate?.Invoke(Item) ?? true) return Left?.GetFirstIndex(predicate) ?? 0;
			else return (Right?.GetFirstIndex(predicate) ?? 0) + LeftCount + 1;
		}

		// not found: -1
		public int GetLastIndex(Func<T, bool> predicate)
		{
			if (predicate?.Invoke(Item) ?? true) return (Right?.GetLastIndex(predicate) ?? -1) + LeftCount + 1;
			else return Left?.GetLastIndex(predicate) ?? -1;
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
	/// Provides a set of extension methods for the <see cref="Node{T}"/> class.
	/// </summary>
	public static class NodeHelper
	{
		public static bool Exists<T>(this Node<T> node) => node != null;

		public static T GetItemOrDefault<T>(this Node<T> node, T defaultItem = default(T)) => node != null ? node.Item : defaultItem;
		public static bool TryGetItem<T>(this Node<T> node, out T item)
		{
			item = node != null ? node.Item : default(T);
			return node != null;
		}

		public static TValue GetValueOrDefault<TKey, TValue>(this Node<KeyValuePair<TKey, TValue>> node, TValue defaultValue = default(TValue)) => node != null ? node.Item.Value : defaultValue;
		public static bool TryGetValue<TKey, TValue>(this Node<KeyValuePair<TKey, TValue>> node, out TValue value)
		{
			value = node != null ? node.Item.Value : default(TValue);
			return node != null;
		}
	}
}
