using System;
using System.Collections.Generic;
using System.Linq;

namespace UNKO.Utils
{
    /// <summary>
    /// Random 관련 로직을 Unity Engine 종속성을 제거하고 구현했습니다.
    /// </summary>
    public static class RandomExtension
    {
        public static T Random<T>(this IEnumerable<T> target)
        {
            int randomIndex = Next(0, target.Count());
            return target.ElementAt(randomIndex);
        }

        public static T Random<T>(this IEnumerable<T> target, System.Func<T, int> getPercentage)
        {
            int totalvariable = 0;
            target.Foreach(item => totalvariable += getPercentage(item));
            int random = Next(0, totalvariable);

            totalvariable = 0;
            foreach (T item in target)
            {
                totalvariable += getPercentage(item);
                if (random < totalvariable)
                    return item;
            }

            return default;
        }

        public static T Random<T>(this IEnumerable<T> target, System.Func<T, bool> onFilter)
        {
            IEnumerable<T> filteredTarget = target.Where(onFilter);
            int randomIndex = Next(0, filteredTarget.Count());
            return filteredTarget.ElementAt(randomIndex);
        }

        public static List<T> Shuffle<T>(this List<T> target)
        {
            target.Sort((a, b) => Next(-1, 2));
            return target;
        }



        // thread safe한 System.Random
        // https://stackoverflow.com/questions/3049467/is-c-sharp-random-number-generator-thread-safe
        private static readonly Random s_global = new Random();
        [ThreadStatic] private static Random _local;

        public static int Next()
        {
            InitRandom();
            return _local.Next();
        }

        /// <summary>
        /// 범위형 int 랜덤
        /// </summary>
        /// <param name="max">최대값, 랜덤값은 이 값이 될 수 없음</param>
        /// <returns></returns>
        public static int Next(int max)
        {
            InitRandom();
            return _local.Next(max);
        }

        /// <summary>
        /// 범위형 int 랜덤
        /// </summary>
        /// <param name="min">최소값, 랜덤값은 이 값이 될 수 있음</param>
        /// <param name="max">최대값, 랜덤값은 이 값이 될 수 없음</param>
        /// <returns></returns>
        public static int Next(int min, int max)
        {
            InitRandom();
            return _local.Next(min, max);
        }

        private static void InitRandom()
        {
            if (_local == null)
            {
                lock (s_global)
                {
                    if (_local == null)
                    {
                        int seed = s_global.Next();
                        _local = new Random(seed);
                    }
                }
            }
        }

    }
}
