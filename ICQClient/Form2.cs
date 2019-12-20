using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICQClient
{
    public partial class Form2 : Form
    {
        public string targetIp = "127.0.0.1";
        public string currIp = "127.0.0.1";
        public int targetPort = 60001;
        public int thisSendPort = 60002;
        public int thisRecvPort = 60000;
        public string uname = "Alice";
        public Form2()
        {
            InitializeComponent();
            textBox1.Text = targetIp;
            textBox2.Text = targetPort.ToString();
            textBox3.Text = thisSendPort.ToString();
            textBox4.Text = thisRecvPort.ToString();
            textBox5.Text = uname;
            textBox6.Text = currIp.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox1.Text != "" && textBox2.Text != "" && textBox3.Text != "" && textBox5.Text != "")
            {
                targetIp = textBox1.Text;
                targetPort = Convert.ToInt32(textBox2.Text);
                thisSendPort = Convert.ToInt32(textBox3.Text);
                thisRecvPort = Convert.ToInt32(textBox4.Text);
                uname = textBox5.Text;
                this.Close();
            }
            else
            {
                MessageBox.Show("不能留空!","错误");
            }
        }


    }
}
