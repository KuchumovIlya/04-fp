using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FCloud.DataReading
{
    public class MystemReader : IDisposable
    {
        private readonly Process process;

        public MystemReader(string pathToMystem)
        {
            var processStartInfo = new ProcessStartInfo
            {
                Arguments = "-nig --format json -e utf-8",
                RedirectStandardOutput = true,
                RedirectStandardInput = true,
                UseShellExecute = false,
                FileName = pathToMystem
            };
            process = new Process { StartInfo = processStartInfo };
            process.Start();
        }

        private static bool IsRussianLetter(char c)
        {
            return Regex.IsMatch(c.ToString(), @"\p{IsCyrillic}");
        }

        public string ReadJson(string word)
        {
            if (!word.All(IsRussianLetter))
                return string.Format("{{\"analysis\":[],\"text\":\"{0}\"}}", word);
            WriteText(word);
            return ReadText();
        }

        private void WriteText(string text)
        {
            var textInBytes = Encoding.UTF8.GetBytes(text + "\n");
            process.StandardInput.BaseStream.Write(textInBytes, 0, textInBytes.Length);
            process.StandardInput.Flush();
        }

        private string ReadText()
        {
            var readedBytes = new List<byte>();
            while (true)
            {
                var readedByte = process.StandardOutput.BaseStream.ReadByte();
                const byte nextLineCharacter = 13;
                if (readedByte == nextLineCharacter)
                {
                    process.StandardOutput.BaseStream.ReadByte();
                    break;
                }
                readedBytes.Add((byte)readedByte);
            }
            return Encoding.UTF8.GetString(readedBytes.ToArray());
        }

        public void Dispose()
        {
            process.Close();
        }
    }
}
