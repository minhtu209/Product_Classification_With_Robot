using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using Emgu.CV.UI;
using Emgu.CV.Structure;
using Emgu.Util;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using OpenCvSharp;
using OpenCvSharp.Blob;
using System.IO;
using System.IO.Ports;
using System.Xml;
using Size = System.Drawing.Size;

namespace WindowsFormsAppcamera
{
    
    public partial class Form1 : Form
    {   

        private FilterInfoCollection cameras;
        //private VideoCaptureDevice cam; 
        MJPEGStream cam;
        int d;
        Image<Bgr, byte> src_image;
        //Image<Bgr, byte> src_image = new Image<Bgr, byte>("C:\\Users\\Admin\\Desktop\\Anhchuoi\\Tamgiacloi.png");
        static SerialPort serialPort1 = new SerialPort();




        public Form1()
        {
            InitializeComponent();

            
            //kiem tra ket noi camera
            cameras = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            //move camera to combox
            foreach(FilterInfo info in cameras)
            {
                comboBox1.Items.Add(info.Name);
            }
            comboBox1.SelectedIndex = 0;

            comboBox2.DataSource = SerialPort.GetPortNames();
            string[] BoardRateArray = { "1200","2400","3600","4800","9600","19200","38400" };
            comboBox3.DataSource = BoardRateArray;
            comboBox3.SelectedIndex = 4;

            
            

        }
        public void button1_Click(object sender, EventArgs e)
        {
             button1.BackColor = Color.Green;
             if(cam != null && cam.IsRunning)
             {
                 cam.Stop();
             }
             //cam = new VideoCaptureDevice(cameras[comboBox1.SelectedIndex].MonikerString);
             cam = new MJPEGStream("http://192.168.1.6:4747/video");
             cam.NewFrame += Cam_NewFrame;
             cam.Start();

            

        }
       public void Cam_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = (Bitmap)eventArgs.Frame.Clone();
            Bitmap bitmap3 = (Bitmap)eventArgs.Frame.Clone();
            Image<Bgr, byte> image = new Image<Bgr, byte>(bitmap);
            Image<Bgr, byte> image1 = new Image<Bgr, byte>(bitmap3);
            src_image = new Image<Bgr, byte>(bitmap3);
            pictureBox1.Image = image.Bitmap;          
            
            
        }
       
        private void findcontours( Image<Bgr,byte> img_src)
        {
            Image<Gray, byte> img_out = img_src.Convert<Gray, byte>();//.ThresholdBinary(new Gray(100), new Gray(255));
            
            CvInvoke.GaussianBlur(img_out, img_out, new Size(5, 5),3);
            CvInvoke.Canny(img_out, img_out, 50, 120, 3);

            //Emgu.CV.Mat kernel1 =  CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle,new Size(3,3),new System.Drawing.Point(-1,-1));
            //CvInvoke.Dilate(img_out, img_out, kernel1, new System.Drawing.Point(-1, -1), 6, BorderType.Constant, new MCvScalar(255, 255, 255));
           Emgu.CV.Util.VectorOfVectorOfPoint countours = new Emgu.CV.Util.VectorOfVectorOfPoint();
           Emgu.CV.Mat hier = new Emgu.CV.Mat();
            CvInvoke.FindContours(img_out,countours,hier,Emgu.CV.CvEnum.RetrType.External,
                Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);

            for(int i = 0; i < countours.Size; i++ )
            {
              Emgu.CV.Util.VectorOfVectorOfPoint ConPoly = new Emgu.CV.Util.
                    VectorOfVectorOfPoint(countours.Size);               
                    double peri = CvInvoke.ArcLength(countours[i], true);
                    CvInvoke.ApproxPolyDP(countours[i],ConPoly[i], 0.01 * peri, true);
                    CvInvoke.DrawContours(img_src, countours, i, new MCvScalar(255, 255, 255), 3);
                    //bourRect[i] = CvInvoke.BoundingRectangle(ConPoly[i]);
                    
                    double objCor = (double)ConPoly[i].Size;
                   
                    var momments = CvInvoke.Moments(countours[i]);
                    int x = (int)(momments.M10 / momments.M00);
                    int y = (int)(momments.M01 / momments.M00);
                   /* if (objCor == 3)
                    {                       
                        CvInvoke.PutText(img_src, "tris ", new System.Drawing.Point(x,y), FontFace.HersheyDuplex, 0.75,
                          new MCvScalar(255, 0, 0), 2);
                        textBox3.Text = " Tam giác " ;
                    }*/
                  

                    if(objCor == 4)
                    {
                        CvInvoke.PutText(img_src, "rect ", new System.Drawing.Point(x, y), FontFace.HersheyDuplex, 0.75,
                            new MCvScalar(255, 255, 0), 3);
                        textBox3.Text = " Hình vuông ";
                        d =3;
                    
                    }
     
                    if(objCor >= 6)
                    {
                        CvInvoke.PutText(img_src, "circle ", new System.Drawing.Point(x, y), FontFace.HersheyDuplex, 0.75,
            new MCvScalar(255, 255, 0), 3);
                        textBox3.Text = " Hình Tròn ";
                        d = 2;
                    }                   
                  
            }           
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            button3.BackColor = Color.Red;
            if ( cam != null && cam.IsRunning)
            {
                cam.Stop();
            }
            try
            {
               
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot  control");
            }

        }
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            if (cam != null && cam.IsRunning)
            {
                cam.Stop();
            }

        }

        public void button2_Click(object sender, EventArgs e)
        {

            button2.BackColor = Color.Blue;   
            findcontours(src_image);
            pictureBox2.Image = src_image.Bitmap;      
           
           
           try
            {
                if (d == 3)
                {
                    serialPort1.Write("3");
                }
                if ( d ==2 )
               {
                   serialPort1.Write("2");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot  control");
            }
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            button4.BackColor = Color.Green;
            try
            {
                if( serialPort1.IsOpen)
                {  
                    MessageBox.Show(" Connection is existing ");
                }
                else
                {
                    serialPort1.PortName = comboBox2.Text;
                    serialPort1.BaudRate = Convert.ToInt32(comboBox3.Text);
                    serialPort1.Open();                   
                    MessageBox.Show(" Connection is successfull");
                }

            }
            catch (Exception ex)
            {   
                MessageBox.Show(" Connection is  failed" );
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            button5.BackColor = Color.Red;
            button4.BackColor = Color.White;

            try
            {
                if (!serialPort1.IsOpen)
                {
                    MessageBox.Show(" Connection is close" );
                }
                else
                {
                    serialPort1.Close();              
                    MessageBox.Show(" Connection closing is successfull");
                }
            }
            catch(Exception ex )
            {
                MessageBox.Show(" Connection is failed");
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
