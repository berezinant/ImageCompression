using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace BoberImageStudio
{
    public partial class BoberForm : Form
    {
        public BoberForm()
        {
            InitializeComponent();
        }
#region: Загрузка и сохранение
        private void SaveButton1_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Title = "Save Image";
                dlg.Filter = "bmp files (*.bmp)|*.bmp";
                dlg.RestoreDirectory = true;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    OriginalImageBox.Image.Save(dlg.FileName);
                }
            }
        }

        private void SaveButton2_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Title = "Save Image";
                dlg.Filter = "bmp files (*.bmp)|*.bmp";
                dlg.RestoreDirectory = true;

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    ModifiedImageBox.Image.Save(dlg.FileName);
                }
            }
        }

        private void OpenButton1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "bmp files (*.bmp)|*.bmp";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    OriginalImageBox.Image = new Bitmap(dlg.FileName);
                }
            }
        }

        private void OpenButton2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = "Open Image";
                dlg.Filter = "bmp files (*.bmp)|*.bmp";

                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    ModifiedImageBox.Image = new Bitmap(dlg.FileName);
                }
            }
        }
        #endregion: Загрузка и сохранение
#region: Очистка формы
        private void ClearButton_Click(object sender, EventArgs e)
        {
            OriginalImageBox.Image = null;
            ModifiedImageBox.Image = null;
            PSNRBox.Text = null;
            YTrack.Value = 8;
            UTrack.Value = 8;
            VTrack.Value = 8;
            YTrack1.Value = 8;
            CrTrack.Value = 8;
            CbTrack.Value = 8;
        }
#endregion: Очистка формы
#region: Два черно-белых изображения
        private void GrayscaleButton_Click(object sender, EventArgs e)
        {
            Bitmap src = (Bitmap)OriginalImageBox.Image;
            Bitmap res = toGrayscale(src, true);
            ModifiedImageBox.Image = res;
            res = toGrayscale(src, false);
            OriginalImageBox.Image = res;
        }

        private Bitmap toGrayscale(Bitmap src, bool weighted)
        {
            if (src != null)
            {
                Bitmap res = new Bitmap(src.Width, src.Height);
                for (int i = 0; i < src.Height; i++)
                {
                    for (int j = 0; j < src.Width; j++)
                    {
                        Color color = src.GetPixel(i, j);
                        int R = color.R;
                        int G = color.G;
                        int B = color.B;
                        int Gray = 0;
                        if (weighted)
                        {
                            Gray = (int)(0.299 * R + 0.587 * G + 0.114 * B);
                        }
                        else
                        {
                            Gray = (int)(0.333 * R + 0.333 * G + 0.333 * B);
                        }
                        color = Color.FromArgb(Gray, Gray, Gray);
                        res.SetPixel(i, j, color);
                    }
                }
                return res;
            }
            return null;
        }
        #endregion: Два черно-белых изображения
#region: Подсчет PSNR
        private void PSNRButton_Click(object sender, EventArgs e)
        {
            Bitmap orig = (Bitmap)OriginalImageBox.Image;
            Bitmap modif = (Bitmap)ModifiedImageBox.Image;
            if (orig != null && modif != null)
            {
                double psnr = PSNR(orig, modif);
                if (psnr < Double.MaxValue)
                {
                    PSNRBox.Text = string.Format("{0:0.00}", psnr);
                }
                else
                {
                    PSNRBox.Text = "+Inf";
                }
            }
        }

        private double PSNR(Bitmap orig, Bitmap modif)
        {
            double mse = Math.Sqrt(MSE(orig, modif));
            return 20 * Math.Log10(255 / mse);
        }

        private double MSE(Bitmap orig, Bitmap modif)
        {
            double result = 0;
            for (int i = 0; i < orig.Width; i++)
            {
                for (int j = 0; j < orig.Height; j++)
                {
                    Color color = orig.GetPixel(i, j);
                    int R = color.R;
                    int G = color.G;
                    int B = color.B;
                    color = modif.GetPixel(i, j);
                    int r = color.R;
                    int g = color.G;
                    int b = color.B;
                    result += Math.Pow((R - r), 2) + Math.Pow((G - g), 2) + Math.Pow((B - b), 2);
                }
            }
            int size = orig.Height * orig.Width;
            return result / (3.0 * size);
        }
