using System;
using System.Collections.Generic;
using System.IO;


namespace tileRead.DataStructures
{

    /// <summary>
    /// Class containing an x, y axis int type
    /// </summary>
    public class Point
    {
        #region Fields
        public int x;
        public int y;
        #endregion

        #region Constructors
        public Point()
        {
            x = 0;
            y = 0;
        }

        public Point(int i1, int i2)
        {
            x = i1;
            y = i2;
        }
        #endregion

        #region Comparators
        /// <summary>
        /// Compares a point based on x, y, and z coords
        /// in that order
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public int CompareTo(Point other)
        {
            int compareResult;
            //check x
            if ((compareResult = this.x.CompareTo(other.x)) == 0)
            {
                //if x is equal check y
                return this.y.CompareTo(other.y);
                //otherwise fall through
            }
            //otherwise return this
            return compareResult;
        }

        public override bool Equals(object obj)
        {
            if (obj is Point)
                return Equals(obj as Point);
            return false;
        }

        public bool Equals(Point other)
        {
            if (other == null)
                return false;
            //check if they are literally the same object
            if (ReferenceEquals(this, other))
                return true;
            return (other.x.Equals(x) && other.y.Equals(y));
        }

        public override int GetHashCode()
        {
            int hash = 7;
            hash = 71 * hash + x;
            hash = 71 * hash + y;
            return hash;
        }
        #endregion
       
        #region Math Methods
        public static float distance(Point p1, Point p2)
        {
            float a = p1.x - p2.x;
            float b = p1.y - p2.y;
            return (float)Math.Sqrt(Math.Pow(a, 2) + Math.Pow(b, 2));
        }

        public static Point add(Point p1, Point p2)
        {
            int newX = p1.x + p2.x;
            int newY = p1.y + p2.y;
            return new Point(newX, newY);
        }


        public static Point subtract(Point p1, Point p2)
        {
            int newX = p1.x - p2.x;
            int newY = p1.y - p2.y;
            return new Point(newX, newY);
        }

        public static Point subtract(Point p1, int val)
        {
            int newX = p1.x - val;
            int newY = p1.y - val;
            return new Point(newX, newY);
        }

        public static Point add(Point p1, int val)
        {
            int newX = p1.x + val;
            int newY = p1.y + val;
            return new Point(newX, newY);
        }

        public static Point multiply(Point p1, int val)
        {
            int newX = p1.x * val;
            int newY = p1.y * val;
            return new Point(newX, newY);
        }

        public static Point multiply(Point p1, Point p2)
        {
            int newX = p1.x * p2.x;
            int newY = p1.y * p2.y;
            return new Point(newX, newY);
        }

        public static Point divide(Point p1, Point p2)
        {
            int newX = (int)(p1.x / p2.x);
            int newY = (int)(p1.y / p2.y);
            return new Point(newX, newY);
        }

        public static Point divide(Point p1, float val)
        {
            int newX = (int)(p1.x / val);
            int newY = (int)(p1.y / val);
            return new Point(newX, newY);
        }
        #endregion

        #region IO Methods
        /// <summary>
        /// given a binary writer, and a type, will write
        /// the given point to the writer as a the given type
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="point"></param>
        /// <param name="t"></param>
        public static void WritePoint(BinaryWriter writer, Point point)
        {
            writer.Write(point.x);
            writer.Write(point.y);
        }

        /// <summary>
        /// Given a list of points, and a filename, will write
        /// the points with type of t to the file
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="t"></param>
        /// <param name="points"></param>
        public static void WritePoints(string fileName, List<Point> points)
        {
            FileStream stream = File.Create(fileName);
            BinaryWriter writer = new BinaryWriter(stream);
            for (int i = 0; i < points.Count; i++)
            {
                WritePoint(writer, points[i]);
            }
            writer.Close();
            stream.Close();
        }

        /// <summary>
        /// Given a reader, and a type, will read from the stream the
        /// correct number of bytes
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Point ReadPoint(BinaryReader reader)
        {
            int xv = reader.ReadInt32();
            int yv = reader.ReadInt32();
            return new Point(xv, yv);
        }

        /// <summary>
        /// Given a file name, and the type of binary reading you would like,
        /// will binary read a file into a list of IPoints
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        public static List<Point> ReadPointsFromFile(string fileName)
        {
            FileStream stream = File.OpenRead(fileName);
            BinaryReader reader = new BinaryReader(stream);
            List<Point> retVal = new List<Point>();
            while (reader.BaseStream.Position != reader.BaseStream.Length)
            {
                Point returned = ReadPoint(reader);
                retVal.Add(returned);
            }
            reader.Close();
            stream.Close();
            return retVal;
        }
        #endregion
    }
}

