using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ICQClient
{
    /// <summary>
    /// Udp 服务类 包含发送、接收功能
    /// 服务端监听采用多线程(防止阻塞主线程)
    /// </summary>
    public class UDPServer
    {
        public bool IsOpen 
        {
            get { return isOpen; }
        }
        #region 内部变量

        private string devIP = "127.0.0.1";
        private int devPort = 60000;
        private UdpClient mySocket;
        private string meIP = "127.0.0.1";
        private int mePort = 60000;
        private IPEndPoint RemotePoint;
        private bool isRunning = false;
        private bool isOpen = false;
        private List<Thread> listThread = new List<Thread>();
        //private string icId = "";
        //private string cardContent = "";
        //byte[] cardContentBuf;
        #endregion

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="ServerIP">服务端ip</param>
            /// <param name="ServerPort">服务端端口</param>
            /// <param name="DevIP">devip</param>
            /// <param name="DevPort">dev端口</param>
        public UDPServer(string ServerIP = "127.0.0.1", int ServerPort = 60000, string DevIP = "127.0.0.1", int DevPort = 60000)
        {
            this.meIP = ServerIP;
            this.mePort = ServerPort;
            this.devIP = DevIP;
            this.devPort = DevPort;

        }

        #region 静态方法
        /// <summary>
        /// 发送Udp数据
        /// </summary>
        /// <param name="toip">目的ip</param>
        /// <param name="toport">目的端口</param>
        /// <param name="mes">消息</param>
        /// <param name="thisport">本地发送端口</param>
        public static void SendMsg(string toip, int toport, string mes,string thisip, int thisport)
        {
            try
            {
                string message = mes;
                //IPEndPoint localIpep = new IPEndPoint(IPAddress.Parse(thisip), thisport);
                //UdpClient udpclient = new UdpClient(localIpep);
                UdpClient udpclient = new UdpClient(thisport);
                IPEndPoint ipendpoint = new IPEndPoint(IPAddress.Parse(toip), toport);

                byte[] data = Encoding.UTF8.GetBytes(message);
                udpclient.Send(data, data.Length, ipendpoint);
                udpclient.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show("UDP发送数据: " + ex.ToString(), "错误");
            }

        }
        #endregion

        #region 非静态方法 

        /// <summary>
        /// 启动UDP服务端监听
        /// </summary>
        public void TurnOn()
        {
            try
            {
                if (isOpen) return;
                mySocket = new UdpClient(mePort);
                IPEndPoint ipLocalPoint = new IPEndPoint(IPAddress.Parse(meIP), mePort);

                RemotePoint = new IPEndPoint(IPAddress.Any, devPort);

                isRunning = true;
                // 新开线程监听
                Thread thread = new Thread(new ThreadStart(this.ReceiveHandle))
                {
                    IsBackground = true
                };
                thread.Start();
                // 添加到线程池
                listThread.Add(thread);
                isOpen = true;

            }
            catch (Exception ex)
            {
                isOpen = false;
                throw new Exception(ex.Message);
            }
        }
        /// <summary>
        /// 关闭Udp服务端监听
        /// </summary>
        public void TurnOff()
        {
            try
            {
                isOpen = false;
                isRunning = false;

                for (int i = 0; i < listThread.Count; i++)
                {
                    try
                    {
                        listThread[i].Abort();
                    }
                    catch (Exception) { }
                }

                if (mySocket != null)
                {
                    mySocket.Close();
                }
            }
            catch (Exception)
            {
            }
        }

        public delegate void GetRecevice(byte[] ReceviceBuff);
        public event GetRecevice EvtGetValues; // 事件
        private void ReceiveHandle()
        {
            byte[] sendbuf = new byte[9];
            byte[] sendwritbuf = new byte[200];

            while (isRunning)
            {
                try
                {
                    if (mySocket == null || mySocket.Available < 1)
                    {
                        Thread.Sleep(300);
                        continue;
                    }
                    //接收UDP数据报，引用参数RemotePoint获得源地址  
                    byte[] buf = mySocket.Receive(ref RemotePoint);
                    if (devIP == null || devIP.Length < 1)
                    {
                        devIP = RemotePoint.Address.ToString();
                        devPort = RemotePoint.Port;
                    }
                    if (EvtGetValues != null)
                    {
                        EvtGetValues(buf);
                    }
                }
                catch (Exception)
                {
                }
            }
        }
        #endregion
    }
}
