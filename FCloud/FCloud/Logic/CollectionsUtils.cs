using System;
using System.Collections.Generic;
using System.Linq;

namespace FCloud.Logic
{
    public static class CollectionsUtils
    {
        //todo add unit tests

        public static IEnumerable<T> RandomShuffle<T>(this IEnumerable<T> source, Random random)
        {
            var sourceArray = source.ToArray();
            for (var i = 1; i < sourceArray.Length; i++)
            {
                var swapIndex = random.Next(i - 1);
                var temp = sourceArray[swapIndex];
                sourceArray[swapIndex] = sourceArray[i];
                sourceArray[i] = temp;
            }
            return sourceArray;
        }

        public static IEnumerable<Tuple<T, double>> CalculateRate<T>(this IEnumerable<T> source)
        {
            var sourceArray = source.ToArray();

            var count = sourceArray
                .GroupBy(x => x)
                .ToDictionary(g => g.Key, g => g.Count());
            var maxCount = count.Max(t => t.Value);

            return sourceArray.Select(x => Tuple.Create(x, (double) count[x] / maxCount));
        }
    }
}
