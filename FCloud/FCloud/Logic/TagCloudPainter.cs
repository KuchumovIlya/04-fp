using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FCloud.Logic
{
    public class TagCloudPainter
    {
        private readonly Options options;
        private readonly Random random;

        public TagCloudPainter(Options options, Random random)
        {
            this.options = options;
            this.random = random;
        }

        public Bitmap DrawWordsWithRateOnBitmap(IEnumerable<RatedWord> ratedWords)
        {
            var lastWordLocation = new Point(0, 0);
            var lastWordBox = new Size(0, 0);

            var wordPlacements = ratedWords
                .Select(x => new WordBox(x, GetWordBox(x)))
                .Where(x => x.Box.Width <= options.Width)
                .Where(x => x.Box.Height <= options.Height)
                .Select(x =>
                {
                    lastWordLocation = GetNewWordLocation(lastWordLocation, lastWordBox, x.Box.Width, x.Box.Height);
                    lastWordBox = x.Box;
                    return new WordPlacement(x.RatedWord, lastWordLocation, x.Box);
                })
                .Where(x => IsInBoundingBox(x.Location, x.Box.Width, x.Box.Height));

            return Draw(wordPlacements);
        }

        private Point GetNewWordLocation(Point oldWordLocation, Size oldWordBox, int newWordWidth, int newWordHeight)
        {
            var newLocation = oldWordLocation + new Size(oldWordBox.Width, 0);
            return IsInBoundingBox(newLocation, newWordWidth, newWordHeight)
                ? newLocation
                : MoveWordToNewLine(oldWordLocation, newWordWidth, newWordHeight);
        }

        private Point MoveWordToNewLine(Point location, int wordWidth, int wordHeight)
        {
            var widthMaximumShift = Math.Min(options.Width - wordWidth, wordWidth);
            var newX = random.Next(widthMaximumShift);
            var newY = location.Y + wordHeight;
            return new Point(newX, newY);
        }

        private bool IsInBoundingBox(Point location, int wordWidth, int wordHeight)
        {
            return 0 <= location.X && location.X + wordWidth < options.Width &&
                   0 <= location.Y && location.Y + wordHeight < options.Height;
        }

        private struct WordPlacement
        {
            public RatedWord RatedWord { get; private set; }
            public Point Location { get; private set; }
            public Size Box { get; private set; }

            public WordPlacement(RatedWord ratedWord, Point location, Size box) 
                : this()
            {
                RatedWord = ratedWord;
                Location = location;
                Box = box;
            }
        }

        private struct WordBox
        {
            public RatedWord RatedWord { get; private set; }
            public Size Box { get; private set; }

            public WordBox(RatedWord ratedWord, Size box) 
                : this()
            {
                RatedWord = ratedWord;
                Box = box;
            }
        }

        private Bitmap Draw(IEnumerable<WordPlacement> wordPlacements)
        {
            var bitmap = new Bitmap(options.Width, options.Height);
            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.Clear(Color.AliceBlue);
                foreach (var wordPlacement in wordPlacements)
                {
                    var word = wordPlacement.RatedWord.Word;
                    var font = CreateFont(wordPlacement.RatedWord.Rate);
                    var brush = CreateBrush(wordPlacement.RatedWord.Rate);
                    graphics.DrawString(word, font, brush, wordPlacement.Location.X, wordPlacement.Location.Y);
                }
            }
            return bitmap;
        }

        private Size GetWordBox(RatedWord ratedWord)
        {
            return new Size(ratedWord.Word.Length * options.FontSize, options.FontSize);
        }

        private Font CreateFont(double rate)
        {
            return new Font(FontFamily.GenericMonospace, options.FontSize * (float)Math.Pow(rate, 0.07));
        }

        private static Brush CreateBrush(double rate)
        {
            var color = ControlPaint.Dark(Color.Blue, (float)Math.Pow(rate, 0.2) + (float)0.5);
            return new Pen(color).Brush;
        }
    }
}
