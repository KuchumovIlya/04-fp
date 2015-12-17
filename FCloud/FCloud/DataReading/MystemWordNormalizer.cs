using System;
using Newtonsoft.Json;

namespace FCloud.DataReading
{
    public class MystemWordNormalizer
    {
        private readonly Func<string, string> readMystemDataInJson;

        public MystemWordNormalizer(Func<string, string> readMystemDataInJson)
        {
            this.readMystemDataInJson = readMystemDataInJson;
        }

        public string NormalizeWord(string word)
        {
            var mystemDataInJson = readMystemDataInJson(word);
            dynamic mystemData = JsonConvert.DeserializeObject(mystemDataInJson);

            return mystemData.analysis.Count == 0 ?
                word : mystemData.analysis[0].lex.ToString();
        }
    }
}
