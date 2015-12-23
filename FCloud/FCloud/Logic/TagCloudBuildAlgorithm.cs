using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FCloud.Logic
{
    public class TagCloudBuildAlgorithm
    {
        private readonly Random random;
        private readonly Func<IEnumerable<RatedWord>, Bitmap> drawWordsWithRateOnBitmap;

        //todo add unit tests
        public TagCloudBuildAlgorithm(Random random, Func<IEnumerable<RatedWord>, Bitmap> drawWordsWithRateOnBitmap)
        {
            this.random = random;
            this.drawWordsWithRateOnBitmap = drawWordsWithRateOnBitmap;
        }

        public Bitmap BuildBitmap(IEnumerable<string> words)
        {
            var shuffledWords = RandomShuffle(words);
            var wordsWithRate = CalculateRate(shuffledWords).Distinct();
            return drawWordsWithRateOnBitmap(wordsWithRate);
        }

        public IEnumerable<T> RandomShuffle<T>(IEnumerable<T> source)
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

        public IEnumerable<RatedWord> CalculateRate(IEnumerable<string> words)
        {
            var sourceArray = words.ToArray();

            var count = sourceArray
                .GroupBy(x => x)
                .ToDictionary(g => g.Key, g => g.Count());
            var maxCount = count.Max(t => t.Value);

            return sourceArray.Select(x => new RatedWord { Word = x, Rate = (double)count[x] / maxCount});
        }
    }
}
