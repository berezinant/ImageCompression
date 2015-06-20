using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

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


#region: Вещественное преобразование YCrCb
        private double _y(int R, int G, int B)
        {
            return (0.299 * R + 0.587 * G + 0.114 * B);
        }
        private double _cr(int R, int G, int B)
        {
            return (0.5 * R - 0.4187 * G - 0.0813 * B + 128);
        }
        private double _cb(int R, int G, int B)
        {
            return (-0.1687 * R - 0.3313 * G + 0.5 * B + 128);
        }
        private int _r1(double Y, double Cr, double Cb)
        {
            return (int)(Y + 1.402 * (Cr - 128));
        }
        private int _g1(double Y, double Cr, double Cb)
        {
            return (int)(Y - 0.34414 * (Cb - 128) - 0.71414 * (Cr - 128));
        }
        private int _b1(double Y, double Cr, double Cb)
        {
            return (int)(Y + 1.772 * (Cb - 128));
        }
#endregion: Вещественное Преобразование YCrCb
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
#region: Кодирование(JPEG) и раскодирование
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
        private void CompressButton_Click(object sender, EventArgs e)
        {
            int blockSize = 8;
            Bitmap src = (Bitmap)OriginalImageBox.Image;
            double[,] M = buildM(blockSize);
            string quantizeMethod = QuantizeMethodBox.Text;
            string decimateMethod = DecimateOptionBox.Text;
            EncodedImage encoded = Encode(src, M, blockSize, quantizeMethod, decimateMethod);
            SaveImage(encoded, String.Format("{0}.txt", FileNameBox.Text));
            CompressFileLZMA(String.Format("{0}.txt", FileNameBox.Text), String.Format("{0}.7z", FileNameBox.Text));
            DecompressFileLZMA(String.Format("{0}.7z", FileNameBox.Text), String.Format("{0}1.txt", FileNameBox.Text));
            LoadImage(ref encoded, String.Format("{0}1.txt", FileNameBox.Text));
            Bitmap res = Decode(encoded, M, blockSize);
            ModifiedImageBox.Image = res;
        }

        private void LoadImage(ref EncodedImage encoded, string fileName)
        {
            FileStream fs = new FileStream(fileName, FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                encoded = (EncodedImage) formatter.Deserialize(fs);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to deserialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        private void SaveImage(EncodedImage encoded, string fileName)
        {

            FileStream fs = new FileStream(fileName, FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, encoded);
            }
            catch (SerializationException e)
            {
                Console.WriteLine("Failed to serialize. Reason: " + e.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        private static void CompressFileLZMA(string inFile, string outFile)
        {
            SevenZip.Compression.LZMA.Encoder coder = new SevenZip.Compression.LZMA.Encoder();
            FileStream input = new FileStream(inFile, FileMode.Open);
            FileStream output = new FileStream(outFile, FileMode.Create);

            // Write the encoder properties
            coder.WriteCoderProperties(output);

            // Write the decompressed file size.
            output.Write(BitConverter.GetBytes(input.Length), 0, 8);

            // Encode the file.
            coder.Code(input, output, input.Length, -1, null);
            output.Flush();
            output.Close();
        }

        private static void DecompressFileLZMA(string inFile, string outFile)
        {
            SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder();
            FileStream input = new FileStream(inFile, FileMode.Open);
            FileStream output = new FileStream(outFile, FileMode.Create);

            // Read the decoder properties
            byte[] properties = new byte[5];
            input.Read(properties, 0, 5);

            // Read in the decompress file size.
            byte[] fileLengthBytes = new byte[8];
            input.Read(fileLengthBytes, 0, 8);
            long fileLength = BitConverter.ToInt64(fileLengthBytes, 0);

            coder.SetDecoderProperties(properties);
            coder.Code(input, output, input.Length, fileLength, null);
            output.Flush();
            output.Close();
        }

        private Bitmap Decode(EncodedImage encoded, double[,] M, int blockSize)
        {
            double[,] Mt = transpose(M, blockSize);
            double[,] Y = new double[encoded.YHeight, encoded.YWidth];
            double[,] Cr = new double[encoded.CrCbHeight, encoded.CrCbWidth];
            double[,] Cb = new double[encoded.CrCbHeight, encoded.CrCbWidth];
            encoded.ResetCounters();
            for (int i = 0; i < encoded.YHeight; i += blockSize)
            {
                for (int j = 0; j < encoded.YWidth; j += blockSize)
                {
                    double[,] YBlockTransformed = encoded.ExtractNextBlock(Channel.Y, blockSize);
                    double[,] YBlock = Multiply(Mt, YBlockTransformed, M, blockSize);
                    for (int k = 0; k < blockSize; k++)
                    {
                        for (int l = 0; l < blockSize; l++)
                        {
                            Y[i + k, j + l] = YBlock[k, l];
                        }
                    }
                }
            }
            for (int i = 0; i < encoded.CrCbHeight; i += blockSize)
            {
                for (int j = 0; j < encoded.CrCbWidth; j += blockSize)
                {
                    double[,] CrBlockTransformed = encoded.ExtractNextBlock(Channel.Cr, blockSize);
                    double[,] CbBlockTransformed = encoded.ExtractNextBlock(Channel.Cb, blockSize);
                    double[,] CrBlock = Multiply(Mt, CrBlockTransformed, M, blockSize);
                    double[,] CbBlock = Multiply(Mt, CbBlockTransformed, M, blockSize);
                    for (int k = 0; k < blockSize; k++)
                    {
                        for (int l = 0; l < blockSize; l++)
                        {
                            Cr[i + k, j + l] = CrBlock[k, l];
                            Cb[i + k, j + l] = CbBlock[k, l];
                        }
                    }
                }
            }
            Recover(encoded.decimateMethod, ref Cr, ref Cb, encoded.CrCbWidth, encoded.CrCbHeight);
            Bitmap res;
            ToRGB(out res, Y, Cr, Cb, encoded.YHeight);
            return res;
        }
       
        private EncodedImage Encode(Bitmap src, double[,] M, int blockSize, string quantizeMethod, string decimateMethod)
        {
            double[,] Mt = transpose(M, blockSize);
            int sourceSize = src.Width;
            double[,] Y;
            double[,] Cr;
            double[,] Cb;
            int YWidth = sourceSize;
            int YHeight = sourceSize;
            int CrCbWidth;
            int CrCbHeight;
            ToYCrCb(src, out Y, out Cr, out Cb);

            Decimate(decimateMethod, sourceSize, ref Cr, ref Cb, out CrCbWidth, out CrCbHeight);
            EncodedImage encodedImage = new EncodedImage(decimateMethod, YWidth, YHeight, CrCbWidth, CrCbHeight);

            for (int i = 0; i < YHeight; i += blockSize)
            {
                for (int j = 0; j < YWidth; j += blockSize)
                {
                    double[,] YBlock = new double[blockSize, blockSize];
                    for (int k = 0; k < blockSize; k++)
                    {
                        for (int l = 0; l < blockSize; l++)
                        {
                            YBlock[k, l] = Y[i + k, j + l];
                        }
                    }
                    double[,] YBlockTransformed = Multiply(M, YBlock, Mt, blockSize);
                    Quantize(quantizeMethod, Channel.Y, ref YBlockTransformed, blockSize);
                    encodedImage.appendZigzagedBlock(YBlockTransformed, blockSize, Channel.Y);
                }
            }
            for (int i = 0; i < CrCbHeight; i += blockSize)
            {
                for (int j = 0; j < CrCbWidth; j += blockSize)
                {
                    double[,] CrBlock = new double[blockSize, blockSize];
                    double[,] CbBlock = new double[blockSize, blockSize];
                    for (int k = 0; k < blockSize; k++)
                    {
                        for (int l = 0; l < blockSize; l++)
                        {
                            CrBlock[k, l] = Cr[i + k, j + l];
                            CbBlock[k, l] = Cb[i + k, j + l];
                        }
                    }
                    double[,] CrBlockTransformed = Multiply(M, CrBlock, Mt, blockSize);
                    Quantize(quantizeMethod, Channel.Cr, ref CrBlockTransformed, blockSize);
                    encodedImage.appendZigzagedBlock(CrBlockTransformed, blockSize, Channel.Cr);
                    double[,] CbBlockTransformed = Multiply(M, CbBlock, Mt, blockSize);
                    Quantize(quantizeMethod, Channel.Cb, ref CbBlockTransformed, blockSize);
                    encodedImage.appendZigzagedBlock(CbBlockTransformed, blockSize, Channel.Cb);
                }
            }
            return encodedImage;
        }

        private void Recover(string decimateMethod, ref double[,] Cr, ref double[,] Cb, int CrCbWidth, int CrCbHeight)
        {
            switch (decimateMethod)
            {
                case "":
                    return;
                case "4:4:4 (no decimation)":
                    return;
                case "4:2:2 (2h1v)":
                    {
                        double[,] CrRecovered = new double[CrCbHeight, CrCbWidth * 2];
                        double[,] CbRecovered = new double[CrCbHeight, CrCbWidth * 2];
                        for (int i = 0; i < CrCbHeight; i++)
                        {
                            for (int j = 0; j < CrCbWidth; j++)
                            {
                                CrRecovered[i, j * 2] = Cr[i, j];
                                CrRecovered[i, j * 2 + 1] = Cr[i, j];
                                CbRecovered[i, j * 2] = Cb[i, j];
                                CbRecovered[i, j * 2 + 1] = Cb[i, j];
                            }
                        }
                        Cr = CrRecovered;
                        Cb = CbRecovered;
                    }
                    break;
                case "4:1:1 (2h2v)":
                    {
                        double[,] CrRecovered = new double[CrCbHeight * 2, CrCbWidth * 2];
                        double[,] CbRecovered = new double[CrCbHeight * 2, CrCbWidth * 2];
                        for (int i = 0; i < CrCbHeight; i++)
                        {
                            for (int j = 0; j < CrCbWidth; j++)
                            {
                                CrRecovered[i * 2, j * 2] = Cr[i, j];
                                CrRecovered[i * 2, j * 2 + 1] = Cr[i, j];
                                CrRecovered[i * 2 + 1, j * 2] = Cr[i, j];
                                CrRecovered[i * 2 + 1, j * 2 + 1] = Cr[i, j];
                                CbRecovered[i * 2, j * 2] = Cb[i, j];
                                CbRecovered[i * 2, j * 2 + 1] = Cb[i, j];
                                CbRecovered[i * 2 + 1, j * 2] = Cb[i, j];
                                CbRecovered[i * 2 + 1, j * 2 + 1] = Cb[i, j];
                            }
                        }
                        Cr = CrRecovered;
                        Cb = CbRecovered;
                    }
                    break;
                case "4:2:2 (1h2v)":
                    {
                        double[,] CrRecovered = new double[CrCbHeight * 2, CrCbWidth];
                        double[,] CbRecovered = new double[CrCbHeight * 2, CrCbWidth];
                        for (int i = 0; i < CrCbHeight; i++)
                        {
                            for (int j = 0; j < CrCbWidth; j++)
                            {
                                CrRecovered[i * 2, j] = Cr[i, j];
                                CrRecovered[i * 2 + 1, j] = Cr[i, j];
                                CbRecovered[i * 2, j] = Cb[i, j];
                                CbRecovered[i * 2 + 1, j] = Cb[i, j];
                            }
                        }
                        Cr = CrRecovered;
                        Cb = CbRecovered;
                    }
                    break;
            }
        }

        private void Decimate(string decimateMethod, int size, ref double[,] Cr, ref double[,] Cb, out int CrCbWidth, out int CrCbHeight)
        {
            CrCbWidth = size;
            CrCbHeight = size;
            switch (decimateMethod)
            {
                case "":
                    return;
                case "4:4:4 (no decimation)":
                    return;
                case "4:2:2 (2h1v)":
                    {
                        CrCbWidth = size / 2;
                        CrCbHeight = size;
                        double[,] CrDecimated = new double[CrCbHeight, CrCbWidth];
                        double[,] CbDecimated = new double[CrCbHeight, CrCbWidth];
                        for (int i = 0; i < CrCbHeight; i++)
                        {
                            for (int j = 0; j < CrCbWidth; j++)
                            {
                                CrDecimated[i, j] = Cr[i, j * 2];
                                CbDecimated[i, j] = Cb[i, j * 2];
                            }
                        }
                        Cr = CrDecimated;
                        Cb = CbDecimated;
                    }
                    break;
                case "4:1:1 (2h2v)":
                    {
                        CrCbWidth = size / 2;
                        CrCbHeight = size / 2;
                        double[,] CrDecimated = new double[CrCbHeight, CrCbWidth];
                        double[,] CbDecimated = new double[CrCbHeight, CrCbWidth];
                        for (int i = 0; i < CrCbHeight; i++)
                        {
                            for (int j = 0; j < CrCbWidth; j++)
                            {
                                CrDecimated[i, j] = Cr[i * 2, j * 2];
                                CbDecimated[i, j] = Cb[i * 2, j * 2];
                            }
                        }
                        Cr = CrDecimated;
                        Cb = CbDecimated;
                    }
                    break;
                case "4:2:2 (1h2v)":
                    {
                        CrCbWidth = size;
                        CrCbHeight = size / 2;
                        double[,] CrDecimated = new double[CrCbHeight, CrCbWidth];
                        double[,] CbDecimated = new double[CrCbHeight, CrCbWidth];
                        for (int i = 0; i < CrCbHeight; i++)
                        {
                            for (int j = 0; j < CrCbWidth; j++)
                            {
                                CrDecimated[i, j] = Cr[i * 2, j];
                                CbDecimated[i, j] = Cb[i * 2, j];
                            }
                        }
                        Cr = CrDecimated;
                        Cb = CbDecimated;
                    }
                    break;
            }
        }

        private void Quantize(string quantizeMethod, Channel channel, ref double[,] block, int blockSize)
        {
            switch (quantizeMethod)
            {
                case "N":
                    {
                        if (channel == Channel.Y)
                        {
                            Quantize(ref block, blockSize, Convert.ToInt32(NYBox.Text));
                        }
                        if (channel == Channel.Cr || channel == Channel.Cb)
                        {
                            Quantize(ref block, blockSize, Convert.ToInt32(NCrCbBox.Text));
                        }
                    }
                    break;
                case "Q":
                    {
                        Quantize(ref block, blockSize, QuantizeMatrix(Convert.ToInt16(AlphaBox.Text), Convert.ToInt16(GammaBox.Text), blockSize));
                    }
                    break;
                case "JPEG":
                    {
                        Quantize(ref block, blockSize, QuantizeMatrix(channel));
                    }
                    break;
                case "JPEG/2":
                    {
                        Quantize(ref block, blockSize, QuantizeMatrix(channel));
                    }
                    break;
            }
        }

        private void Quantize(ref double[,] block, int blockSize, int N) 
        {
            double eps = 1e-2;
            double[] line = new double[blockSize * blockSize];
            int next = 0;
            for (int i = 0; i < blockSize; i++)
            {
                for (int j = 0; j < blockSize; j++)
                {
                    line[next] = block[i, j];
                    next++;
                }
            }
            Array.Sort(line, new Comparison<double>((i1, i2) => i2.CompareTo(i1)));
            double min = line[N - 1];
            for (int i = 0; i < blockSize; i++)
            {
                for (int j = 0; j < blockSize; j++)
                {
                    block[i, j] = Math.Abs(block[i, j] - min) >= eps ? block[i, j] : 0;
                }
            }
        }

        private void Quantize(ref double[,] block, int blockSize, double[,] Q)
        {
            double eps = 1e-2;
            for (int i = 0; i < blockSize; i++)
            {
                for (int j = 0; j < blockSize; j++)
                {
                    block[i, j] = Math.Abs(block[i, j] / Q[i, j]) >= eps ? block[i, j] : 0;
                }
            }
        }

        private double[,] QuantizeMatrix(int alpha, int gamma, int blockSize)
        {
            double[,] QMatrix = new double[blockSize, blockSize];
            for (short i = 0; i < blockSize; i++)
            {
                for (short j = 0; j < blockSize; j++)
                {
                    QMatrix[i,j] = (alpha * (1 + gamma * (i + j + 2)));
                }
            }
            return QMatrix;
        }

        private double[,] QuantizeMatrix(Channel channel)
        {
            if (channel == Channel.Y)
            {
                 double[,] Y = {{16, 11, 10, 16, 24, 40, 51, 61},
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
            if (channel == Channel.Cr || channel == Channel.Cb)
            {
                double[,] CrCb = {{17, 18, 24, 47, 99, 99, 99, 99},
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
            return null;
        }

        private double[,] Multiply(double[,] M, double[,] block, double[,] Mt, int blockSize)
        {
            double[,] res = new double[blockSize, blockSize];
            double[,] temp = new double[blockSize, blockSize];
            for (int i = 0; i < blockSize; i++)
            {
                for (int j = 0; j < blockSize; j++)
                {
                    double acc = 0;
                    for (int k = 0; k < blockSize; k++)
                    {
                        acc += (M[i, k] * block[k, j]);
                    }
                    temp[i, j] = acc;
                }
            }
            for (int i = 0; i < blockSize; i++)
            {
                for (int j = 0; j < blockSize; j++)
                {
                    double acc = 0;
                    for (int k = 0; k < blockSize; k++)
                    {
                        acc += (temp[i, k] * Mt[k, j]);
                    }
                    res[i, j] = acc;
                }
            }
            return res;
        }

        private void ToYCrCb(Bitmap src, out double[,] Y, out double[,] Cr, out double[,] Cb)
        {
            Y = new double[src.Width, src.Height];
            Cr = new double[src.Width, src.Height];
            Cb = new double[src.Width, src.Height];
            for (int i = 0; i < src.Height; i++)
            {
                for (int j = 0; j < src.Width; j++)
                {
                    Color color = src.GetPixel(i, j);
                    int R = color.R;
                    int G = color.G;
                    int B = color.B;
                    double y = _y(R, G, B);
                    double cr = _cr(R, G, B);
                    double cb = _cb(R, G, B);
                    Y[i, j] = y;
                    Cr[i, j] = cr;
                    Cb[i, j] = cb;
                }
            }
        }

        private void ToRGB(out Bitmap res, double[,] Y, double[,] Cr, double[,] Cb, int size)
        {
            res = new Bitmap(size, size);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    double y = Y[i, j];
                    double cr = Cr[i, j];
                    double cb = Cb[i, j];
                    int R = toCorrectRange(_r1(y, cr, cb));
                    int G = toCorrectRange(_g1(y, cr, cb));
                    int B = toCorrectRange(_b1(y, cr, cb));
                    Color color = Color.FromArgb(R, G, B);
                    res.SetPixel(i, j, color);
                }
            }
        }
#endregion: Кодирование(JPEG) и раскодирование
    }
}