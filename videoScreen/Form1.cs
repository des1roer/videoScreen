using System;
using System.Drawing;
using System.Windows.Forms;

using Microsoft.Expression.Encoder.ScreenCapture;
using System.IO;


namespace videoScreen
{
    public partial class Form1 : Form
    {
        ScreenCaptureJob _screenCaptureJob;
        string file = Directory.GetCurrentDirectory() + "/test.wmv";
        public Form1()
        {
            _screenCaptureJob = new ScreenCaptureJob();
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Rectangle _screenRectangle = Screen.PrimaryScreen.Bounds;
                
                _screenCaptureJob.CaptureRectangle = _screenRectangle;
                _screenCaptureJob.ShowFlashingBoundary = true;
                _screenCaptureJob.ScreenCaptureVideoProfile.FrameRate = 20;
                _screenCaptureJob.CaptureMouseCursor = true;

                _screenCaptureJob.ScreenCaptureVideoProfile.Quality = 10;
             
                _screenCaptureJob.OutputScreenCaptureFileName = string.Format(file);
                if (File.Exists(_screenCaptureJob.OutputScreenCaptureFileName))
                {
                    File.Delete(_screenCaptureJob.OutputScreenCaptureFileName);
                }
                _screenCaptureJob.Start();
            }
            catch (Exception error) {
                Console.WriteLine(error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            _screenCaptureJob.Stop();
            var ffMpeg = new NReco.VideoConverter.FFMpegConverter();

            ffMpeg.ConvertMedia(file,
                null, // autodetect by input file extension 
                "output.mp4",
                null, // autodetect by output file extension 
                new NReco.VideoConverter.ConvertSettings()
                {
                    
                    //      CustomOutputArgs = " -filter_complex \"[0] yadif=0:-1:0,scale=iw*sar:ih,scale='if(gt(a,16/9),1280,-2)':'if(gt(a,16/9),-2,720)'[scaled];[scaled] pad=1280:720:(ow-iw)/2:(oh-ih)/2:black \" -c:v libx264 -c:a mp3 -ab 128k "
                        CustomOutputArgs = " -vcodec msmpeg4v2  -c:v libx264 -crf 24 -r 20 -b:v 500k"

                }
            );
            //       var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
            //       ffMpeg.ConvertMedia(file, "output.mp4", Format.mp4);

        }
    }
}
