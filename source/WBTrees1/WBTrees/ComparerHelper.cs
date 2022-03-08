using System;
using System.Collections.Generic;

namespace WBTrees
{
	public static class ComparerHelper
	{
		public static IComparer<T> GetDefault<T>()
		{
			// Speeds up a string comparison that is independent of language.
			if (typeof(T) == typeof(string)) return (IComparer<T>)StringComparer.Ordinal;
			return Comparer<T>.Default;
		}

		public static IComparer<T> ToDescending<T>(this IComparer<T> c)
		{
			if (c == null) throw new ArgumentNullException(nameof(c));
			return Comparer<T>.Create((x, y) => c.Compare(y, x));
		}
	}

	public static class ComparerHelper<T>
	{
		public static IComparer<T> Create(bool descending = false)
		{
			var c = ComparerHelper.GetDefault<T>();
			return descending ? c.ToDescending() : c;
		}

		public static IComparer<T> Create<TKey>(Func<T, TKey> keySelector, bool descending = false)
		{
			if (keySelector == null) throw new ArgumentNullException(nameof(keySelector));
			var c = ComparerHelper<TKey>.Create(descending);
			return Comparer<T>.Create((x, y) => c.Compare(keySelector(x), keySelector(y)));
		}

		public static IComparer<T> Create<TKey1, TKey2>(Func<T, TKey1> keySelector1, bool descending1, Func<T, TKey2> keySelector2, bool descending2)
		{
			if (keySelector1 == null) throw new ArgumentNullException(nameof(keySelector1));
			if (keySelector2 == null) throw new ArgumentNullException(nameof(keySelector2));
			var c1 = ComparerHelper<TKey1>.Create(descending1);
			var c2 = ComparerHelper<TKey2>.Create(descending2);
			return Comparer<T>.Create((x, y) =>
			{
				var d = c1.Compare(keySelector1(x), keySelector1(y));
				if (d != 0) return d;
				return c2.Compare(keySelector2(x), keySelector2(y));
			});
		}

		public static IComparer<T> Create<TKey1, TKey2, TKey3>(Func<T, TKey1> keySelector1, bool descending1, Func<T, TKey2> keySelector2, bool descending2, Func<T, TKey3> keySelector3, bool descending3)
		{
			if (keySelector1 == null) throw new ArgumentNullException(nameof(keySelector1));
			if (keySelector2 == null) throw new ArgumentNullException(nameof(keySelector2));
			if (keySelector3 == null) throw new ArgumentNullException(nameof(keySelector3));
			var c1 = ComparerHelper<TKey1>.Create(descending1);
			var c2 = ComparerHelper<TKey2>.Create(descending2);
			var c3 = ComparerHelper<TKey3>.Create(descending3);
			return Comparer<T>.Create((x, y) =>
			{
				var d = c1.Compare(keySelector1(x), keySelector1(y));
				if (d != 0) return d;
				d = c2.Compare(keySelector2(x), keySelector2(y));
				if (d != 0) return d;
				return c3.Compare(keySelector3(x), keySelector3(y));
			});
		}
	}
}
