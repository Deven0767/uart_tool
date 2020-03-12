using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uart_tool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                //将可能产生异常的代码放置在try块中
                //根据当前串口属性来判断是否打开
                if (serialPort1.IsOpen)
                {
                    //串口已经处于打开状态
                    serialPort1.Close();    //关闭串口
                    button1.Text = "打开串口";
                    button1.BackColor = Color.ForestGreen;
                    comboBox1.Enabled = true;
                    comboBox2.Enabled = true;
                    //comboBox3.Enabled = true;
                    //comboBox4.Enabled = true;
                    //comboBox5.Enabled = true;
                    textBox_receive.Text = "";  //清空接收区
                    textBox_send.Text = "";     //清空发送区
                }
                else
                {
                    //串口已经处于关闭状态，则设置好串口属性后打开
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;
          
                    serialPort1.PortName = comboBox1.Text;
                    serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text);
                    serialPort1.DataBits = 8;// Convert.ToInt16(comboBox3.Text);

                   // if (comboBox4.Text.Equals("None"))
                        serialPort1.Parity = System.IO.Ports.Parity.None;
                    //else if (comboBox4.Text.Equals("Odd"))
                    //    serialPort1.Parity = System.IO.Ports.Parity.Odd;
                    //else if (comboBox4.Text.Equals("Even"))
                    //    serialPort1.Parity = System.IO.Ports.Parity.Even;
                    //else if (comboBox4.Text.Equals("Mark"))
                    //    serialPort1.Parity = System.IO.Ports.Parity.Mark;
                    //else if (comboBox4.Text.Equals("Space"))
                    //    serialPort1.Parity = System.IO.Ports.Parity.Space;

                   // if (comboBox5.Text.Equals("1"))
                        serialPort1.StopBits = System.IO.Ports.StopBits.One;
                    //else if (comboBox5.Text.Equals("1.5"))
                    //    serialPort1.StopBits = System.IO.Ports.StopBits.OnePointFive;
                    //else if (comboBox5.Text.Equals("2"))
                    //    serialPort1.StopBits = System.IO.Ports.StopBits.Two;

                    serialPort1.Open();     //打开串口
                    button1.Text = "关闭串口";
                    button1.BackColor = Color.Firebrick;
                }
            }
            catch (Exception ex)
            {
                //捕获可能发生的异常并进行处理

                //捕获到异常，创建一个新的对象，之前的不可以再用
                serialPort1 = new System.IO.Ports.SerialPort();
                //刷新COM口选项
                comboBox1.Items.Clear();
                comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
                //响铃并显示异常给用户
                System.Media.SystemSounds.Beep.Play();
                button1.Text = "打开串口";
                button1.BackColor = Color.ForestGreen;
                MessageBox.Show(ex.Message);
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
  
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string[] baud = { "9600", "43000", "56000", "57600", "100000", "115200", "128000", "230400", "256000", "460800" };
            comboBox2.Items.AddRange(baud);

            comboBox1.Items.AddRange(System.IO.Ports.SerialPort.GetPortNames());
        }
    }
}
