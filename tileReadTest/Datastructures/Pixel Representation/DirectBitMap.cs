using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace tileRead.Datastructures.Pixel_Representation
{

    public class DirectBitmap : IDisposable
    {
        public Bitmap Bitmap { get; private set; }
        public Int32[] Bits { get; private set; }
        public bool Disposed { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        protected GCHandle BitsHandle { get; private set; }

        public DirectBitmap(int width, int height)
        {
            Width = width;
            Height = height;
            Bits = new Int32[width * height];
            BitsHandle = GCHandle.Alloc(Bits, GCHandleType.Pinned);
            Bitmap = new Bitmap(width, height, width * 4, PixelFormat.Format32bppPArgb, BitsHandle.AddrOfPinnedObject());
        }

        public void Dispose()
        {
            if (Disposed) return;
            Disposed = true;
            Bitmap.Dispose();
            BitsHandle.Free();
        }
    }

    public class CustomColour
    {
        public float a;
        public float r;
        public float g;
        public float b;

        public CustomColour(float A, float R, float G, float B)
        {
            a = A;
            r = R;
            g = G;
            b = B;
        }
    }

    public class IntermediateBitmap
    {
        public int width;
        public int height;
        public Stack<CustomColour>[][] array;

        public IntermediateBitmap(int dimensionSize) : this(dimensionSize, dimensionSize){}

        public IntermediateBitmap(int width, int height)
        {
            array = new Stack<CustomColour>[width][];
            for(int i = 0; i < width; i++)
            {
                array[i] = new Stack<CustomColour>[height];
            }
        }

        public DirectBitmap toBitMap()
        {
            DirectBitmap retVal = new DirectBitmap(width, height);
            for(int i = 0; i < width; i++)
            {
                for(int j = 0; j < height; j++)
                {
                    Color c = BlendStack(array[i][j]);
                    retVal.Bitmap.SetPixel(i, j, c);
                }
            }
            return retVal;
        }

        private Color BlendStack(Stack<CustomColour> stack)
        {
            while(stack.Count > 1)
            {

            }
        }
    }
}
