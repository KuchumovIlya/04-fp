using System;
using System.IO;
using FCloud;
using FCloud.DataReading;
using FCloud.DataWriting;
using FCloud.Logic;

namespace ConsoleFCloud
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new Options();
            if (!CommandLine.Parser.Default.ParseArguments(Environment.GetCommandLineArgs(), options))
            {
                //maybe show usage
                return;
            }
            
            var parser = new WordsParser();
            var reader = new MystemReader(options.PathToMystem);
            var normalizer = new MystemWordsNormalizer(reader.ReadMystemDataInJson);
            var filter = new MystemWordsFilter(reader.ReadMystemDataInJson);
            var random = new Random();
            var painter = new TagCloudPainter(options, random);
            var algorithm = new TagCloudBuildAlgorithm(random, painter.DrawWordsWithRateOnBitmap);
            var builder = new TagCloudBuilder();
            var writer = new ImageBitmapWriter();

            var text = File.ReadAllText(options.InputFilePath);
            var bitmap = builder.Build(text, parser.ParseWords, normalizer.NormalizeWord, filter.IsInterestingWord, algorithm.BuildBitmap);
            writer.WriteBitmap(bitmap, options);
        }
    }
}
