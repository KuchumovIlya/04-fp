using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;

namespace FCloud.DataWriting
{
    public class ImageBitmapWriter
    {
        //todo add unit tests
        public void WriteBitmap(Bitmap bitmap, Options options)
        {
            var toImageFormatConverter = new Dictionary<string, ImageFormat>
            {
                {".png", ImageFormat.Png},
                {".jpeg", ImageFormat.Jpeg},
                {".bmp", ImageFormat.Bmp}
            };
            bitmap.Save(options.OutputFilePath + options.OutputFormat, toImageFormatConverter[options.OutputFormat]);
        }
    }
}
