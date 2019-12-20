using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ICQClient
{
    /// <summary>
    /// 用于连接服务器获取密钥的类 使用了第三方库解析Json
    /// </summary>
    class Connection
    {
        
        private string uname;
        private string tname;
        private string key;
        private string statusCode;
        private string url = "http://localhost/getSessionKey";
        public string Uname { get { return uname; } }
        public string Tname { get { return tname; } }
        public string Key { get { return key; } }
        public string Code { get { return statusCode; } }


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="uname">你的用户名</param>
        /// <param name="tname">要聊天对象的用户名</param>
        /// <param name="u">服务器链接</param>
        public Connection(string uname, string tname,string u)
        {
            this.uname = uname;
            this.tname = tname;
            url = u;
        }


        /// <summary>
        /// 请求服务器获取会话密钥
        /// </summary>
        public void Req()
        {
            string post = "uname=" + uname + "&tname=" + tname;
            HttpWebResponse res = Requests.Post(url, Data: post, AllowRedirect: true);
            string result = Requests.GetResponseText(res);
            JObject json = JsonConvert.DeserializeObject<JObject>(result);
            //string result = "{\"key\":\"390231c23c\"}";
            statusCode = json.GetValue("c").ToString();
            if (statusCode == "RepeatError") throw new Exception("用户名可能有重复");
            if (statusCode == "UnkownError") throw new Exception("未知错误");
            uname = json.GetValue("u").ToString();
            tname = json.GetValue("t").ToString();
            key = json.GetValue("s").ToString();

            //string asyncKey = result.Split('\"')[3];


        }
    }
}
