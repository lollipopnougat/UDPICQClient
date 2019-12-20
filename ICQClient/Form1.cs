using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;

namespace ICQClient
{
    public partial class Form1 : Form
    {
        private UDPServer udp;
        private string mes;
        private string targetIp;
        private string currIp;
        private int targetPort;
        private int thisSendPort;
        private int thisRecvPort;
        private string uname = "Alice";
        private string tname = "Bob";
        private string key = "1234567890111213";

        Form2 fm2;
        public Form1()
        {
            InitializeComponent();
            textBox3.Text = key;
            textBox4.Text = tname;
            fm2 = new Form2();
            SetServer();
            FormClosing += ShutdownUdp; //窗体关闭
            try { 
            udp.TurnOn(); //启动UDP监听
            }
            catch(Exception er)
            {
                MessageBox.Show($"{er.Message} 您可以通过 选项-设置连接 来切换其他端口", "出错了！",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// udp数据解析(事件函数)
        /// </summary>
        /// <param name="ReceviceBuff">接收的数据流</param>
        private void udp_EvtGetValues(byte[] ReceviceBuff)
        {
            string message = Encoding.UTF8.GetString(ReceviceBuff, 0, ReceviceBuff.Length);
            //可接收中文内容
            //Encoding ei = Encoding.GetEncoding(936);
            //string message = ei.GetString(ReceviceBuff, 0, ReceviceBuff.Length);
            if (message.Length > 0)
            {
                string demsg = message;
                if(启用加密ToolStripMenuItem.Checked)
                {
                    // 传输的是经过加密的结果使用Base64编码的字符串，使用UTF8作为传送字符串编码
                    demsg = AesEncryption.MyDecryptFromBase64(message, key, "1234567890111213");
                    //demsg = AESManager.Decrypt(Convert.FromBase64String(message), AESManager.StringToByteArray(key, 16), AESManager.StringToByteArray("1234567890131211", 16));
                    //mes = DateTime.Now + " " + uname + " : " + demsg + "\r\n";
                    //setTextBox1(mes);
                }
                // 处理接收逻辑
                //textBox1.Text += DateTime.Now.ToString() + " : " + message;
                //string tmp = DateTime.Now + " " + uname + " : " + message;
                mes = DateTime.Now + " " + uname + " : " + demsg + "\r\n";
                setTextBox1(mes);
                //MessageBox.Show(tmp, "消息");
                //string tmp = DateTime.Now+ " : " + message;
            }

        }

        /// <summary>
        /// 用于修改界面的委托
        /// </summary>
        /// <param name="str"></param>
        delegate void delegateSetTextBox(string str); 
        private void setTextBox1(string str)
        {
            if (this.InvokeRequired) Invoke(new delegateSetTextBox(setTextBox1), new object[] { str });
            else textBox1.Text += str;
        }
        private void setTextBox3(string str)
        {
            if (this.InvokeRequired) Invoke(new delegateSetTextBox(setTextBox3), new object[] { str });
            else textBox3.Text = str;
        }
        private void setLabel2(string str)
        {
            if (this.InvokeRequired) Invoke(new delegateSetTextBox(setLabel2), new object[] { str });
            else label2.Text = str;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(textBox2.Text == null || textBox2.Text =="") 
            {
                MessageBox.Show("消息不能为空!", "错误！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string smsg = textBox2.Text;
            
            string cmsg = smsg;
            // 检测是否启用加密
            if(启用加密ToolStripMenuItem.Checked)
            {
                if(key == "naidesu")
                {
                    MessageBox.Show("未设定会话密钥！", "错误！");
                    return;
                }
                cmsg = AesEncryption.MyEncryptToBase64(smsg, key, "1234567890111213");
                //cmsg = Convert.ToBase64String(AESManager.Encrypt(smsg, AESManager.StringToByteArray(key, 16), AESManager.StringToByteArray("1234567890131211", 16)));
            }
            //发送Udp数据
            UDPServer.SendMsg(targetIp, targetPort, cmsg, currIp, thisSendPort);
            mes = DateTime.Now + " 您 : " + smsg + "\r\n";
            setTextBox1(mes);
            textBox2.Text = "";
        }

        /// <summary>
        /// 初始化Udp服务端
        /// </summary>
        private void SetServer()
        {
            fm2.ShowDialog();
            targetPort = fm2.targetPort;
            targetIp = fm2.targetIp;
            thisSendPort = fm2.thisSendPort;
            thisRecvPort = fm2.thisRecvPort;
            currIp = fm2.currIp;
            uname = fm2.uname;
            udp = new UDPServer(currIp, thisRecvPort, targetIp, targetPort);
            udp.EvtGetValues += new UDPServer.GetRecevice(udp_EvtGetValues); // 事件订阅
        }
        private void 设置连接ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (udp.IsOpen) udp.TurnOff();
            SetServer();
            try
            {
                udp.TurnOn(); 
            }
            catch (Exception er)
            {
                MessageBox.Show($"{er.Message} 您可以通过 选项-设置连接 来切换其他端口", "出错了！", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void 关于此程序ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox1 ab = new AboutBox1();
            ab.ShowDialog();
        }

        private void ShutdownUdp(object sender, EventArgs e)
        {
            if(udp.IsOpen) udp.TurnOff();
        }
        
        /// <summary>
        /// 获取随机字符串
        /// </summary>
        /// <param name="Len">长度 类型:int</param>
        /// <returns>返回的指定长度随机字符串</returns>
        public static string GetRandomString(int Len)
        {
            string Dict = "ABCDEFGHJKMNPQRSTWXYZabcdefhijkmnprstwxyz2345678";
            double DictNum = Dict.Length;
            string Result = "";
            Random random = new Random(Guid.NewGuid().GetHashCode());
            //Console.WriteLine(random.Next());
            for (int i = 0; i < Len; i++)
            {
                Result += Dict.ElementAt((int)Math.Floor(random.NextDouble() * DictNum));
                //retStr += $aes_chars.charAt(Math.floor(Math.random() * aes_chars_len));
            }
            return Result;
        }

        

        private void 启用加密ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(启用加密ToolStripMenuItem.Checked) 启用加密ToolStripMenuItem.Checked = false;
            else 启用加密ToolStripMenuItem.Checked = true;
        }

        private void 帮助ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("程序默认自带会话密钥","注意");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox3.Text.Length != 16) 
            { 
                MessageBox.Show("密钥长度不合法!", "错误!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            key = textBox3.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string url = textBox5.Text;
            // 网络的I/O是非常耗时的，为了防止阻塞主线程，故一般采用异步执行的方式
            // 异步操作 使用了lambda表达式
            Func<string> act = () =>
            {
                string asyncKey = "naidesu";
                try
                {
                    //HttpWebResponse res = Requests.Get("http://localhost/getSessionId", AllowRedirect: true);
                    //string result = Requests.GetResponseText(res);
                    Connection conn = new Connection(uname, tname, url);
                    conn.Req();
                    //string result = "{\"key\":\"390231c23c\"}";
                    //asyncKey = result.Split('\"')[3];
                    asyncKey = conn.Key;
                    return asyncKey;
                }
                catch (Exception ex)
                {
                    setLabel2("出错啦");
                    MessageBox.Show($"发生错误! \n{ex.Message} \n 请检查 {url}", "啊呀！", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                return asyncKey;

            };
            // 异步回调
            AsyncCallback callback = t =>
            {
                key = act.EndInvoke(t);
                if (key != "naidesu")
                {
                    setLabel2("获取会话密钥成功!");
                    MessageBox.Show($"获取的会话密钥是 {key}", "提示");
                    setTextBox3(key);
                    
                }
            };
            setLabel2("正在向服务器请求会话密钥...");
            //开始异步操作执行 异步执行完毕会自动调用回调函数
            act.BeginInvoke(callback, null);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            tname = textBox4.Text;
        }
    }
}