#endregion: Подсчет PSNR
#region: Преобразование YUV
        private int y(int R, int G, int B) {
            return (int)(0.299 * R + 0.587 * G + 0.114 * B);
        }
        private int u(int R, int G, int B) {
            return (int)(-0.14713 * R - 0.28886 * G + 0.436 * B);
        }
        private int v(int R, int G, int B) {
            return (int)(0.615 * R - 0.515 * G - 0.100 * B);
        }
        private int r(int Y, int U, int V) {
            return (int)(Y + 1.140 * V);
        }
        private int g(int Y, int U, int V) {
            return (int)(Y - 0.394 * U - 0.581 * V);
        }
        private int b(int Y, int U, int V) {
            return (int)(Y + 2.032 * U);
        }
        #endregion: Преобразование YUV
#region: Перевод YUV и обратно
        private void YUV_Click(object sender, EventArgs e)
        {
            Bitmap src = (Bitmap)OriginalImageBox.Image;
            Bitmap res = toYUVtoRGB(src, (int)YTrack.Value, (int)UTrack.Value, (int)VTrack.Value);
            ModifiedImageBox.Image = res;
        }

        private Bitmap toYUVtoRGB(Bitmap src, int Ybits, int Ubits, int Vbits)
        {
            if (src != null)
            {
                Bitmap res = new Bitmap(src.Width, src.Height);
                for (int i = 0; i < src.Height; i++)
                {
                    for (int j = 0; j < src.Width; j++)
                    {
                        Color color = src.GetPixel(i, j);
                        int R = color.R;
                        int G = color.G;
                        int B = color.B;
                        int Y = reduceBits(y(R, G, B), Ybits, 8);
                        int U = reduceBits(u(R, G, B), Ubits, 8);
                        int V = reduceBits(v(R, G, B), Vbits, 8);
                        R = toCorrectRange(increaseBits(r(Y, U, V), Ybits, 8));
                        G = toCorrectRange(increaseBits(g(Y, U, V), Ubits, 8));
                        B = toCorrectRange(increaseBits(b(Y, U, V), Vbits, 8));
                        color = Color.FromArgb(R, G, B);
                        res.SetPixel(i, j, color);
                    }
                }
                return res;
            }
            return null;
        }

        private int reduceBits(int value, int bits, int sourceBits)
        {
            return (int) Math.Round(value / Math.Pow(2, sourceBits - bits));
        }

        private int increaseBits(int value, int bits, int sourceBits)
        {
            if (bits == 0)
            {
                return 0;
            }
            value = (int)(value * Math.Pow(2, sourceBits - bits));
            if (bits != 8)
            {
                value = (int)(value + Math.Pow(2, sourceBits - bits - 1));
            }
            return value;
        }

        private int toCorrectRange(int value)
        {
            value = value < 0 ? 0 : value;
            value = value > 255 ? 255 : value;
            return value;
        }
#endregion: Перевод YUV и обратно
#region: Отдельный вывод каналов Y, U, V
        private void YButton_Click(object sender, EventArgs e)
        {
            Bitmap src = (Bitmap)OriginalImageBox.Image;
            Bitmap res = toYChannel(src, (int)YTrack.Value);
            ModifiedImageBox.Image = res;
        }

        private void UButton_Click(object sender, EventArgs e)
        {
            Bitmap src = (Bitmap)OriginalImageBox.Image;
            Bitmap res = toUChannel(src, (int)UTrack.Value);
            ModifiedImageBox.Image = res;
        }

        private void VButton_Click(object sender, EventArgs e)
        {
            Bitmap src = (Bitmap)OriginalImageBox.Image;
            Bitmap res = toVChannel(src, (int)VTrack.Value);
            ModifiedImageBox.Image = res;
        }

        private Bitmap toYChannel(Bitmap src, int Ybits)
        {
            if (src != null)
            {
                Bitmap res = new Bitmap(src.Width, src.Height);
                for (int i = 0; i < src.Height; i++)
                {
                    for (int j = 0; j < src.Width; j++)
                    {
                        Color color = src.GetPixel(i, j);
                        int R = color.R;
                        int G = color.G;
                        int B = color.B;
                        int Y = toCorrectRange(increaseBits(reduceBits(y(R, G, B), Ybits, 8), Ybits, 8));
                        color = Color.FromArgb(Y, Y, Y);
                        res.SetPixel(i, j, color);
                    }
                }
                return res;
            }
            return null;
        }

        private Bitmap toUChannel(Bitmap src, int Ubits)
        {
            if (src != null)
            {
                Bitmap res = new Bitmap(src.Width, src.Height);
                for (int i = 0; i < src.Height; i++)
                {
                    for (int j = 0; j < src.Width; j++)
                    {
                        Color color = src.GetPixel(i, j);
                        int R = color.R;
                        int G = color.G;
                        int B = color.B;
                        int U = toCorrectRange(increaseBits(reduceBits(u(R, G, B), Ubits, 8), Ubits, 8));
                        color = Color.FromArgb(U, U, U);
                        res.SetPixel(i, j, color);
                    }
                }
                return res;
            }
            return null;
        }

        private Bitmap toVChannel(Bitmap src, int Vbits)
        {
            if (src != null)
            {
                Bitmap res = new Bitmap(src.Width, src.Height);
                for (int i = 0; i < src.Height; i++)
                {
                    for (int j = 0; j < src.Width; j++)
                    {
                        Color color = src.GetPixel(i, j);
                        int R = color.R;
                        int G = color.G;
                        int B = color.B;
                        int V = toCorrectRange(increaseBits(reduceBits(v(R, G, B), Vbits, 8), Vbits, 8));
                        color = Color.FromArgb(V, V, V);
                        res.SetPixel(i, j, color);
                    }
                }
                return res;
            }
            return null;
        }
