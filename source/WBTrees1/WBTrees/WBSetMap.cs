using System;
using System.Collections.Generic;
using System.Linq;

namespace WBTrees
{
	public abstract class WBSetBase<T> : WBTreeBase<T>
	{
		protected WBSetBase(IComparer<T> comparer = null) : base(comparer) { }

		public Node<T> GetFirst(T item) => Root?.GetFirst(item, Comparer);
		public Node<T> GetLast(T item) => Root?.GetLast(item, Comparer);
		public bool Remove(T item) => RemoveNode(GetLast(item)) != null;

		public int GetFirstIndex(T item) => GetFirst(item)?.GetIndex() ?? -1;
		public int GetLastIndex(T item) => GetLast(item)?.GetIndex() ?? -1;
		public bool Contains(T item) => GetFirst(item) != null;
		public int GetCount(T item) => GetCount(x => Comparer.Compare(x, item) >= 0, x => Comparer.Compare(x, item) <= 0);
	}

	public class WBSet<T> : WBSetBase<T>
	{
		protected override bool IsDistinct => true;
		public WBSet(IComparer<T> comparer = null) : base(comparer) { }
		public Node<T> Get(T item) => GetFirst(item);
		public int GetIndex(T item) => GetFirstIndex(item);
	}

	public class WBMultiSet<T> : WBSetBase<T>
	{
		protected override bool IsDistinct => false;
		public WBMultiSet(IComparer<T> comparer = null) : base(comparer) { }
		public int RemoveAll(T item) => RemoveItems(x => Comparer.Compare(x, item) >= 0, x => Comparer.Compare(x, item) <= 0);
	}

	public abstract class WBMapBase<TKey, TValue> : WBTreeBase<KeyValuePair<TKey, TValue>>
	{
		public IComparer<TKey> KeyComparer { get; }

		protected WBMapBase(IComparer<TKey> keyComparer = null) : base(CreateComparer(keyComparer))
		{
			KeyComparer = keyComparer ?? Comparer<TKey>.Default;
		}

		static IComparer<KeyValuePair<TKey, TValue>> CreateComparer(IComparer<TKey> keyComparer)
		{
			if (keyComparer == null) keyComparer = Comparer<TKey>.Default;
			return Comparer<KeyValuePair<TKey, TValue>>.Create((x, y) => keyComparer.Compare(x.Key, y.Key));
		}

		public Node<KeyValuePair<TKey, TValue>> GetFirst(TKey key) => Root?.GetFirst(new KeyValuePair<TKey, TValue>(key, default(TValue)), Comparer);
		public Node<KeyValuePair<TKey, TValue>> GetLast(TKey key) => Root?.GetLast(new KeyValuePair<TKey, TValue>(key, default(TValue)), Comparer);
		public Node<KeyValuePair<TKey, TValue>> RemoveFirst(TKey key) => RemoveNode(GetFirst(key));
		public Node<KeyValuePair<TKey, TValue>> RemoveLast(TKey key) => RemoveNode(GetLast(key));

		public int GetFirstIndex(TKey key) => GetFirst(key)?.GetIndex() ?? -1;
		public int GetLastIndex(TKey key) => GetLast(key)?.GetIndex() ?? -1;
		public bool ContainsKey(TKey key) => GetFirst(key) != null;
		public int GetCount(TKey key) => GetCount(p => KeyComparer.Compare(p.Key, key) >= 0, p => KeyComparer.Compare(p.Key, key) <= 0);

		public Node<KeyValuePair<TKey, TValue>> Add(TKey key, TValue value) => Add(new KeyValuePair<TKey, TValue>(key, value));
		public void Initialize(IEnumerable<(TKey key, TValue value)> items, bool assertsItems = true) => Initialize(items?.Select(p => new KeyValuePair<TKey, TValue>(p.key, p.value)), assertsItems);
		public int AddItems(IEnumerable<(TKey key, TValue value)> items) => AddItems(items?.Select(p => new KeyValuePair<TKey, TValue>(p.key, p.value)));
	}

	public class WBMap<TKey, TValue> : WBMapBase<TKey, TValue>
	{
		protected override bool IsDistinct => true;
		public WBMap(IComparer<TKey> keyComparer = null) : base(keyComparer) { }

		public Node<KeyValuePair<TKey, TValue>> Get(TKey key) => GetFirst(key);
		public Node<KeyValuePair<TKey, TValue>> Remove(TKey key) => RemoveLast(key);
		public int GetIndex(TKey key) => GetFirstIndex(key);

		public TValue this[TKey key]
		{
			get
			{
				var node = GetFirst(key) ?? throw new KeyNotFoundException("The specified key is not found.");
				return node.Item.Value;
			}
			set
			{
				var item = new KeyValuePair<TKey, TValue>(key, value);
				AddOrGetNode(item).Item = item;
			}
		}
	}

	public class WBMultiMap<TKey, TValue> : WBMapBase<TKey, TValue>
	{
		protected override bool IsDistinct => false;
		public WBMultiMap(IComparer<TKey> keyComparer = null) : base(keyComparer) { }

		public int RemoveAll(TKey key) => RemoveItems(p => KeyComparer.Compare(p.Key, key) >= 0, p => KeyComparer.Compare(p.Key, key) <= 0);

		public IEnumerable<TValue> this[TKey key]
		{
			get
			{
				for (var n = Root?.GetFirst(p => KeyComparer.Compare(p.Key, key) >= 0); n != null && KeyComparer.Compare(n.Item.Key, key) == 0; n = n.GetNext())
					yield return n.Item.Value;
			}
		}
	}
}
