using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace FCloud
{
    public class TagCloudBuilder
    {
        public Bitmap Build(string text,
            Func<string, IEnumerable<string>> parseWords,
            Func<string, string> normalizeWord,
            Func<string, bool> isInterestingWord,
            Func<IEnumerable<string>, Bitmap> drawOnBitmap)
        {
            var words = parseWords(text)
                .Select(normalizeWord)
                .Where(isInterestingWord);
            return drawOnBitmap(words);
        }
    }
}
