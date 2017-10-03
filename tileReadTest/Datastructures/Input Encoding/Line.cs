using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using tileRead.DataStructures;

namespace tileRead.Datastructures
{
    /// <summary>
    /// A class for generating an array of points corresponding to a states' "walk" 
    /// </summary>
    public class Line
    {
        private string hash;
        private Point[] linePoints;
        /// <summary>
        /// used using because the math is very computationally expensive
        /// and it is quicker to do it once, then keep looking up, then to
        /// recalculate it for all of our hashes
        /// </summary>
        private static Dictionary<byte, Point> byteDistributionDictionary;
        private static double P = Math.PI * (3 - (Math.Sqrt(5)));

        /// <summary>
        /// Implementation of Vogel's method to spread points over a disc; extended to a sphere
        /// </summary>
        public static void generateByteDistributionDictionary()
        {
            ///We generate a point for every byte
            for(byte i = 0; i <= byte.MaxValue; i++)
            {
                double Pi = P * i;
                float Zi = (1 - (1 / 256)) * (1 - (2 * i / (256 - 1)));
                double PSIi = Math.Sqrt(1 - (Zi*Zi));

                float Xi = (float)(Pi*Math.Cos(PSIi));
                float Yi = (float)(Pi * Math.Sin(PSIi));
                //byteDistributionDictionary.Add(i, new Point(Xi, Yi));
            }
        }

        public Line(string userHash)
        {
            ///Sha-1 hash is a 160bit digest i.e. 20 bytes
            linePoints = new Point[20];
            generateLine(userHash);
        }
        
        /// <summary>
        /// for creation of a line from a known line
        /// </summary>
        /// <param name="userHash"></param>
        /// <param name="points"></param>
        public Line(string userHash, Point[] points)
        {
            hash = userHash;
            linePoints = points;
        }

        public Point[] getLine()
        {
            ///don't need to check for null because there is no null constructor for Line
            return linePoints;
        }

        public string getHash()
        {
            ///don't need to check for null because there is no null constructor for Line
            return hash;
        }

        private void generateLine(string hash)
        {
            byte[] bytes = System.Convert.FromBase64String(hash);
            ///Every point is dependant on its predecessor
            ///An explanation of this process should be provided via documentation
            for (int i = 0; i < bytes.Length; i++)
            {
                if(i == 0)
                {
                    linePoints[0] = byteDistributionDictionary[bytes[i]];
                }
                else
                {
                    linePoints[i] = Point.add(linePoints[i - 1], byteDistributionDictionary[bytes[i]]);
                }
            }
        }

        
    }
}