#endregion: Отдельный вывод каналов Y, U, V
#region: Преобразование YCrCb
        private int cr(int R, int G, int B)
        {
            return (int)(0.5 * R - 0.4187 * G - 0.0813 * B + 128);
        }
        private int cb(int R, int G, int B)
        {
            return (int)(-0.1687 * R - 0.3313 * G + 0.5 * B + 128);
        }
        private int r1(int Y, int Cr, int Cb)
        {
            return (int)(Y + 1.402 * (Cr - 128));
        }
        private int g1(int Y, int Cr, int Cb)
        {
            return (int)(Y - 0.34414 * (Cb - 128) - 0.71414 * (Cr - 128));
        }
        private int b1(int Y, int Cr, int Cb)
        {
            return (int)(Y + 1.772 * (Cb - 128));
        }
        #endregion: Преобразование YCrCb
#region: Перевод YCrCb и обратно
        private void YCrCbButton_Click(object sender, EventArgs e)
        {
            Bitmap src = (Bitmap)OriginalImageBox.Image;
            Bitmap res = toYCrCbtoRGB(src, (int)YTrack1.Value, (int)CrTrack.Value, (int)CbTrack.Value);
            ModifiedImageBox.Image = res;
        }

        private Bitmap toYCrCbtoRGB(Bitmap src, int Ybits, int Crbits, int Cbbits)
        {
            if (src != null)
            {
                Bitmap res = new Bitmap(src.Width, src.Height);
                String s = null;
                for (int i = 0; i < src.Height; i++)
                {
                    for (int j = 0; j < src.Width; j++)
                    {
                        Color color = src.GetPixel(i, j);
                        int R = color.R;
                        int G = color.G;
                        int B = color.B;
                        //int Y = y(R, G, B);
                        //int Cr = cr(R, G, B);
                        //int Cb = cb(R, G, B);
                        int Y = reduceBits(y(R, G, B), Ybits, 8);
                        int Cr = reduceBits(cr(R, G ,B), Crbits, 8);
                        int Cb = reduceBits(cb(R, G, B), Cbbits, 8);
                        s =  "" + Y + " " + Cr + " " + Cb;
                        //R = toCorrectRange(r1(Y, Cr, Cb));
                        //G = toCorrectRange(g1(Y, Cr, Cb));
                        //B = toCorrectRange(b1(Y, Cr, Cb));
                        R = toCorrectRange(increaseBits(r1(Y, Cr, Cb), Ybits, 8));
                        G = toCorrectRange(increaseBits(g1(Y, Cr, Cb), Crbits, 8));
                        B = toCorrectRange(increaseBits(b1(Y, Cr, Cb), Cbbits, 8));
                        color = Color.FromArgb(R, G, B);
                        res.SetPixel(i, j, color);
                    }
                }
                return res;
            }
            return null;
        }
        #endregion: Перевод YCrCb и обратно
