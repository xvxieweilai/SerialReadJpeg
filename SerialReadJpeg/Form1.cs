using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialReadJpeg
{
    public partial class Form1 : Form
    {
        bool start = false;
        bool rec = false;
        byte last_flag;
        public Form1()
        {
            InitializeComponent();
            serialPort1.DataReceived += SerialPort1_DataReceived;
            
        }

        private void SerialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {



            int count = serialPort1.BytesToRead;
            byte[] buff = new byte[count];
            ///接收数据
            serialPort1.Read(buff, 0, count);
 
            //将字节数据转换为二进制数据
            FileStream fs = new FileStream("img.jpg", FileMode.Append, FileAccess.Write); //创建图片
            BinaryWriter bw = new BinaryWriter(fs);
            foreach (byte buf in buff)
            {
                if(buf==0xFF)
                {
                    start = true;
                    Console.WriteLine(buff.ToString());
                }
                if (start)
                {
                    bw.Write(buf);
                }
                
                //textBox2.Text += buf.ToString();

            }
            fs.Close();
            bw.Close();
            
                try
                {
                    Image imgRead = Image.FromFile("img.jpg");
                    pictureBox1.Image = new Bitmap(imgRead);
                    imgRead.Dispose();
                }
                catch (Exception)
                {

                    
                }
               
          
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = false;
            String[] ports = SerialPort.GetPortNames();
           foreach(String sp in ports)
            {
                comboBox1.Items.Add(sp);
                comboBox1.SelectedIndex = 0;
            }
       
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(serialPort1.IsOpen)
            {
                serialPort1.Close();
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                button1.Text = "打开";
            }
            else
            {
                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = int.Parse(comboBox2.Text);
                serialPort1.Open();
                button1.Text = "关闭";
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            start = false;
            textBox1.Text = textBox1.Text.ToUpper();
            if (serialPort1.IsOpen)
            { 
            BinaryWriter bw = new BinaryWriter(new FileStream("img.jpg", FileMode.Create));
            bw.Close();
            serialPort1.WriteLine(textBox1.Text);
            }
            else
            {
                MessageBox.Show("请先打开串口通讯！");
            }
        }

       

        private void button3_Click_1(object sender, EventArgs e)
        {
            BinaryWriter bw = new BinaryWriter(new FileStream("img.jpg", FileMode.Create));
            bw.Close();
            serialPort1.Write("p2.jpg");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            BinaryWriter bw = new BinaryWriter(new FileStream("img.jpg", FileMode.Create));
            bw.Close();
            serialPort1.Write("p3.jpg");
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
        }
    }
}
