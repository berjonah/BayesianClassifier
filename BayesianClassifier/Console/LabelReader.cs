using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console
{
    class LabelReader
    {
        private byte[] bytes;
        private int magicNumber;
        private int numberOfItems;


        public LabelReader(string filepath)
        {
            bytes = File.ReadAllBytes(filepath);

            magicNumber = BitConverter.ToInt32(bytes.Take(4).Reverse().ToArray(), 0);
            numberOfItems = BitConverter.ToInt32(bytes.Skip(4).Take(4).Reverse().ToArray(), 0);
        }

        public int[] ConvertToInts()
        {
            int[] ints = new int[numberOfItems];
            for (int i = 0; i < numberOfItems; i++)
            {
                ints[i] = Convert.ToInt32(bytes[8 + i]);
            }

            return ints;
        }
    }
}
