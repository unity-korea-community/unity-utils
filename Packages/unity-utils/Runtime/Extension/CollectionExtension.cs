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
                        _stringBuilder.Append(", ");
                }

                _stringBuilder.Append("]");

            }

            return _stringBuilder.ToString();
        }

        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> target)
        {
            return new HashSet<T>(target);
        }

        public static IEnumerable<T> Foreach<T>(this IEnumerable<T> target, System.Action<T> OnEach)
        {
            foreach (var item in target)
                OnEach(item);

            return target;
        }

        public static T Dequeue<T>(this List<T> target)
        {
            int index = 0;
            T item = target[index];
            target.RemoveAt(index);

            return item;
        }

        public static T Pop<T>(this List<T> target)
        {
            int index = target.Count - 1;
            T item = target[index];
            target.RemoveAt(index);

            return item;
        }
    }
}
