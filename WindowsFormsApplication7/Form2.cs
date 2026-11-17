using openCV;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


using System.Windows.Forms.DataVisualization.Charting;
using System.IO;
namespace WindowsFormsApplication7
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }
        IplImage originalImage;
        Bitmap equalizedImage;

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = " ";
            openFileDialog1.Filter = "JPEG|*JPG|Bitmap|*.bmp|All|*.*-11";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    originalImage = cvlib.CvLoadImage(openFileDialog1.FileName, cvlib.CV_LOAD_IMAGE_COLOR);
                    CvSize size = new CvSize(pictureBox1.Width, pictureBox1.Height);
                    IplImage resized_image = cvlib.CvCreateImage(size, originalImage.depth, originalImage.nChannels);
                    cvlib.CvResize(ref originalImage, ref resized_image, cvlib.CV_INTER_LINEAR);
                    pictureBox1.Image = (System.Drawing.Image)resized_image;

                    //pictureBox1.BackgroundImage = (Image)image1;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
        
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap hisImage = (Bitmap)originalImage;
            int width = hisImage.Width;
            int height = hisImage.Height;

            int[] arrRed = new int[256];
            int[] arrGreen = new int[256];
            int[] arrBlue = new int[256];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    Color pixelColor = hisImage.GetPixel(j, i);
                    arrRed[pixelColor.R]++;
                    arrGreen[pixelColor.G]++;
                    arrBlue[pixelColor.B]++;

                }
            }

            //Histogram

            chart1.Series["Red"].Points.Clear();
            chart1.Series["Green"].Points.Clear();
            chart1.Series["Blue"].Points.Clear();

            for (int i = 0; i < 256; i++)
            {
                chart1.Series["Red"].Points.AddY(arrRed[i]);
                chart1.Series["Green"].Points.AddY(arrGreen[i]);
                chart1.Series["Blue"].Points.AddY(arrBlue[i]);

            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (equalizedImage == null)
            {
                MessageBox.Show("No equalized image found.");
                return;
            }

            int[] equalRed = new int[256];
            int[] equalGreen = new int[256];
            int[] equalBlue = new int[256];

            int h = equalizedImage.Height;
            int w = equalizedImage.Width;

            for (int i = 0; i < h; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    Color pixelColor = equalizedImage.GetPixel(j, i);
                    equalRed[pixelColor.R]++;
                    equalGreen[pixelColor.G]++;
                    equalBlue[pixelColor.B]++;
                }
            }

            chart2.Series["Red"].Points.Clear();
            chart2.Series["Green"].Points.Clear();
            chart2.Series["Blue"].Points.Clear();

            for (int i = 0; i < 256; i++)
            {
                chart2.Series["Red"].Points.AddY(equalRed[i]);
                chart2.Series["Green"].Points.AddY(equalGreen[i]);
                chart2.Series["Blue"].Points.AddY(equalBlue[i]);
            }
        }


        private void button3_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("Sorry, there is no picture ");
            }
            else
            {
                Bitmap originalImageCopy = (Bitmap)originalImage;
                using (MemoryStream ms = new MemoryStream())
                {
                    pictureBox1.Image.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
                    originalImageCopy = new Bitmap(ms);
                }
                int width = originalImageCopy.Width;
                int height = originalImageCopy.Height;

                equalizedImage = new Bitmap(width, height);

                int[] arrRed = new int[256];
                int[] arrGreen = new int[256];
                int[] arrBlue = new int[256];

                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        Color pixelColor = originalImageCopy.GetPixel(j, i);
                        arrRed[pixelColor.R]++;
                        arrGreen[pixelColor.G]++;
                        arrBlue[pixelColor.B]++;
                    }
                }

                decimal[] proArrRed = new decimal[256];
                decimal[] proArrGreen = new decimal[256];
                decimal[] proArrBlue = new decimal[256];
                for (int i = 0; i < 256; i++)
                {
                    proArrRed[i] = (decimal)arrRed[i] / (decimal)(width * height);
                    proArrGreen[i] = (decimal)arrGreen[i] / (decimal)(width * height);
                    proArrBlue[i] = (decimal)arrBlue[i] / (decimal)(width * height);
                }

                decimal[] cdfArrRed = new decimal[256];
                decimal[] cdfArrGreen = new decimal[256];
                decimal[] cdfArrBlue = new decimal[256];

                cdfArrRed[0] = proArrRed[0];
                cdfArrGreen[0] = proArrGreen[0];
                cdfArrBlue[0] = proArrBlue[0];
                for (int i = 1; i < 256; i++)
                {
                    cdfArrRed[i] = proArrRed[i] + cdfArrRed[i - 1];
                    cdfArrGreen[i] = proArrGreen[i] + cdfArrGreen[i - 1];
                    cdfArrBlue[i] = proArrBlue[i] + cdfArrBlue[i - 1];
                }

                int red, green, blue;
                int constant = 255;
                for (int i = 0; i < width; i++)
                {
                    for (int j = 0; j < height; j++)
                    {
                        Color pixelColor = originalImageCopy.GetPixel(i, j);
                        red = Math.Max(0, Math.Min(255, (int)(cdfArrRed[pixelColor.R] * constant)));
                        green = Math.Max(0, Math.Min(255, (int)(cdfArrGreen[pixelColor.G] * constant)));
                        blue = Math.Max(0, Math.Min(255, (int)(cdfArrBlue[pixelColor.B] * constant)));
                        Color newColor = Color.FromArgb(red, green, blue);
                        equalizedImage.SetPixel(i, j, newColor);
                    }
                }

                pictureBox2.Image = equalizedImage;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }
    }
}
