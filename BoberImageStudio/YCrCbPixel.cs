using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoberImageStudio
{
    class YCrCbPixel
    {
        public int Y { get; private set; }
        public int Cr { get; private set; }
        public int Cb { get; private set; }

        public YCrCbPixel(int Y, int Cr, int Cb)
        {
            this.Y = Y;
            this.Cr = Cr;
            this.Cb = Cb;
        }
        
        public override string ToString() 
        {
            return "" + Y + " " + Cr + " " + Cb;
        }
    }
}
