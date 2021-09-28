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

        public static T Random<T>(this T[] target, System.Func<T, int, int> getPercentage)
        {
            int totalVariable = 0;
            for (int i = 0; i < target.Length; i++)
            {
                totalVariable += getPercentage(target[i], i);
            }
            int random = Next(totalVariable);

            totalVariable = 0;
            for (int i = 0; i < target.Length; i++)
            {
                totalVariable += getPercentage(target[i], i);
                if (random < totalVariable)
                {
                    return target[i];
                }
            }

            return default;
        }

        public static T Random<T>(this IReadOnlyList<T> target, System.Func<T, int, int> getPercentage)
        {
            int totalVariable = 0;
            for (int i = 0; i < target.Count; i++)
            {
                totalVariable += getPercentage(target[i], i);
            }
            int random = Next(totalVariable);

            totalVariable = 0;
            for (int i = 0; i < target.Count; i++)
            {
                totalVariable += getPercentage(target[i], i);
                if (random < totalVariable)
                {
                    return target[i];
                }
            }

            return default;
        }

        public static T Random<T>(this IReadOnlyList<T> target, System.Func<T, int, float> getPercentage)
        {
            float totalVariable = 0;
            for (int i = 0; i < target.Count; i++)
            {
                totalVariable += getPercentage(target[i], i);
            }
            float random = Next(totalVariable);

            totalVariable = 0;
            for (int i = 0; i < target.Count; i++)
            {
                totalVariable += getPercentage(target[i], i);
                if (random < totalVariable)
                {
                    return target[i];
                }
            }

            return default;
        }
        public static T Random<T>(this IReadOnlyList<T> target, System.Func<T, bool> onFilter)
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
        public static int Next(int max) => Next(0, max);

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

        /// <summary>
        /// 범위형 float 랜덤
        /// </summary>
        /// <param name="max">최대값, 랜덤값은 이 값이 될 수 없음</param>
        /// <returns></returns>
        public static float Next(float max) => Next(0f, max);

        /// <summary>
        /// 범위형 float 랜덤
        /// </summary>
        /// <param name="max">최대값, 랜덤값은 이 값이 될 수 없음</param>
        /// <returns></returns>
        public static float Next(float min, float max)
        {
            InitRandom();
            return (float)_local.NextDouble() * (max - min) + min;
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
