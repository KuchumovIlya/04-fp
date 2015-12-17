using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace FCloud.DataReading
{
    public class WordsParser
    {
        public IEnumerable<string> ParseWords(string text)
        {
            var regExp = new Regex(@"\W|\d");
            return regExp
                .Split(text.ToLower())
                .Where(s => s != "");
        }
    }
}
