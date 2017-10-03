using System;
using System.Collections.Generic;
using tileRead.Datastructures.Pixel_Representation;
using tileRead.DataStructures;

namespace tileRead.Tools
{
    public static class BitMapAltering
    {
        public static float alphaPerc = 0.1f;

        public static UInt32[] colourValues = new UInt32[]{ 0XFFFFFF, 0XE4E4E4, 0X888888, 0X222222, 0XFFA7D1, 0XE50000, 0XE59500, 0XA06A42, 0XE5D900, 0X94E044, 0X02BE01, 0X00E5F0, 0X0083C7, 0X0000EA, 0XE04AFF, 0X820080 };

        /// <summary>
        /// Were gonna be using Bresenham's line algorithm because of its speed,
        /// and us potentially needing to do this like a bajillion times
        /// </summary>
        /// <param name=p1></param>
        /// <param name=p2></param>
        /// <param name=colour></param>
        public static void drawBresenhamLine(IntermediateBitmap b, Point p1, Point p2, byte[] colour)
        {

        }

        /// <summary>
        /// the fractional part of x
        /// </summary>
        /// <param name=x></param>
        /// <returns></returns>
        private static float fPart(float x)
        {
            if (x < 0)
                return (1 - (x - (float)Math.Floor(x)));
            return x - (float)Math.Floor(x);
        }

        /// <summary>
        /// The complement to the fractional part
        /// </summary>
        private static float rFPart(float x)
        {
            return 1 - fPart(x);
        }

        /// <summary>
        /// Also gonna try to implement Xiaolin Wu's line drawing due to the ability
        /// of anti-aliasing. I want to test to see if it looks any better.
        /// </summary>
        /// <param name=p1></param>
        /// <param name=p2></param>
        /// <param name=colour>Make sure this is in </param>
        public static void drawWuLine(IntermediateBitmap b, Point p1, Point p2, int colourID)
        {
            int x1 = p1.x, x2 = p2.x, y1 = p1.y, y2 = p2.y;
            bool steep = Math.Abs(y2 - y1) > Math.Abs(x2 - x1);
            if (steep)
            {
                int temp = x1;
                x1 = y1;
                y1 = temp;

                temp = x2;
                x2 = y2;
                y2 = temp;
            }
            if (x1 > x2)
            {
                int temp = x1;
                x1 = x2;
                x2 = temp;

                temp = y1;
                y1 = y2;
                y2 = temp;
            }

            int deltaX = x2 - x1;
            int deltaY = y2 - y1;
            float gradient;

            if (deltaX == 0)
            {
                gradient = 1f;
            }
            else
            {
                gradient = deltaY / deltaX;
            }

            #region Generate Starting Pixel
            float xEnd = x1 + 1;
            float yEnd = y1 + gradient;
            float xGap = rFPart(x1 + 0.5f);
            float xPixel1 = xEnd;
            float yPixel1 = (int)yEnd;

            if (steep)
            {
                placePixelOnBoard(b, (int)yPixel1, (int)xPixel1, colourID, rFPart(yEnd) * xGap);
                placePixelOnBoard(b, (int)yPixel1 + 1, (int)xPixel1, colourID, rFPart(yEnd) * xGap);
            }
            else
            {
                placePixelOnBoard(b, (int)xPixel1, (int)yPixel1, colourID, rFPart(yEnd) * xGap);
                placePixelOnBoard(b, (int)xPixel1, (int)yPixel1 + 1, colourID, rFPart(yEnd) * xGap);
            }
            #endregion

            float interY = yEnd + gradient;

            #region Generate Ending Pixel
            xEnd = x2 + 1;
            yEnd = y2 + gradient;
            xGap = rFPart(x2 + 0.5f);
            double xPixel2 = xEnd;
            double yPixel2 = (int)yEnd;

            if (steep)
            {
                placePixelOnBoard(b, (int)yPixel2, (int)xPixel2, colourID, rFPart(yEnd) * xGap);
                placePixelOnBoard(b, (int)yPixel2 + 1, (int)xPixel2, colourID, rFPart(yEnd) * xGap);
            }
            else
            {
                placePixelOnBoard(b, (int)xPixel2, (int)yPixel2, colourID, rFPart(yEnd) * xGap);
                placePixelOnBoard(b, (int)xPixel2, (int)yPixel2 + 1, colourID, rFPart(yEnd) * xGap);
            }
            #endregion

            #region Generate In-Between
            if (steep)
            {
                for (int x = (int)(xPixel1 + 1); x <= xPixel2 - 1; x++)
                {
                    placePixelOnBoard(b, (int)interY, x, colourID, rFPart(interY));
                    placePixelOnBoard(b, (int)interY + 1, x, colourID, fPart(interY));
                    interY += gradient;
                }
            }
            else
            {
                for (int x = (int)(xPixel1 + 1); x <= xPixel2 - 1; x++)
                {
                    placePixelOnBoard(b, x, (int)interY, colourID, rFPart(interY));
                    placePixelOnBoard(b, x, (int)interY + 1, colourID, fPart(interY));
                    interY += gradient;
                }
            }
            #endregion
        }

        /// <summary>
        /// Takes our current bitmap and draws a line on it
        /// </summary>
        /// <param name=x></param>
        /// <param name=y></param>
        /// <param name=colourID></param>
        /// <param name=alpha></param>
        public static void placePixelOnBoard(IntermediateBitmap bit, int x, int y, int colourID, float alpha)
        {
            UInt32 colourVal = colourValues[colourID];
            float r = ((colourVal >> 16) & 0XFF) / 255f;
            float g = ((colourVal >> 8) & 0XFF) / 255f;
            float b = (colourVal & 0XFF) / 255f;
            float a = alpha * alphaPerc;
            System.Drawing.Color c = System.Drawing.Color.FromArgb()

        }



    }
}
