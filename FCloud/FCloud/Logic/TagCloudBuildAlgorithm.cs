using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FCloud.Logic
{
    public class TagCloudBuildAlgorithm
    {
        private readonly Random random;
        private readonly Func<IEnumerable<Tuple<string, double>>, Bitmap> drawWordsWithRateOnBitmap;

        //todo add unit tests
        public TagCloudBuildAlgorithm(Random random, Func<IEnumerable<Tuple<string, double>>, Bitmap> drawWordsWithRateOnBitmap)
        {
            this.random = random;
            this.drawWordsWithRateOnBitmap = drawWordsWithRateOnBitmap;
        }

        public Bitmap BuildBitmap(IEnumerable<string> words)
        {
            var wordsWithRate = words
                .RandomShuffle(random)
                .CalculateRate()
                .Distinct();

            return drawWordsWithRateOnBitmap(wordsWithRate);
        }   
    }
}