#region: Отдельный вывод каналов Y, Cr, Cb
        private void YButton1_Click(object sender, EventArgs e)
        {
            Bitmap src = (Bitmap)OriginalImageBox.Image;
            Bitmap res = toYChannel1(src, (int)YTrack1.Value);
            ModifiedImageBox.Image = res;
        }

        private void CrButton_Click(object sender, EventArgs e)
        {
            Bitmap src = (Bitmap)OriginalImageBox.Image;
            Bitmap res = toCrChannel(src, (int)CrTrack.Value);
            ModifiedImageBox.Image = res;
        }

        private void CbButton_Click(object sender, EventArgs e)
        {
            Bitmap src = (Bitmap)OriginalImageBox.Image;
            Bitmap res = toCbChannel(src, (int)CbTrack.Value);
            ModifiedImageBox.Image = res;
        }

        private Bitmap toYChannel1(Bitmap src, int Ybits)
        {
            if (src != null)
            {
                Bitmap res = new Bitmap(src.Width, src.Height);
                for (int i = 0; i < src.Height; i++)
                {
                    for (int j = 0; j < src.Width; j++)
                    {
                        Color color = src.GetPixel(i, j);
                        int R = color.R;
                        int G = color.G;
                        int B = color.B;
                        int Y = toCorrectRange(increaseBits(reduceBits(y(R, G, B), Ybits, 8), Ybits, 8));
                        color = Color.FromArgb(Y, Y, Y);
                        res.SetPixel(i, j, color);
                    }
                }
                return res;
            }
            return null;
        }

        private Bitmap toCrChannel(Bitmap src, int Crbits)
        {
            if (src != null)
            {
                Bitmap res = new Bitmap(src.Width, src.Height);
                for (int i = 0; i < src.Height; i++)
                {
                    for (int j = 0; j < src.Width; j++)
                    {
                        Color color = src.GetPixel(i, j);
                        int R = color.R;
                        int G = color.G;
                        int B = color.B;
                        int Cr = toCorrectRange(increaseBits(reduceBits(cr(R, G, B), Crbits, 8), Crbits, 8));
                        color = Color.FromArgb(Cr, Cr, Cr);
                        res.SetPixel(i, j, color);
                    }
                }
                return res;
            }
            return null;
        }

        private Bitmap toCbChannel(Bitmap src, int Cbbits)
        {
            if (src != null)
            {
                Bitmap res = new Bitmap(src.Width, src.Height);
                for (int i = 0; i < src.Height; i++)
                {
                    for (int j = 0; j < src.Width; j++)
                    {
                        Color color = src.GetPixel(i, j);
                        int R = color.R;
                        int G = color.G;
                        int B = color.B;
                        int Cb = toCorrectRange(increaseBits(reduceBits(cb(R, G, B), Cbbits, 8), Cbbits, 8));
                        color = Color.FromArgb(Cb, Cb, Cb);
                        res.SetPixel(i, j, color);
                    }
                }
                return res;
            }
            return null;
        }
        #endregion: Отдельный вывод каналов Y, Cr, Cb



