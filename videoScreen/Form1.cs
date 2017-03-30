using System;
using System.Drawing;
using System.Windows.Forms;

using Microsoft.Expression.Encoder.ScreenCapture;
using System.IO;

using System.Threading.Tasks;
using System.Timers;
using System.Threading;

namespace videoScreen
{
    public partial class Form1 : Form
    {
        ScreenCaptureJob _screenCaptureJob;
        string file = Directory.GetCurrentDirectory() + "/test.wmv";
        static System.Windows.Forms.Timer myTimer = new System.Windows.Forms.Timer();
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

                myTimer.Tick += new EventHandler(timer1_Tick);

                // Sets the timer interval to 5 seconds.
                myTimer.Interval = 1000 * 30;
                myTimer.Start();

                _screenCaptureJob.Start();
            }
            catch (Exception error) {
                Console.WriteLine(error);
            }
        }

        private void timer1_Tick(object sender, System.EventArgs e)
        {

            myTimer.Stop();

            Console.WriteLine("Текущее время:  " +
             DateTime.Now.ToLongTimeString());
            _screenCaptureJob.Stop();

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
        }



        private void button2_Click(object sender, EventArgs e)
        {
       
            //       var ffMpeg = new NReco.VideoConverter.FFMpegConverter();
            //       ffMpeg.ConvertMedia(file, "output.mp4", Format.mp4);

        }
    }
}
