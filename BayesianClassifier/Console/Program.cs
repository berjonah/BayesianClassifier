using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BayesianClassifier;

namespace Console
{
    class Program
    {
        
        static void Main(string[] args)
        {
            ImageReader imageReaderTraining = new ImageReader(@"C:\tmp\train-images.idx3-ubyte");
            LabelReader labelReaderTraining = new LabelReader(@"C:\tmp\train-labels.idx1-ubyte");
            ImageReader imageReaderVerify = new ImageReader(@"C:\tmp\t10k-images.idx3-ubyte");
            LabelReader labelReaderVerify = new LabelReader(@"C:\tmp\t10k-labels.idx1-ubyte");
            int[][] samplesInts = imageReaderTraining.ConvertToInts();

            double[][] samplesTraining = imageReaderTraining.ConvertToNormalizedDoubles();
            int[] labelsTraining = labelReaderTraining.ConvertToInts();

            double[][] samplesVerify = imageReaderVerify.ConvertToNormalizedDoubles();
            int[] labelsVerify = labelReaderVerify.ConvertToInts();

            BayesianClassifier.BayesianClassifier classifier = new BayesianClassifier.BayesianClassifier(784,10);
            System.Console.WriteLine("Starting Training");
            classifier.Train(samplesTraining,labelsTraining);
            System.Console.WriteLine("Done Training");
            System.Console.ReadLine();

            //foreach (var image in classifier.meanVector)
            //{
            //    int[] lard=new int[image.Length];
            //    for (int i = 0; i < image.Length; i++)
            //    {
            //        lard[i] = (int) image[i];
            //    }
            //    ImageCreater foo = new ImageCreater(lard,28,28);
            //    System.Console.ReadLine();
            //}
            //foreach (var image in classifier.stdDvVector)
            //{
            //    int[] lard = new int[image.Length];
            //    for (int i = 0; i < image.Length; i++)
            //    {
            //        lard[i] = (int)image[i];
            //    }
            //    ImageCreater foo = new ImageCreater(lard, 28, 28);
            //    System.Console.ReadLine();
            //}

            for (int i = 0; i < labelsTraining.Length; i++)
            {
                ImageCreater creater = new ImageCreater(samplesInts[i],28,28);
                //System.Console.WriteLine("Bayesian {0} {1} {2}",classifier.ClassifyBayesian(samplesTraining[i]), labelsTraining[i], classifier.ClassifyBayesian(samplesTraining[i]) == labelsTraining[i]);
                //System.Console.WriteLine("Euclidean {0} {1} {2}", classifier.ClassifyEuclidean(samplesTraining[i]), labelsTraining[i], classifier.ClassifyEuclidean(samplesTraining[i]) == labelsTraining[i]);
                System.Console.WriteLine("Label: {0}", labelsTraining[i]);
                for (int p = 0; p < 100; p++)
                {
                    System.Console.Write("p={0}: ",p);
                    foreach (var rank in classifier.PNormRanks(samplesTraining[i],p))
                    {
                        System.Console.Write("{0} ", rank);
                    }
                    System.Console.WriteLine();
                }
                System.Console.ReadLine();
            }
        }
    }
}
