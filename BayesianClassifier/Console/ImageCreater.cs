using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Console
{
    
    class ImageCreater
    {
        private Bitmap imageBitmap;

        public ImageCreater(int[] pixels, int numRows, int numCol)
        {
            imageBitmap=new Bitmap(numRows,numCol);
            for (int i = 0; i < numRows; i++)
            {
                for (int j = 0; j < numCol; j++)
                {
                    imageBitmap.SetPixel(j,i,Color.FromArgb(pixels[i*numCol+j], pixels[i * numCol + j], pixels[i * numCol + j]));
                }
            }
            Random rng = new Random();
            imageBitmap.Save(String.Format(@"C:\tmp\{0}.jpg",rng.Next()), System.Drawing.Imaging.ImageFormat.Jpeg);

        }

    }
}
