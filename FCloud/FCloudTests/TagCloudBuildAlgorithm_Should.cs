using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FCloud;
using FCloud.DataReading;
using FCloud.DataWriting;
using FCloud.Logic;
using Microsoft.VisualStudio.TestTools.UITesting;
using NUnit.Framework;

namespace FCloudTests
{
    [TestFixture]
    class TagCloudBuildAlgorithm_Should
    {
        [Test]
        public void BuildBitmap()
        {
            var options = new Options
            {
                Width = 500,
                FontSize = 30,
                Height = 500,
                InputFilePath = "..//../input_small.txt",
                OutputFilePath = "../../output.png",
                PathToMystem = "../../mystem.exe"
            };

            var parser = new WordsParser();
            var reader = new MystemReader(options.PathToMystem);
            var normalizer = new MystemWordsNormalizer(reader.ReadMystemDataInJson);
            var filter = new MystemWordsFilter(reader.ReadMystemDataInJson);
            var random = new Random(20);
            var painter = new TagCloudPainter(options, random);
            var algorithm = new TagCloudBuildAlgorithm(random, painter.DrawWordsWithRateOnBitmap);
            var builder = new TagCloudBuilder();

            var text = File.ReadAllText(options.InputFilePath);
            var actual = builder.Build(text, parser.ParseWords, normalizer.NormalizeWord, filter.IsInterestingWord, algorithm.BuildBitmap);

            var expected = Image.FromFile(options.OutputFilePath);

            Assert.IsTrue(ImageComparer.Compare(actual, expected));
        }
    }
}
