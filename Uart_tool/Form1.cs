﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Uart_tool
{
    public partial class Form1 : Form
    {




       // private StringBuilder sb = new StringBuilder();
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




            serialPort1.DataReceived += new SerialDataReceivedEventHandler(port_DataReceived);
        }

        private void port_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                //因为要访问UI资源，所以需要使用invoke方式同步ui
                this.Invoke((EventHandler)(delegate
                {
                    textBox_receive.AppendText(serialPort1.ReadExisting());
                }
                   )
                );

            }
            catch (Exception ex)
            {
                //响铃并显示异常给用户
                System.Media.SystemSounds.Beep.Play();
                MessageBox.Show(ex.Message);

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] temp = new byte[10];
                //首先判断串口是否开启
                if (serialPort1.IsOpen)                 
                {
                    //串口处于开启状态，将发送区文本发送
                    //serialPort1.Write(textBox_send.Text);
                    if (radioButton1.Checked)       //hex模式
                    {
                        //以HEX模式发送
                        //首先需要用正则表达式将用户输入字符中的十六进制字符匹配出来
                        string buf = textBox_send.Text;
                        string pattern = @"\s";
                        string replacement = "";
                        Regex rgx = new Regex(pattern);
                        string send_data = rgx.Replace(buf, replacement);           //替换空格
                        MessageBox.Show(send_data);
                        //不发送新行
                       int  num = (send_data.Length - send_data.Length % 2) / 2;
                        for (int i = 0; i < num; i++)
                        {
                            temp[0] = Convert.ToByte(send_data.Substring(i * 2, 2), 16);
                            serialPort1.Write(temp, 0, 1);  //循环发送
                        }
                        //如果用户输入的字符是奇数，则单独处理
                        if (send_data.Length % 2 != 0)
                        {
                            temp[0] = Convert.ToByte(send_data.Substring(textBox_send.Text.Length - 1, 1), 16);
                            serialPort1.Write(temp, 0, 1);
                            num++;
                        }
                        //判断是否需要发送新行
                        //if (checkBox3.Checked)
                        //{
                        //    //自动发送新行
                        //    serialPort1.WriteLine("");
                        //}
                    }
                    else
                    { 
                        
                    }
                }
            }
            catch (Exception ex)
            {
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

        private void button3_Click(object sender, EventArgs e)
        {
            byte[] buffer = new byte[100];
           
           
            
        }
    }
}