#region: Прореживание
        private void DecimateButton_Click(object sender, EventArgs e)
        {
            Bitmap src = (Bitmap)OriginalImageBox.Image;
            if (DecimateOptionBox.Text != null)
            {
                Bitmap res = Decimate(src, DecimateOptionBox.Text);
                ModifiedImageBox.Image = res;
            }
        }

        private Bitmap Decimate(Bitmap src, string p)
        {
            if (src != null && p != null)
            {
                Bitmap res = new Bitmap(src.Width, src.Height);
                switch (p)
                {
                    case "4:4:4 (no decimation)":
                        {
                            return src;
                        }
                    case "4:2:2 (2h1v)":
                        {
                            for (int i = 0; i < res.Height; i++)
                            {
                                for (int j = 0; j < res.Width; j = j + 2)
                                {
                                    Color color = src.GetPixel(i, j);
                                    res.SetPixel(i, j, color);
                                    Color c = src.GetPixel(i, j + 1);
                                    color = decimatedCrCb(color, c);
                                    res.SetPixel(i, j + 1, color);
                                }
                            }
                            return res;
                        }
                    // первый пиксель
                    case "4:1:1 (2h2v)":
                        {
                            for (int i = 0; i < res.Height; i++)
                            {
                                for (int j = 0; j < res.Width; j = j + 2)
                                {
                                    if ((i % 2) == 0)
                                    {
                                        Color color = src.GetPixel(i, j);
                                        res.SetPixel(i, j, color);
                                        Color c = src.GetPixel(i, j + 1);
                                        color = decimatedCrCb(color, c);
                                        res.SetPixel(i, j + 1, color);
                                    }
                                    else
                                    {
                                        Color color = src.GetPixel(i - 1, j);
                                        Color c = src.GetPixel(i, j);
                                        color = decimatedCrCb(color, c);
                                        res.SetPixel(i, j, color);
                                        color = src.GetPixel(i - 1, j);
                                        c = src.GetPixel(i, j);
                                        color = decimatedCrCb(color, c);
                                        res.SetPixel(i, j + 1, color);
                                    }
                                }
                            }
                            return res;
                        }
                    case "4:2:2 (1h2v)":
                        {
                            for (int i = 0; i < res.Height; i++)
                            {
                                for (int j = 0; j < res.Width; j = j + 2)
                                {
                                    if ((i % 2) == 0)
                                    {
                                        Color color = src.GetPixel(i, j);
                                        res.SetPixel(i, j, color);
                                        color = src.GetPixel(i, j + 1);
                                        res.SetPixel(i, j + 1, color);
                                    }
                                    else
                                    {
                                        Color color = src.GetPixel(i - 1, j);
                                        Color c = src.GetPixel(i, j);
                                        color = decimatedCrCb(color, c);
                                        res.SetPixel(i, j, color);
                                        color = src.GetPixel(i - 1, j + 1);
                                        c = src.GetPixel(i, j + 1);
                                        color = decimatedCrCb(color, c);
                                        res.SetPixel(i, j + 1, color);
                                    }
                                }
                            }
                            return res;
                        }
                }
            }
            return null;
        }

        private Color decimatedCrCb(Color sourceColor, Color decimatedColor)
        {
            int R = sourceColor.R;
            int G = sourceColor.G;
            int B = sourceColor.B;
            int Y = y(R, G, B);
            int Cr = cr(R, G, B);
            int Cb = cb(R, G, B);
            R = decimatedColor.R;
            G = decimatedColor.G;
            B = decimatedColor.B;
            int Y1 = y(R, G, B);
            R = toCorrectRange(r1(Y1, Cr, Cb));
            G = toCorrectRange(g1(Y1, Cr, Cb));
            B = toCorrectRange(b1(Y1, Cr, Cb));
            return Color.FromArgb(R, G, B);
        }


