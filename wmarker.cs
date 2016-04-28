using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWatermark
{
    internal class WMarker
    {
        private TextureBrush _watermarkBrush;
        private Image _watermarkImage;
        
        public WMarker()
        { }

        public WMarker(string watermarkPath)
        {
            _watermarkImage = Image.FromFile(watermarkPath);
            _watermarkBrush = new TextureBrush(_watermarkImage);
        }

        public void ApplyWatermark(string sourceImagePath, string watermarkPath, string outputImagePath)
        {
            using (Image image = Image.FromFile(sourceImagePath))
            using (Image watermarkImage = Image.FromFile(watermarkPath))
            using (Graphics imageGraphics = Graphics.FromImage(image))
            using (TextureBrush watermarkBrush = new TextureBrush(watermarkImage))
            {
                int x = (image.Width / 2 - watermarkImage.Width / 2);
                int y = (image.Height / 2 - watermarkImage.Height / 2);
                watermarkBrush.TranslateTransform(x, y);
                imageGraphics.FillRectangle(watermarkBrush, new Rectangle(new Point(x, y), new Size(watermarkImage.Width+1, watermarkImage.Height)));
                image.Save(outputImagePath);
            }
        }

        public void ApplyWatermark(string sourceImagePath, string outputImagePath)
        {
            using (Image image = Image.FromFile(sourceImagePath))            
            using (Graphics imageGraphics = Graphics.FromImage(image))            
            {
                int x = (image.Width / 2 - _watermarkImage.Width / 2);
                int y = (image.Height / 2 - _watermarkImage.Height / 2);
                _watermarkBrush.TranslateTransform(x, y);
                imageGraphics.FillRectangle(_watermarkBrush, new Rectangle(new Point(x, y), new Size(_watermarkImage.Width + 1, _watermarkImage.Height)));
                image.Save(outputImagePath);
            }        
        }
    }
}
