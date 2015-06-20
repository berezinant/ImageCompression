using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BoberImageStudio
{
    [Serializable]
    struct EncodedImage
    {
        internal string decimateMethod;
        internal int YWidth;
        internal int YHeight;
        internal int CrCbWidth;
        internal int CrCbHeight;
        private int YCounter;
        private int CrCounter;
        private int CbCounter;

        internal short[] Y;
        internal short[] Cr;
        internal short[] Cb;

        public EncodedImage(string decimateMethod, int YWidth, int YHeight, int CrCbWidth, int CrCbHeight)
        {
            this.decimateMethod = decimateMethod;
            this.YWidth = YWidth;
            this.YHeight = YHeight;
            this.CrCbWidth = CrCbWidth;
            this.CrCbHeight = CrCbHeight;
            this.YCounter = 0;
            this.CrCounter = 0;
            this.CbCounter = 0;
            this.Y = new short[YWidth * YHeight];
            this.Cr = new short[CrCbWidth * CrCbHeight];
            this.Cb = new short[CrCbWidth * CrCbHeight];
        }

        public void Zigzag(ref double[,] block, ref double[] line, int blockSize, ZigzagDirection direction)
        {
            if (direction == ZigzagDirection.FromBlockToLine)
            {
                line = new double[blockSize * blockSize];
            }
            if (direction == ZigzagDirection.FromLineToBlock)
            {
                block = new double[blockSize, blockSize];
            }
            int counter = 0;
            int i = 0;
            int j = 0;
            ZigzagState state = ZigzagState.Top;
            while ((i < blockSize) && (j < blockSize))
            {
                if (direction == ZigzagDirection.FromBlockToLine)
                {
                    line[counter] = block[i, j];
                }
                if (direction == ZigzagDirection.FromLineToBlock)
                {
                    block[i, j] = line[counter];
                    
                }
                counter++;
                if (state == ZigzagState.Top)
                {
                    j++;
                    state = ZigzagState.MovingDown;
                    continue;
                }
                if (state == ZigzagState.MovingDown)
                {
                    i++;
                    j--;
                    if (i == blockSize - 1)
                    {
                        state = ZigzagState.Bottom;
                        continue;
                    }
                    if (j == 0)
                    {
                        state = ZigzagState.Left;
                        continue;
                    }
                    continue;
                }
                if (state == ZigzagState.Left)
                {
                    i++;
                    state = ZigzagState.MovingUp;
                    continue;
                }
                if (state == ZigzagState.MovingUp)
                {
                    i--;
                    j++;
                    if (i == 0)
                    {
                        state = ZigzagState.Top;
                        continue;
                    }
                    if (j == blockSize - 1)
                    {
                        state = ZigzagState.Right;
                        continue;
                    }
                    continue;
                }
                if (state == ZigzagState.Bottom)
                {
                    j++;
                    state = ZigzagState.MovingUp;
                    continue;
                }
                if (state == ZigzagState.Right)
                {
                    i++;
                    state = ZigzagState.MovingDown;
                    continue;
                }
            }
        }

        internal void appendZigzagedBlock(double[,] block, int blockSize, Channel channel)
        {
            double[] line = new double[blockSize * blockSize];
            Zigzag(ref block, ref line, blockSize, ZigzagDirection.FromBlockToLine);
            switch (channel)
            {
               case Channel.Y:
                  {
                      for (int i = 0; i < line.Length; i++)
                      {
                          Y[YCounter] = (short) line[i];
                          YCounter++;
                      }
                   }
                   break;
                case Channel.Cr:
                   {
                       for (int i = 0; i < line.Length; i++)
                       {
                           Cr[CrCounter] = (short) line[i];
                           CrCounter++;
                       }
                   }
                   break;
               case Channel.Cb:
                   {
                       for (int i = 0; i < line.Length; i++)
                      {
                          Cb[CbCounter] = (short) line[i];
                          CbCounter++;
                      }

                   }
                   break;
                }
            }
        internal void ResetCounters()
        {
            YCounter = 0;
            CrCounter = 0;
            CbCounter = 0;
        }
        internal double[,] ExtractNextBlock(Channel channel, int blockSize)
        {
            int lineSize = blockSize * blockSize;
            double[] line = new double[lineSize];
            switch (channel) {
                case Channel.Y:
                    {
                        for (int i = 0; i < lineSize; i++)
                        {
                            line[i] = (double) Y[YCounter + i];
                        }
                        YCounter += 64;
                    }
                    break;
                case Channel.Cr:
                    {
                        for (int i = 0; i < lineSize; i++)
                        {
                            line[i] = (double) Cr[CrCounter + i];
                        }
                        CrCounter += 64;
                    }
                    break;
                case Channel.Cb:
                    {
                        for (int i = 0; i < lineSize; i++)
                        {
                            line[i] = (double) Cb[CbCounter + i];
                        }
                        CbCounter += 64;
                    }
                    break;
            }
            double[,] result = new double[blockSize, blockSize];
            Zigzag(ref result, ref line, blockSize, ZigzagDirection.FromLineToBlock);
            return result;
        }
    }
}

    internal enum ZigzagState 
    {
        Top, Bottom, Left, Right, MovingUp, MovingDown
    }

    internal enum ZigzagDirection
    {
        FromBlockToLine, FromLineToBlock
    }