#endregion: Прореживание
#region: DCT
        private void DCTButton_Click(object sender, EventArgs e)
        {
            int blockSize = 8;
            Bitmap src = (Bitmap)OriginalImageBox.Image;
            double[,] M = buildM(blockSize);
            double[,] Mt = transpose(M, blockSize);
            string quantizeMethod = QuantizeMethodBox.Text;
            Bitmap res = DCT(src, M, Mt, blockSize, quantizeMethod);
            ModifiedImageBox.Image = res;
        }

        private double[,] buildM(int blockSize)
        {
            double[,] M = new double[blockSize, blockSize];
            for (int j = 0; j < blockSize; j++)
            {
                M[0, j] = 1.0 / Math.Sqrt(8);
            }
            for (int i = 1; i < blockSize; i++)
            {
                for (int j = 0; j < blockSize; j++)
                {
                    M[i, j] = 0.5 * Math.Cos(((2.0 * j + 1.0) * i * Math.PI) / 16.0);
                }
            }
            return M;
        }
        private double[,] transpose(double[,] M, int size)
        {
            double[,] Mt = new double[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    Mt[i, j] = M[j, i];
                }
            }
            return Mt;
        }
        
        private Bitmap DCT(Bitmap src, double[,] M, double[,] Mt, int blockSize, string quantizeMethod)
        {
            Bitmap res = new Bitmap(src.Width, src.Height);
            var blocks = ListBlocks(src, blockSize).ToArray();
            int nextBlock = 0;
            YCrCbImage image = new YCrCbImage(src.Width, src.Height);
            for (int i = 0; i < image.Height; i += blockSize)
            {
                for (int j = 0; j < image.Width; j += blockSize)
                {
                    YCrCbImage D = blocks[nextBlock];
                    nextBlock++;
                    YCrCbImage C = Multiply(M, D, Mt);
                    switch (quantizeMethod)
                    {
                        case "N":
                            {
                                C = Quantize(C, Convert.ToInt32(NYBox.Text), Convert.ToInt32(NCrCbBox.Text));
                            }
                            break;
                        case "Q":
                            {
                                C = Quantize(C, QuantizeMatrix( Convert.ToInt32(AlphaBox.Text),  Convert.ToInt32(GammaBox.Text), blockSize));
                            }
                            break;
                        case "JPEG":
                            {
                                C = Quantize(C, QuantizeMatrix("Y"), QuantizeMatrix("CrCb"));
                            }
                            break;
                    }
                    C = Multiply(Mt, C, M);
                    for (int k = 0; k < blockSize; k++)
                    {
                        for (int l = 0; l < blockSize; l++)
                        {
                            YCrCbPixel pixel = C.GetPixel(k, l);
                            image.SetPixel(i + k, j + l, pixel);
                        }
                    }
                }
            }
            res = toRGB(image);
            return res;
        }

        private YCrCbImage Quantize(YCrCbImage C, int NY, int NCrCb)
        {
            throw new NotImplementedException();
        }

        private YCrCbImage Quantize(YCrCbImage C, int[,] Q)
        {
            YCrCbImage quantizedImage = new YCrCbImage(C.Width, C.Height);
            for (int i = 0; i < C.Height; i++)
            {
                for (int j = 0; j < C.Width; j++)
                {
                    YCrCbPixel pixel = C.GetPixel(i, j);
                    int Yquantized = (pixel.Y / Q[i, j]);
                    int Crquantized = (pixel.Cr / Q[i, j]);
                    int Cbquantized = (pixel.Cb / Q[i, j]);
                    Yquantized *= Q[i, j];
                    Crquantized *= Q[i, j];
                    Cbquantized *= Q[i, j];
                    pixel = new YCrCbPixel(Yquantized, Crquantized, Cbquantized);
                    quantizedImage.SetPixel(i, j, pixel);
                }
            }
            return quantizedImage;
        }

        private YCrCbImage Quantize(YCrCbImage C, int[,] Y, int[,] CrCb)
        {
            YCrCbImage quantizedImage = new YCrCbImage(C.Width, C.Height);
            for (int i = 0; i < C.Height; i++)
            {
                for (int j = 0; j < C.Width; j++)
                {
                    YCrCbPixel pixel = C.GetPixel(i, j);
                    int Yquantized = (pixel.Y / Y[i, j]);
                    int Crquantized = (pixel.Cr / CrCb[i, j]);
                    int Cbquantized = (pixel.Cb / CrCb[i, j]);
                    Yquantized *= Y[i, j];
                    Crquantized *= CrCb[i, j];
                    Cbquantized *= CrCb[i, j];
                    pixel = new YCrCbPixel(Yquantized, Crquantized, Cbquantized);
                    quantizedImage.SetPixel(i, j, pixel);
                }
            }
            return quantizedImage;
        }

        private int[,] QuantizeMatrix(int alpha, int gamma, int blockSize)
        {
            int[,] Q = new int[blockSize, blockSize];
            for (int i = 0; i < blockSize; i++)
            {
                for (int j = 0; j < blockSize; j++)
                {
                    Q[i,j] = alpha * (1 + gamma * ( i + j + 2));
                }
            }
            return Q;
        }

        private int[,] QuantizeMatrix(string channelName)
        {
            switch (channelName) 
            {
                case "Y":
                    {
                        int[,] Y = {{16, 11, 10, 16, 24, 40, 51, 61},
                           {12, 12, 14, 19, 26, 58, 60, 55},
                           {14, 13, 16, 24, 40, 57, 69, 56},
                           {14, 17, 22, 29, 51, 87, 80, 62},
                           {18, 22, 37, 56, 68, 109, 103, 77},
                           {24, 35, 55, 64, 81, 104, 113, 92},
                           {49, 64, 78, 87, 103, 121, 120, 101},
                           {72, 92, 95, 98, 112, 100, 103, 99}
                          };
                        return Y;
                    }
                case "CrCb":
                    {
                        int[,] CrCb = {{17, 18, 24, 47, 99, 99, 99, 99},
                              {18, 21, 26, 66, 99, 99, 99, 99},
                              {24, 26, 56, 99, 99, 99, 99, 99},
                              {47, 66, 99, 99, 99, 99, 99, 99},
                              {99, 99, 99, 99, 99, 99, 99, 99},
                              {99, 99, 99, 99, 99, 99, 99, 99},
                              {99, 99, 99, 99, 99, 99, 99, 99},
                              {99, 99, 99, 99, 99, 99, 99, 99}
                             };
                        return CrCb;
                    }
            }
            return null;
        }

        private YCrCbImage Multiply(double[,] M, YCrCbImage D, double[,] Mt)
        {
            YCrCbImage C = new YCrCbImage(D.Width, D.Height);
            YCrCbImage res = new YCrCbImage(D.Width, D.Height);
            for (int i = 0; i < C.Height; i++)
            {
                for (int j = 0; j < C.Width; j++)
                {
                    int Ytransformed = 0;
                    int Crtransformed = 0;
                    int Cbtransformed = 0;
                    for (int k = 0; k < C.Width; k++)
                    {
                        YCrCbPixel pixel = D.GetPixel(k, j);
                        Ytransformed += (int) (M[i, k] * pixel.Y);
                        Crtransformed += (int) (M[i, k] * pixel.Cr);
                        Cbtransformed += (int) (M[i, k] * pixel.Cb);
                    }
                    YCrCbPixel pixelTransformed = new YCrCbPixel(Ytransformed, Crtransformed, Cbtransformed);
                    C.SetPixel(i, j, pixelTransformed);
                }
            }
            for (int i = 0; i < C.Height; i++)
            {
                for (int j = 0; j < C.Width; j++)
                {
                    int Ytransformed = 0;
                    int Crtransformed = 0;
                    int Cbtransformed = 0;
                    for (int k = 0; k < C.Width; k++)
                    {
                        YCrCbPixel pixel = C.GetPixel(i, k);
                        Ytransformed += (int) (pixel.Y * Mt[k, j]);
                        Crtransformed += (int) (pixel.Cr * Mt[k, j]);
                        Cbtransformed += (int) (pixel.Cb * Mt[k, j]);
                    }
                    YCrCbPixel pixelTransformed = new YCrCbPixel(Ytransformed, Crtransformed, Cbtransformed);
                    res.SetPixel(i, j, pixelTransformed);
                }
            }
            return res;
        }

        private IEnumerable<YCrCbImage> ListBlocks(Bitmap src, int blockSize)
        {
            YCrCbImage image = toYCrCb(src);
            for (int i = 0; i < image.Height; i += blockSize)
            {
                for (int j = 0; j < image.Width; j += blockSize)
                {
                    YCrCbImage block = new YCrCbImage(blockSize, blockSize);
                    for (int k = 0; k < blockSize; k++)
                    {
                        for (int l = 0; l < blockSize; l++)
                        {
                            YCrCbPixel pixel = image.GetPixel(i + k, j + l);
                            block.SetPixel(k, l, pixel);
                        }
                    }
                    yield return block;
                }
            }
        }

        private YCrCbImage toYCrCb(Bitmap src)
        {
            YCrCbImage res = new YCrCbImage(src.Width, src.Height);
            for (int i = 0; i < src.Height; i++)
            {
                for (int j = 0; j < src.Width; j++)
                {
                    Color color = src.GetPixel(i, j);
                    int R = color.R;
                    int G = color.G;
                    int B = color.B;
                    int Y = y(R, G, B);
                    int Cr = cr(R, G, B);
                    int Cb = cb(R, G, B);
                    YCrCbPixel pixel = new YCrCbPixel(Y, Cr, Cb);
                    res.SetPixel(i, j, pixel);
                }
            }
            return res;
        }

        private Bitmap toRGB(YCrCbImage image)
        {
            Bitmap res = new Bitmap(image.Width, image.Height);
            for (int i = 0; i < res.Height; i++)
            {
                for (int j = 0; j < res.Width; j++)
                {
                    YCrCbPixel pixel = image.GetPixel(i, j);
                    int R = toCorrectRange(r1(pixel.Y, pixel.Cr, pixel.Cb));
                    int G = toCorrectRange(g1(pixel.Y, pixel.Cr, pixel.Cb));
                    int B = toCorrectRange(b1(pixel.Y, pixel.Cr, pixel.Cb));
                    Color color = Color.FromArgb(R, G, B);
                    res.SetPixel(i, j, color);
                }
            }
            return res;
        }
        
#endregion: DCT

    }
}