using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Console
{
    class ImageReader
    {
        private byte[] bytes;
        private int magicNumber;
        private int numberOfImages;
        private int numberOfRows;
        private int numberOfColumns;
        public ImageReader(string filePath)
        {
            bytes = File.ReadAllBytes(filePath);
            //System.Console.ReadLine();
            magicNumber = BitConverter.ToInt32(bytes.Take(4).Reverse().ToArray(), 0);
            numberOfImages = BitConverter.ToInt32(bytes.Skip(4).Take(4).Reverse().ToArray(), 0);
            numberOfRows = BitConverter.ToInt32(bytes.Skip(8).Take(4).Reverse().ToArray(), 0);
            numberOfColumns = BitConverter.ToInt32(bytes.Skip(12).Take(4).Reverse().ToArray(), 0);
        }

        public int[][] ConvertToInts()
        {
            int[][] ints = new int[numberOfImages][];
            for (int i = 0; i < numberOfImages; i++)
            {
                ints[i] = new int[numberOfRows*numberOfColumns];
                for (int j = 0; j < numberOfRows*numberOfColumns; j++)
                {
                    ints[i][j] = Convert.ToInt32(bytes[16 + 784*i + j]);
                }
            }
            return ints;
        }

        public double[][] ConvertToNormalizedDoubles()
        {
            double[][] doubles = new double[numberOfImages][];
            for (int i = 0; i < numberOfImages; i++)
            {
                doubles[i] = new double[numberOfRows * numberOfColumns];
                for (int j = 0; j < numberOfRows * numberOfColumns; j++)
                {
                    doubles[i][j] = (double)Convert.ToInt32(bytes[16 + 784 * i + j]);// / 255;
                }
            }
            return doubles;
        }
    }
}
