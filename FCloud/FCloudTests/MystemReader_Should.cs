using FCloud.DataReading;
using NUnit.Framework;

namespace FCloudTests
{
    [TestFixture]
    public class MystemReader_Should
    {
        const string PathToMystem = "../../mystem.exe";

        [Test]
        public void ReadDataForRussianWords()
        {
            const string word = "бежать";
            const string expected = "{\"analysis\":[{\"lex\":\"бежать\",\"gr\":\"V,нп=(инф,несов|инф,сов)\"}],\"text\":\"бежать\"}";
            using (var parser = new MystemReader(PathToMystem))
            {
                var actual = parser.ReadJson(word);
                Assert.AreEqual(expected, actual);
            }
        }

        [Test]
        public void ReadEmptyDataForDigits()
        {
            const string word = "1234";
            const string expected = "{\"analysis\":[],\"text\":\"1234\"}";
            using (var parser = new MystemReader(PathToMystem))
            {
                var actual = parser.ReadJson(word);
                Assert.AreEqual(expected, actual);
            }
        }

        [Test]
        public void ReadEmptyDataForEnglishWords()
        {
            const string word = "home";
            const string expected = "{\"analysis\":[],\"text\":\"home\"}";
            using (var parser = new MystemReader(PathToMystem))
            {
                var actual = parser.ReadJson(word);
                Assert.AreEqual(expected, actual);
            }
        }
    }
}
