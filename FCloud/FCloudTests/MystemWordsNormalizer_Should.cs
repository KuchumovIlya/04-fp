using FCloud.DataReading;
using NUnit.Framework;

namespace FCloudTests
{
    [TestFixture]
    public class MystemWordsNormalizer_Should
    {
        [Test]
        public void NormalizeVerbs()
        {
            var normalizer = new MystemWordsNormalizer(s =>
                "{\"analysis\":[{\"lex\":\"бежать\",\"gr\":\"V,нп=(прош,ед,изъяв,муж,несов|прош,ед,изъяв,муж,сов)\"}],\"text\":\"бежал\"}");
            const string word = "бежал";
            const string expected = "бежать";

            var actual = normalizer.NormalizeWord(word);
            
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void NormalizeAdjectives()
        {
            var normalizer = new MystemWordsNormalizer(s =>
               "{\"analysis\":[{\"lex\":\"домашний\",\"gr\":\"A=(пр,мн,полн|вин,мн,полн,од|род,мн,полн)\"}," +
               "{\"lex\":\"домашние\",\"gr\":\"S,мн,од=(пр|вин|род)\"}],\"text\":\"домашних\"}");
            const string word = "домашних";
            const string expected = "домашний";

            var actual = normalizer.NormalizeWord(word);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void NormalizeNouns()
        {
            var normalizer = new MystemWordsNormalizer(s =>
                "{\"analysis\":[{\"lex\":\"великолепность\",\"qual\":\"bastard\"," +
                "\"gr\":\"S,жен,неод=(пр,ед|вин,мн|дат,ед|род,ед|им,мн)\"}],\"text\":\"великолепности\"}");
            const string word = "великолепности";
            const string expected = "великолепность";

            var actual = normalizer.NormalizeWord(word);

            Assert.AreEqual(expected, actual);
        }
    }
}
