using System;
using System.Linq;
using Newtonsoft.Json;

namespace FCloud.DataReading
{
    public class MystemWordsFilter
    {
        //See https://tech.yandex.ru/mystem/doc/grammemes-values-docpage/#parts to learn more.
        private readonly string[] notInterestingPartsOfSpeech = {
            "APRO",
            "COM",
            "CONJ",
            "INTJ",
            "NUM",
            "PART",
            "PR",
            "SPRO",
            "ADVPRO"
        };

        private readonly Func<string, string> readMystemDataInJson;

        public MystemWordsFilter(Func<string, string> readMystemDataInJson)
        {
            this.readMystemDataInJson = readMystemDataInJson;
        }

        public bool IsInterestingWord(string word)
        {
            var mystemDataInJson = readMystemDataInJson(word);
            dynamic mystemData = JsonConvert.DeserializeObject(mystemDataInJson);

            if (mystemData.analysis.Count == 0)
                return false;

            var gr = mystemData.analysis[0].gr.ToString();
            return notInterestingPartsOfSpeech.All(possiblePart => !gr.StartsWith(possiblePart));
        }
    }
}
