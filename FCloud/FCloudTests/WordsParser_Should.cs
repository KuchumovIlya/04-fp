using FCloud.DataReading;
using NUnit.Framework;

namespace FCloudTests
{
    [TestFixture]
    public class WordsParser_Should
    {
        [Test]
        public void SplitTextOnLettersTokens()
        {
            const string text = "ab аб cd";
            var parser = new WordsParser();
            var expected = new[] {"ab", "аб", "cd"};
            
            var actual = parser.ParseWords(text);
        
            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void SkipDigitsTokens()
        {
            const string text = "a 123 b";
            var parser = new WordsParser();
            var expected = new[] { "a", "b" };

            var actual = parser.ParseWords(text);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void SkipEmptyWords()
        {
            const string text = "   ab    аб    cd    ";
            var parser = new WordsParser();
            var expected = new[] { "ab", "аб", "cd" };

            var actual = parser.ParseWords(text);

            CollectionAssert.AreEqual(expected, actual);
        }

        [Test]
        public void ChangeLettersToLowercase()
        {
            const string text = "ААББCCZZ";
            var parser = new WordsParser();
            var expected = new[] { "ааббcczz" };

            var actual = parser.ParseWords(text);

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
