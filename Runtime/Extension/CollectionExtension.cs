using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UNKO.Utils
{
    public static class CollectionExtension
    {
        private static StringBuilder _stringBuilder = new StringBuilder();

        public static string ToStringCollection<T>(this IEnumerable<T> target) => ToStringCollection(target.ToArray());
        public static string ToStringCollection<T>(this T[] target)
        {
            _stringBuilder.Length = 0;

            _stringBuilder.Append($"Count: {target.Length}, ");
            if (target.Length != 0)
            {
                _stringBuilder.Append("[");
                for (int i = 0; i < target.Length; i++)
                {
                    _stringBuilder.Append(target[i].ToString());
                    if (i < target.Length - 1)
                    {
                        _stringBuilder.Append(", ");
                    }
                }
                _stringBuilder.Append("]");
            }

            return _stringBuilder.ToString();
        }

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> target, System.Action<T> OnEach)
        {
            foreach (var item in target)
            {
                OnEach(item);
            }

            return target;
        }

        public static bool IsEmpty<T>(this T[] target)
        {
            return target == null || target.Length == 0;
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> target)
        {
            return new HashSet<T>(target);
        }


        public static T Dequeue<T>(this List<T> target)
        {
            int index = 0;
            T item = target[index];
            target.RemoveAt(index);

            return item;
        }

        public static void Push<T>(this List<T> target, T addedItem)
        {
            target.Add(addedItem);
        }

        public static T Pop<T>(this List<T> target)
        {
            int index = target.Count - 1;
            T item = target[index];
            target.RemoveAt(index);

            return item;
        }

        public static T Peek<T>(this List<T> target)
        {
            int index = target.Count - 1;
            T item = target[index];

            return item;
        }

        public static Dictionary<TKey, TSource> ToDictionaryEx<TSource, TKey>(this IEnumerable<TSource> source, System.Func<TSource, TKey> keySelector)
            => ToDictionaryEx(source, keySelector, UnityEngine.Debug.LogError);

        public static Dictionary<TKey, TSource> ToDictionaryEx<TSource, TKey>(this IEnumerable<TSource> source, System.Func<TSource, TKey> keySelector, System.Action<string> OnError)
        {
            Dictionary<TKey, TSource> dictionary = new Dictionary<TKey, TSource>();
            foreach (var eachSource in source)
            {
                TKey key = keySelector(eachSource);
                if (dictionary.ContainsKey(key))
                {
                    OnError($"{nameof(ToDictionaryEx)} already ContainKey, key:{key}, added Source:{dictionary[key]}, try add source:{eachSource}");
                    continue;
                }
                dictionary.Add(key, eachSource);
            }

            return dictionary;
        }

        public static TValue GetValueEx<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> target, TKey key)
        {
            target.TryGetValueEx(key, out TValue value);
            return value;
        }

        public static bool TryGetValueEx<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> target, TKey key, out TValue value)
            => TryGetValueEx(target, key, out value, UnityEngine.Debug.LogError);

        public static bool TryGetValueEx<TKey, TValue>(this IReadOnlyDictionary<TKey, TValue> target, TKey key, out TValue value, System.Action<string> OnError)
        {
            value = default;
            if (target == null)
            {
                OnError($"Dictionary is null, key:{key}");
                return false;
            }

            if (key == null)
            {
                OnError($"key is null value");
                return false;
            }

            bool result = target.TryGetValue(key, out value);
            if (!result)
            {
                OnError($"key:{key} not found in dictionary");
            }

            return result;
        }

        public static bool IsNullOrZeroCount<T>(this IEnumerable<T> target)
        {
            return target == null || target.Count() == 0;
        }

        public static bool IsNullOrZeroCount(this Array target)
        {
            return target == null || target.Length == 0;
        }
    }
}