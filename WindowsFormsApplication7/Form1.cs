using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using openCV;
using WindowsFormsApplication7;

namespace WindowsFormsApplication7
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        IplImage image1;
        Bitmap bmp;
        IplImage img;
        private object bitmap;

public object Histogram { get; private set; }

        

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            openFileDialog1.FileName = " ";
            openFileDialog1.Filter = "JPEG|*JPG|Bitmap|*.bmp|All|*.*-11";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {

                    image1 = cvlib.CvLoadImage(openFileDialog1.FileName, cvlib.CV_LOAD_IMAGE_COLOR);
                    CvSize size = new CvSize(pictureBox1.Width, pictureBox1.Height);
                    IplImage resized_image = cvlib.CvCreateImage(size, image1.depth, image1.nChannels);
                    cvlib.CvResize(ref image1, ref resized_image, cvlib.CV_INTER_LINEAR);
                    pictureBox1.Image = (Image)resized_image;



                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }

        private void grayscalToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Resize خاص لتحويل grayscale
                CvSize size = new CvSize(pictureBox3.Width, pictureBox3.Height);
                IplImage resized_image = cvlib.CvCreateImage(size, image1.depth, image1.nChannels);
                cvlib.CvResize(ref image1, ref resized_image, cvlib.CV_INTER_LINEAR);

                // تحويل إلى Bitmap
                Bitmap bmp = new Bitmap(resized_image.width, resized_image.height, resized_image.widthStep,
                                        System.Drawing.Imaging.PixelFormat.Format24bppRgb,
                                        resized_image.imageData);

                int width = bmp.Width;
                int height = bmp.Height;
                Color p;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        p = bmp.GetPixel(x, y);
                        int a = p.A;
                        int r = p.R;
                        int g = p.G;
                        int b = p.B;
                        int avg = (r + g + b) / 3;
                        bmp.SetPixel(x, y, Color.FromArgb(a, avg, avg, avg));
                    }
                }

                pictureBox3.Image = bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void redToolStripMenuItem_Click(object sender, EventArgs e)
        {
            img = cvlib.CvCreateImage(new CvSize(image1.width, image1.height), image1.depth, image1.nChannels);
            int srcAdd = image1.imageData.ToInt32();
            int dstAdd = img.imageData.ToInt32();
            unsafe
            {
                int srcIndex, dstIndex;
                for (int r = 0; r < img.height; r++)
                    for (int c = 0; c < img.width; c++)
                    {
                        srcIndex = dstIndex = (img.width * r * img.nChannels) + (c * img.nChannels);
                        *(byte*)(dstAdd + dstIndex + 0) = 0;
                        *(byte*)(dstAdd + dstIndex + 1) = 0;
                        *(byte*)(dstAdd + dstIndex + 2) = *(byte*)(srcAdd + srcIndex + 2);
                    }
            }
            CvSize size = new CvSize(pictureBox4.Width, pictureBox4.Height);
            IplImage resized_image = cvlib.CvCreateImage(size, img.depth, img.nChannels);
            cvlib.CvResize(ref img, ref resized_image, cvlib.CV_INTER_LINEAR);
            pictureBox4.BackgroundImage = (Image)resized_image;
        }

        private void greenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            img = cvlib.CvCreateImage(new CvSize(image1.width, image1.height), image1.depth, image1.nChannels);
            int srcAdd = image1.imageData.ToInt32();
            int dstAdd = img.imageData.ToInt32();
            unsafe
            {
                int srcIndex, dstIndex;
                for (int r = 0; r < img.height; r++)
                    for (int c = 0; c < img.width; c++)
                    {
                        srcIndex = dstIndex = (img.width * r * img.nChannels) + (c * img.nChannels);
                        *(byte*)(dstAdd + dstIndex + 0) = 0;
                        *(byte*)(dstAdd + dstIndex + 1) = *(byte*)(srcAdd + srcIndex + 1);
                        *(byte*)(dstAdd + dstIndex + 2) = 0;
                    }
            }
            CvSize size = new CvSize(pictureBox4.Width, pictureBox4.Height);
            IplImage resized_image = cvlib.CvCreateImage(size, img.depth, img.nChannels);
            cvlib.CvResize(ref img, ref resized_image, cvlib.CV_INTER_LINEAR);
            pictureBox4.BackgroundImage = (Image)resized_image;
        }

        private void blueToolStripMenuItem_Click(object sender, EventArgs e)
        {
            img = cvlib.CvCreateImage(new CvSize(image1.width, image1.height), image1.depth, image1.nChannels);
            int srcAdd = image1.imageData.ToInt32();
            int dstAdd = img.imageData.ToInt32();
            unsafe
            {
                int srcIndex, dstIndex;
                for (int r = 0; r < img.height; r++)
                    for (int c = 0; c < img.width; c++)
                    {
                        srcIndex = dstIndex = (img.width * r * img.nChannels) + (c * img.nChannels);
                        *(byte*)(dstAdd + dstIndex + 0) = *(byte*)(srcAdd + srcIndex + 0);
                        *(byte*)(dstAdd + dstIndex + 1) = 0;
                        *(byte*)(dstAdd + dstIndex + 2) = 0;
                    }
            }
            CvSize size = new CvSize(pictureBox4.Width, pictureBox4.Height);
            IplImage resized_image = cvlib.CvCreateImage(size, img.depth, img.nChannels);
            cvlib.CvResize(ref img, ref resized_image, cvlib.CV_INTER_LINEAR);
            pictureBox4.BackgroundImage = (Image)resized_image;
        }
        //////////////

        private void sobelEdgeDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CvSize size = new CvSize(pictureBox2.Width, pictureBox2.Height);
                IplImage resized_image = cvlib.CvCreateImage(size, image1.depth, image1.nChannels);
                cvlib.CvResize(ref image1, ref resized_image, cvlib.CV_INTER_LINEAR);

                // <span style="color:blue">تحويل إلى Grayscale</span>
                IplImage gray = cvlib.CvCreateImage(size, 8, 1);
                cvlib.CvCvtColor(ref resized_image, ref gray, cvlib.CV_BGR2GRAY);

                IplImage grad_x = cvlib.CvCreateImage(size, 8, 1);
                IplImage grad_y = cvlib.CvCreateImage(size, 8, 1);
                IplImage abs_grad_x = cvlib.CvCreateImage(size, 8, 1);
                IplImage abs_grad_y = cvlib.CvCreateImage(size, 8, 1);

                cvlib.CvSobel(ref gray, ref grad_x, 1, 0, 3);
                cvlib.CvSobel(ref gray, ref grad_y, 0, 1, 3);

                cvlib.CvConvertScaleAbs(ref grad_x, ref abs_grad_x, 1, 0);
                cvlib.CvConvertScaleAbs(ref grad_y, ref abs_grad_y, 1, 0);

                IplImage edge = cvlib.CvCreateImage(size, 8, 1);
                cvlib.CvAddWeighted(ref abs_grad_x, 0.5, ref abs_grad_y, 0.5, 0, ref edge);

                
                IplImage edgeColor = cvlib.CvCreateImage(size, 8, 3);
                cvlib.CvCvtColor(ref edge, ref edgeColor, cvlib.CV_GRAY2BGR);

                Bitmap bmp = new Bitmap(edgeColor.width, edgeColor.height, edgeColor.widthStep,
                                        System.Drawing.Imaging.PixelFormat.Format24bppRgb,
                                        edgeColor.imageData);

                pictureBox2.Image = bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sobel Error: " + ex.Message);
            }
        }

        private void gaussianBlurToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // حجم الصورة المعاد تحجيمها لتتناسب مع PictureBox
                CvSize size = new CvSize(pictureBox2.Width, pictureBox2.Height);
                IplImage resized_image = cvlib.CvCreateImage(size, image1.depth, image1.nChannels);
                cvlib.CvResize(ref image1, ref resized_image, cvlib.CV_INTER_LINEAR);

                // إنشاء صورة جديدة للنتيجة
                IplImage blurred = cvlib.CvCreateImage(size, image1.depth, image1.nChannels);

                // تطبيق Gaussian Blur مع نواة أكبر (مثل 15x15 بدلاً من 5x5)
                int kernelSize = 15; // يمكنك تغيير هذه القيمة لزيادة أو تقليل التمويه
                cvlib.CvSmooth(ref resized_image, ref blurred, cvlib.CV_GAUSSIAN, kernelSize, kernelSize, 0, 0);

                // تحويل النتيجة إلى Bitmap لعرضها
                Bitmap bmp = new Bitmap(blurred.width, blurred.height, blurred.widthStep,
                                        System.Drawing.Imaging.PixelFormat.Format24bppRgb,
                                        blurred.imageData);

                pictureBox2.Image = bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Gaussian Blur Error: " + ex.Message);
            }
        }

        private void negativeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                CvSize size = new CvSize(pictureBox2.Width, pictureBox2.Height);
                IplImage resized_image = cvlib.CvCreateImage(size, image1.depth, image1.nChannels);
                cvlib.CvResize(ref image1, ref resized_image, cvlib.CV_INTER_LINEAR);

                Bitmap bmp = new Bitmap(resized_image.width, resized_image.height, resized_image.widthStep,
                                        System.Drawing.Imaging.PixelFormat.Format24bppRgb,
                                        resized_image.imageData);

                int width = bmp.Width;
                int height = bmp.Height;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Color pixel = bmp.GetPixel(x, y);
                        Color negativeColor = Color.FromArgb(
                            pixel.A,
                            255 - pixel.R,
                            255 - pixel.G,
                            255 - pixel.B
                        );
                        bmp.SetPixel(x, y, negativeColor);
                    }
                }

                pictureBox2.Image = bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Negative Filter Error: " + ex.Message);
            }
        }

        private void brightnessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int brightnessOffset = 50; // ممكن تغير القيمة حسب الحاجة (موجب لزيادة الإضاءة، وسالب لتقليلها)

                CvSize size = new CvSize(pictureBox2.Width, pictureBox2.Height);
                IplImage resized_image = cvlib.CvCreateImage(size, image1.depth, image1.nChannels);
                cvlib.CvResize(ref image1, ref resized_image, cvlib.CV_INTER_LINEAR);

                Bitmap bmp = new Bitmap(resized_image.width, resized_image.height, resized_image.widthStep,
                                        System.Drawing.Imaging.PixelFormat.Format24bppRgb,
                                        resized_image.imageData);

                int width = bmp.Width;
                int height = bmp.Height;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Color pixel = bmp.GetPixel(x, y);

                        int r = Math.Min(255, Math.Max(0, pixel.R + brightnessOffset));
                        int g = Math.Min(255, Math.Max(0, pixel.G + brightnessOffset));
                        int b = Math.Min(255, Math.Max(0, pixel.B + brightnessOffset));

                        Color brightColor = Color.FromArgb(pixel.A, r, g, b);
                        bmp.SetPixel(x, y, brightColor);
                    }
                }

                pictureBox2.Image = bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Brightness Filter Error: " + ex.Message);
            }
        }

        private void warmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
               int warmFactor = 20; // زوّد أو قلل هذه القيمة حسب قوة التأثير

                CvSize size = new CvSize(pictureBox2.Width, pictureBox2.Height);
                IplImage resized_image = cvlib.CvCreateImage(size, image1.depth, image1.nChannels);
                cvlib.CvResize(ref image1, ref resized_image, cvlib.CV_INTER_LINEAR);

                Bitmap bmp = new Bitmap(resized_image.width, resized_image.height, resized_image.widthStep,
                                        System.Drawing.Imaging.PixelFormat.Format24bppRgb,
                                        resized_image.imageData);

                int width = bmp.Width;
                int height = bmp.Height;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        Color pixel = bmp.GetPixel(x, y);

                        int r = Math.Min(255, pixel.R + warmFactor);
                        int b = Math.Max(0, pixel.B - warmFactor);

                        Color warmColor = Color.FromArgb(pixel.A, r, pixel.G, b);
                        bmp.SetPixel(x, y, warmColor);
                    }
                }

                pictureBox2.Image = bmp;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Warm Filter Error: " + ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
             
        }

        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 frame2 = new Form2();
            frame2.Show();
            this.Hide();
        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        

        
 }

        
    }

