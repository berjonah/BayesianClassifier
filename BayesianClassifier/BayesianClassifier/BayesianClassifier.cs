using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace BayesianClassifier
{
    public class BayesianClassifier
    {
        private int sampleVectorSize;
        private int numberOfClasses;
        public List<double[]> meanVector;
        public List<double[]> stdDvVector;

        public BayesianClassifier(int sampleVectorSize, int numberOfClasses)
        {
            this.sampleVectorSize = sampleVectorSize;
            this.numberOfClasses = numberOfClasses;
            meanVector = new List<double[]>();
            stdDvVector = new List<double[]>();
        }

        public void Train(double[][] samples, int[] classes)
        {
            var zippedVector = samples.Zip(classes,
                (first, second) => new Tuple<double[], int>(first, second));

            meanVector = zippedVector
                .GroupBy(X => X.Item2)
                .Select(Group =>
                {
                    int groupSize = Group.Aggregate(0, (acc, x) => acc + 1);
                    double[] tempArr = new double[groupSize];
                    double[] meanArr = new double[sampleVectorSize];
                    for (int i = 0; i < sampleVectorSize; i++)
                    {
                        for (int j = 0; j < groupSize; j++)
                        {
                            tempArr[j] = Group.ElementAt(j).Item1[i];
                        }
                        meanArr[i] = mean(tempArr);
                    }
                    //Console.WriteLine("Mean for class {0}", Group.Key);
                    //foreach (var el in meanArr)
                    //{
                    //    Console.Write("{0} ", el);
                    //}
                    //Console.WriteLine();
                    return meanArr;
                }).ToList();


            stdDvVector = zippedVector
                .GroupBy(X => X.Item2)
                .Select(Group =>
                {
                    int groupSize = Group.Aggregate(0, (acc, x) => acc + 1);
                    double[] tempArr = new double[groupSize];
                    double[] meanArr = new double[sampleVectorSize];
                    for (int i = 0; i < sampleVectorSize; i++)
                    {
                        for (int j = 0; j < groupSize; j++)
                        {
                            tempArr[j] = Group.ElementAt(j).Item1[i];
                        }
                        meanArr[i] = stdDev(tempArr, meanVector.ElementAt(Group.Key)[i]);
                    }
                    //Console.WriteLine("Std Deviation for class {0}", Group.Key);
                    //foreach (var el in meanArr)
                    //{
                        //Console.Write("{0} ", el);
                    //}
                    //Console.WriteLine();
                    return meanArr;
                }).ToList();
        }

        public int ClassifyBayesian(double[] sample)
        {
            int maxClass = 0;
            double maxProb = findProb(sample, 0);
            for (int i = 1; i < numberOfClasses; i++)
            {
                double newProb = findProb(sample, i);
                if (newProb > maxProb)
                {
                    maxClass = i;
                    maxProb = newProb;
                }
            }
            return maxClass;
        }

        public int ClassifyEuclidean(double[] sample)
        {
            int minClass = 0;
            double minDist = EuclideanDistance(sample, meanVector[0]);
            for (int i = 1; i < numberOfClasses; i++)
            {
                double newDist = EuclideanDistance(sample, meanVector[i]);
                if (newDist < minDist)
                {
                    minClass = i;
                    minDist = newDist;
                }
            }
            return minClass;
        }

        public int[] PNormRanks(double[] sample, int p)
        {
            Tuple<int,double>[] tups = new Tuple<int, double>[numberOfClasses];
            for (int i = 0; i < numberOfClasses; i++)
            {
                tups[i] = new Tuple<int, double>(i,pNormDistance(sample,meanVector[i],p));
            }

            return tups.OrderBy(tup => tup.Item2).Select(tup => tup.Item1).ToArray();
        }

        private double EuclideanDistance(double[] a, double[] b)
        {
            double result = -1;
            if (a.Length == b.Length)
            {
                double sum = 0;
                for (int i = 0; i < a.Length; i++)
                {
                    sum += (a[i] - b[i])*(a[i] - b[i]);
                }
                result = Math.Sqrt(sum);
            }
            return result;
        }

        private double pNormDistance(double[] a, double[] b, int p)
        {
            double result = -1;
            if (a.Length == b.Length)
            {
                double sum = 0;
                for (int i = 0; i < a.Length; i++)
                {
                    sum += Math.Pow(a[i] - b[i],p);
                }
                result = Math.Pow(sum, 1/(double)p);
            }
            return result;
        }

        private double findProb(double[] sample, int cla)
        {
            double prob = 1;
            for (int i = 0; i < sampleVectorSize; i++)
            {
                prob *= Norm(sample[i], meanVector.ElementAt(cla)[i], stdDvVector.ElementAt(cla)[i]);
            }

            return prob;
        }

        private double Norm(double x, double mean, double std)
        {
            double foo = Math.Sqrt(Math.Exp(-(x - mean)*(x - mean)/(2*std*std))/Math.Sqrt(2*std*std*Math.PI));
            return foo;
        }

        private double mean(double[] arr)
        {
            return arr.Aggregate(0.0, (acc, x) => acc + x)/arr.Length;
        }

        private double stdDev(double[] arr, double mean)
        {
            return Math.Sqrt(arr.Aggregate(0.0, (acc, x) => acc + (x - mean)*(x - mean))/(arr.Length - 1)) + Math.Sqrt(Double.Epsilon);
        }
    }
}
