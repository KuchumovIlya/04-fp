using FCloud.DataReading;
using NUnit.Framework;

namespace FCloudTests
{
    [TestFixture]
    public class MystemWordsFilter_Should
    {
        [Test]
        public void AcceptVerbrs()
        {
            var normalizer = new MystemWordsFilter(s =>
                "{\"analysis\":[{\"lex\":\"бежать\",\"gr\":\"V,нп=(прош,ед,изъяв,муж,несов|прош,ед,изъяв,муж,сов)\"}],\"text\":\"бежать\"}");
            const string word = "бежать";

            var actual = normalizer.IsInterestingWord(word);

            Assert.IsTrue(actual);
        }

        [Test]
        public void AcceptAdjectives()
        {
            var normalizer = new MystemWordsFilter(s =>
                "{\"analysis\":[{\"lex\":\"домашний\",\"gr\":\"A=(пр,мн,полн|вин,мн,полн,од|род,мн,полн)\"}," +
                "{\"lex\":\"домашние\",\"gr\":\"S,мн,од=(пр|вин|род)\"}],\"text\":\"домашний\"}");
            const string word = "домашний";

            var actual = normalizer.IsInterestingWord(word);

            Assert.IsTrue(actual);
        }

        [Test]
        public void AcceptNouns()
        {
            var normalizer = new MystemWordsFilter(s =>
                "{\"analysis\":[{\"lex\":\"великолепность\",\"qual\":\"bastard\"," +
                "\"gr\":\"S,жен,неод=(пр,ед|вин,мн|дат,ед|род,ед|им,мн)\"}],\"text\":\"великолепность\"}");
            const string word = "великолепность";

            var actual = normalizer.IsInterestingWord(word);

            Assert.IsTrue(actual);
        }

        [Test]
        public void NotAcceptDigits()
        {
            var normalizer = new MystemWordsFilter(s =>
                "{\"analysis\":[],\"text\":\"123\"}");
            const string word = "123";

            var actual = normalizer.IsInterestingWord(word);

            Assert.IsFalse(actual);
        }

        [Test]
        public void NotAcceptEnglishWords()
        {
            var normalizer = new MystemWordsFilter(s =>
                "{\"analysis\":[],\"text\":\"run\"}");
            const string word = "run";

            var actual = normalizer.IsInterestingWord(word);

            Assert.IsFalse(actual);
        }

        [Test]
        public void NotAcceptParticles()
        {
            var normalizer = new MystemWordsFilter(s =>
                "{\"analysis\":[{\"lex\":\"для\",\"gr\":\"PR=\"}],\"text\":\"для\"}");
            const string word = "для";

            var actual = normalizer.IsInterestingWord(word);

            Assert.IsFalse(actual);
        }

        [Test]
        public void NotAcceptPronouns()
        {
            var normalizer = new MystemWordsFilter(s =>
                "{\"analysis\":[{\"lex\":\"она\",\"gr\":\"SPRO,ед,3-л,жен=им\"}],\"text\":\"она\"}");
            const string word = "она";

            var actual = normalizer.IsInterestingWord(word);

            Assert.IsFalse(actual);
        }

        [Test]
        public void AcceptNumerals()
        {
            var normalizer = new MystemWordsFilter(s =>
                "{\"analysis\":[{\"lex\":\"одинадцатый\",\"qual\":\"bastard\",\"gr\":\"ANUM=(вин,ед,муж,неод|им,ед,муж)\"}]," +
                "\"text\":\"одинадцатый\"}");
            const string word = "одинадцатый";

            var actual = normalizer.IsInterestingWord(word);

            Assert.IsTrue(actual);
        }
    }
}
