using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using AForge.Video;
using AForge.Video.DirectShow;

namespace MyDirectShow
{
    public partial class Form1 : Form
    {
        VideoCaptureDevice _videoSource;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // enumerate video devices
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo videoDevice in videoDevices)
            {
                comboBox1.Items.Add(videoDevice);
            }
            if (comboBox1.Items.Count > 0)
            {
                comboBox1.DisplayMember = "Name";
                comboBox1.SelectedIndex = 0;
            }
        }

        private void VideoSource_SnapshotFrame(object sender, NewFrameEventArgs eventArgs)
        {
            throw new NotImplementedException();
        }

        private void VideoSource_NewFrame(object sender, AForge.Video.NewFrameEventArgs eventArgs)
        {
            if (eventArgs.Frame != null)
            {
                Bitmap bmp = (Bitmap)eventArgs.Frame.Clone();

                Rectangle bmpRect = new Rectangle(0, 0, bmp.Width, bmp.Height);
                System.Drawing.Imaging.BitmapData bmpData = bmp.LockBits(bmpRect, System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);
                AForge.Imaging.Drawing.Rectangle(bmpData, new Rectangle(10, 10, 100, 100), Color.Green);
                bmp.UnlockBits(bmpData);
                pictureBox1.Image = bmp;
            }

        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            FilterInfo videoDevice = (FilterInfo)comboBox1.Items[comboBox1.SelectedIndex];
            _videoSource = new VideoCaptureDevice(videoDevice.MonikerString);
            _videoSource.NewFrame += VideoSource_NewFrame;
            _videoSource.Start();

        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            if(_videoSource != null)
                _videoSource.Stop();
        }
    }

}
