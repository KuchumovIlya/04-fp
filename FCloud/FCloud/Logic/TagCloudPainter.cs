using System;
using System.Collections.Generic;
using System.Drawing;
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

        public Bitmap DrawWordsWithRateOnBitmap(IEnumerable<Tuple<string, double>> wordsWithRate)
        {
            var bitmap = new Bitmap(options.Width, options.Height);
            var location = new Point(0, 0);

            using (var g = Graphics.FromImage(bitmap))
            {
                g.Clear(Color.AliceBlue);
                foreach (var wordWithRate in wordsWithRate)
                    DrawWordWithRateOnBitmap(g, wordWithRate.Item1, wordWithRate.Item2, ref location);
            }
            return bitmap;
        }

        private void DrawWordWithRateOnBitmap(Graphics g, string word, double rate, ref Point location)
        {
            var wordWidth = word.Length * options.FontSize;
            var wordHeight = options.FontSize;
            if (wordWidth > options.Width)
                return;
            var drawLocation = GetWordLocation(ref location, wordWidth, wordHeight);
            if (drawLocation.X == -1)
                return;
            var font = new Font(FontFamily.GenericMonospace, options.FontSize * (float)Math.Pow(rate, 0.07));
            var color = ControlPaint.Dark(Color.Blue, (float)Math.Pow(rate, 0.2) + (float)0.5);
            var pen = new Pen(color);
            g.DrawString(word, font, pen.Brush, drawLocation.X, drawLocation.Y);
        }

        private Point GetWordLocation(ref Point location, int wordWidth, int wordHeight)
        {
            location.X += wordWidth;

            if (location.X > options.Width)
            {
                var canAdd = Math.Min(options.Width - wordWidth, wordWidth);
                location.X = wordWidth + random.Next(canAdd);
                location.Y += wordHeight;
            }

            return location.Y + wordHeight < options.Height ?
                new Point(location.X - wordWidth, location.Y) : new Point(-1, -1);
        }
    }
}
