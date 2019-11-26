using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerialForwarder
{
    public partial class Form1 : Form
    {

        Boolean started = false;
        SerialPort port1;
        SerialPort port2;
        string data1, data2;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            foreach (string s in SerialPort.GetPortNames())
            {
                comboBox1.Items.Add(s);
                comboBox2.Items.Add(s);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            comboBox1.Enabled = false;
            button1.Enabled = false;

            try
            {
                port1 = new SerialPort(comboBox1.SelectedItem.ToString(), int.Parse(numericUpDown1.Value.ToString()), Parity.None, 8, StopBits.One);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void Button2_Click(object sender, EventArgs e)
        {
            comboBox2.Enabled = false;
            button2.Enabled = false;

            try
            {
                port2 = new SerialPort(comboBox2.SelectedItem.ToString(), int.Parse(numericUpDown2.Value.ToString()), Parity.None, 8, StopBits.One);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Port1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            var data = port1.ReadExisting();
            data1 = data;
            if (checkBox1.Checked)
            {
                if (port1.IsOpen && port2.IsOpen)
                {
                    port2.Write(data);
                }
            }

        }

        private void Port2_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {

            var data = port2.ReadExisting();
            data2 = data;
            if (checkBox2.Checked)
            {
                if (port1.IsOpen && port2.IsOpen)
                {
                    port1.Write(data);
                }
            }

        }

        private void Button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (!started)
                {
                    started = true;
                    button3.Text = "Stop";
                    groupBox1.Enabled = false;
                    groupBox2.Enabled = false;
                    port1.Open();
                    port1.DataReceived += new SerialDataReceivedEventHandler(Port1_DataReceived);
                    port2.Open();
                    port2.DataReceived += new SerialDataReceivedEventHandler(Port2_DataReceived);
                    timer1.Start();
                }
                else
                {
                    started = false;
                    button3.Text = "Start";
                    groupBox1.Enabled = true;
                    groupBox2.Enabled = true;
                    port1.Close();
                    port2.Close();
                    timer1.Stop();
                }
            }
            catch (NullReferenceException)
            {
                MessageBox.Show("Select all serial ports");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/adryyyy/");
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {

            if (checkBox3.Checked && data1 != null)
            {
                listBox1.Items.Add(port1.PortName + ": " + data1);

                if (checkBox4.Checked)
                {
                    int visibleItems = listBox1.ClientSize.Height / listBox1.ItemHeight;
                    listBox1.TopIndex = Math.Max(listBox1.Items.Count - visibleItems + 1, 0);
                }
            }


            if (checkBox3.Checked && data2 != null)
            {
                listBox1.Items.Add(port2.PortName + ": " + data2);

                if (checkBox4.Checked)
                {
                    int visibleItems = listBox1.ClientSize.Height / listBox1.ItemHeight;
                    listBox1.TopIndex = Math.Max(listBox1.Items.Count - visibleItems + 1, 0);
                }
            }
        }
    }
}
