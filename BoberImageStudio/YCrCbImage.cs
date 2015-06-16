using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoberImageStudio
{
    class YCrCbImage
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        YCrCbPixel[,] pixels;

        public YCrCbImage(int width, int height)
        {
            Width = width;
            Height = height;
            pixels = new YCrCbPixel[width, height];
        }

        internal void SetPixel(int i, int j, YCrCbPixel pixel)
        {
            pixels[i, j] = pixel;
        }

        internal YCrCbPixel GetPixel(int i, int j)
        {
            return pixels[i, j];
        }
    }
}
