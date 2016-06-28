using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyWatermark
{
    public class PhotoItem
    {
        public String Name { get; private set; }
        public String FilePath { get; private set; }
        public String FullPath { get; private set; }
        public String Dimension { get { return _height + "x" + _width; } }

        private Int32 _height;
        private Int32 _width;


        public PhotoItem(string fileName)
        {
            _height = 0;
            _width = 0;

            if (File.Exists(fileName))
            {
                FullPath = Path.GetFullPath(fileName);
                FilePath = Path.GetFullPath(fileName);
                Name = Path.GetFileName(fileName);

                System.Drawing.Image img = System.Drawing.Image.FromFile(fileName);

                if (img != null)
                {
                    _height = img.Height;
                    _width = img.Width;
                }
            }
        }
    }
}
