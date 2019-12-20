using GA.SuperSocket.AppClient.Common;
using GA.SuperSocket.AppClient.RemoteFastPrintNetService;
using GA.SuperSocket.AppClient.Utility;
using SuperSocket.ClientEngine;
using SuperSocket.ProtoBase;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GA.SuperSocket.AppClient
{
    public partial class MessageMainForm : Form
    {
        private EasyClient<StringPackageInfo> tcpPassiveEngine = null;
        private const string Client_Unique_Key = "ClientUniqueID";
        private FastPrintNetService fastPrintNetClient;
        private bool WebServiceConneted = false;
        private static object objLock = new object();
        private System.Threading.Timer tmrHeartBeat = null;
        private int mHeartBeatInterval = 1000 * 10;

        public MessageMainForm()
        {
            //初始化窗口组件
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            //显示登录账号信息
            this.notifyIcon.Text = string.Format("登录账号:[{0}]", GlobalStatic.UserId);
            this.Text = string.Format("{0}—【{1}】", this.Text, GlobalStatic.UserId);
            RegisterFormEvents();
            try
            {
                //开启心跳数据包
                tmrHeartBeat = new System.Threading.Timer(HeartBeatCallBack, null, mHeartBeatInterval, mHeartBeatInterval);
                //初始化远程服务对象
                fastPrintNetClient = new FastPrintNetService();
                fastPrintNetClient.Url = string.Concat(AppContext.AppServiceConfig.RemoteWebServiceURL.TrimEnd(new char[] { '/' }), "/FastPrintNetService.asmx");
                fastPrintNetClient.EnableDecompression = true;
                fastPrintNetClient.GetServiceRunStatus();//检查接口状态
                this.WebServiceConneted = true;
            }
            catch (WebException wex)
            {
                if (wex.Status == WebExceptionStatus.ConnectFailure
                     || wex.Status == WebExceptionStatus.ProtocolError
                     || wex.Status == WebExceptionStatus.NameResolutionFailure)
                {
                    WriteToolStripMsg("与WebSevice服务器连接失败,请联系技术支持!", Color.Red);
                    this.WebServiceConneted = false;
                    this.tmrServiceRunStatus.Enabled = true;
                    this.tmrServiceRunStatus.Start();
                }
            }
            catch (Exception ex)
            {
                WriteToolStripMsg("系统出错,请联系技术支持!", Color.Red);
                this.WebServiceConneted = false;
                this.tmrServiceRunStatus.Enabled = true;
                this.tmrServiceRunStatus.Start();
            }
        }

        private void HeartBeatCallBack(object state)
        {
            try
            {
                tmrHeartBeat.Change(Timeout.Infinite, Timeout.Infinite);
                if (tcpPassiveEngine != null && tcpPassiveEngine.IsConnected)//与客户端连接成功后发送消息
                {
                    var sbMessage = new StringBuilder();
                    sbMessage.AppendFormat(string.Format("heartbeat {0}\r\n", "心跳数据包:ok"));
                    var data = Encoding.UTF8.GetBytes(sbMessage.ToString());
                    tcpPassiveEngine.Send(new ArraySegment<byte>(data, 0, data.Length));
                }
            }
            finally
            {
                tmrHeartBeat.Change(mHeartBeatInterval, mHeartBeatInterval);
            }
        }

        private static MessageMainForm _Instance;

        /// <summary>
        /// MainForm主窗体实例
        /// </summary>
        public static MessageMainForm Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new MessageMainForm();
                }
                return _Instance;
            }
        }

        public string AssemblyProduct
        {
            get
            {
                object[] attributes = Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyProductAttribute), false);
                if (attributes.Length == 0)
                {
                    return "";
                }
                return ((AssemblyProductAttribute)attributes[0]).Product;
            }
        }

        public string AssemblyVersion
        {
            get
            {
                return FileVersionInfo.GetVersionInfo(System.Windows.Forms.Application.ExecutablePath).FileVersion;
                //return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }

        /// <summary>
        /// 注册窗体事件
        /// </summary>
        private void RegisterFormEvents()
        {
            this.SizeChanged += (s, e) =>
            {
                if (this.WindowState == FormWindowState.Minimized)
                {
                    this.Hide();
                    this.ShowInTaskbar = false;
                    this.notifyIcon.Visible = true;
                }
            };
            this.FormClosing += (s, e) =>
            {
                e.Cancel = true;// 取消关闭窗体                             
                this.Hide();
                this.ShowInTaskbar = false;
                this.notifyIcon.Visible = true;
            };
        }

        /// <summary>
        /// Socekt服务器EndPoint
        /// </summary>
        public IPEndPoint SocketServerEndPoint
        {
            get
            {
                string ip = AppContext.AppServiceConfig.Socket_Server_Address.Split(new char[] { ':' })[0];
                string port = AppContext.AppServiceConfig.Socket_Server_Address.Split(new char[] { ':' })[1];
                IPAddress ipAddress;
                System.Net.IPAddress.TryParse(ip, out ipAddress);
                if (ipAddress == null)//如果不是有效的数字IP地址，则通过域名去解析。
                {
                    ipAddress = GetIPAddress(ip);
                }
                IPEndPoint _IPEndPoint = new IPEndPoint(ipAddress, int.Parse(port));
                return _IPEndPoint;
            }
        }


        ///<summary>
        /// 传入域名返回对应的IP
        ///</summary>
        ///<param name="domain">域名</param>
        ///<returns></returns>
        public static string GetIP(string domain)
        {
            domain = domain.Replace("http://", "").Replace("https://", "");
            IPHostEntry hostEntry = Dns.GetHostEntry(domain);
            IPEndPoint ipEndPoint = new IPEndPoint(hostEntry.AddressList[0], 0);
            return ipEndPoint.Address.ToString();
        }

        ///<summary>
        /// 传入域名返回对应的IP
        ///</summary>
        ///<param name="domain">域名</param>
        ///<returns></returns>
        public static IPAddress GetIPAddress(string domain)
        {
            domain = domain.Replace("http://", "").Replace("https://", "");
            IPHostEntry hostEntry = Dns.GetHostEntry(domain);
            IPEndPoint ipEndPoint = new IPEndPoint(hostEntry.AddressList[0], 0);
            return ipEndPoint.Address;
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                if (WebServiceConneted == false) return;
                tcpPassiveEngine = new EasyClient<StringPackageInfo>()
                {
                    ReceiveBufferSize = 65535
                };
                //初始化socket服务
                tcpPassiveEngine.Initialize(new MyTerminatorReceiveFilter());
                //绑定会话连接方法
                tcpPassiveEngine.Connected += tcpPassiveEngine_Connected;
                //绑定客户端接收数据（即服务端发送数据）方法
                tcpPassiveEngine.NewPackageReceived += tcpPassiveEngine_NewPackageReceived;
                //绑定socket会话关闭方法
                tcpPassiveEngine.Closed += tcpPassiveEngine_Closed;
                //绑定socket会话异常方法
                tcpPassiveEngine.Error += tcpPassiveEngine_Error;
                //发起对ip端口号的请求连接
                await tcpPassiveEngine.ConnectAsync(new DnsEndPoint(SocketServerEndPoint.Address.ToString(), SocketServerEndPoint.Port));
               
                SendHearBeatMessageToServer();
                this.colTime.Width = 300;
                this.colMessage.Width = 750;
            }
            catch (SocketException sex)
            {
                string errorMsg = string.Format("与通讯[{0}]服务器连接出现异常,请联系技术支持!", SocketServerEndPoint);
                WriteToolStripMsg(errorMsg, Color.Red);
                NLogHelper.Instance.Error(errorMsg);
            }
        }

        /// <summary>
        /// 客户端接受数据时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tcpPassiveEngine_NewPackageReceived(object sender, PackageEventArgs<StringPackageInfo> e)
        {
            try
            {
                switch (e.Package.Key.ToLower())
                {
                    case "heartbeat":
                        string message = string.Format("接受到服务端的心跳数据包:{0}", e.Package.Body);
                        ShowMessageInfo(message);
                        break;
                    case "push":
                        string msg = string.Format("当前消息数据为:{0}", e.Package.Body);
                        ShowMessageInfo(msg);
                        NLogHelper.Instance.Info(msg);
                        break;
                    case "welcome":
                        break;
                    case "unknown":
                        break;
                    case "error":
                        break;
                    default:
                        ShowMessageInfo(string.Format("未知的指令（error unknow command）:{0}", e.Package.Key + " " + e.Package.Body));
                        break;
                }
            }
            catch (Exception ex)
            {
                string errorMessage = string.Format("出错-{0}", ex.Message);
                ShowMessageInfo(errorMessage);
                NLogHelper.Instance.Error(errorMessage);
            }
        }


        /// <summary>
        /// 异常事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tcpPassiveEngine_Error(object sender, ErrorEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler<ErrorEventArgs>(this.tcpPassiveEngine_Error), new object[] { sender, e });
            }
            else
            {
                WriteToolStripMsg(string.Format("与通讯[{0}]服务器连接失败,请联系技术支持!", this.SocketServerEndPoint.ToString()), Color.Red);
            }
        }

        /// <summary>
        /// 连接成功事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tcpPassiveEngine_Connected(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler(this.tcpPassiveEngine_Connected), new object[] { sender, e });
            }
            else
            {
                this.tmrServiceRunStatus.Enabled = false;
                this.tmrServiceRunStatus.Stop();
                ShowSocketServerConnectStatus();
            }
        }

        /// <summary>
        /// 断开连接关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tcpPassiveEngine_Closed(object sender, EventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new EventHandler(this.tcpPassiveEngine_Closed), new object[] { sender, e });
            }
            else
            {
                this.tmrServiceRunStatus.Enabled = true;
                this.tmrServiceRunStatus.Start();
                ShowSocketServerConnectStatus();
                this.tcpPassiveEngine.Close();
                this.tcpPassiveEngine = null;
            }
        }


        /// <summary>
        /// 显示与服务器连接信息
        /// </summary>
        private void ShowSocketServerConnectStatus()
        {
            if (tslServerConnectionMsg == null) return;
            this.tslServerConnectionMsg.Text = string.Format("与[{0}]服务器", this.SocketServerEndPoint.ToString());
            this.tslServerConnectionMsg.ForeColor = Color.Blue;
            this.tslServerStatusText.Text = this.tcpPassiveEngine.IsConnected == true ? "成功" : "失败";
            if (this.tcpPassiveEngine.IsConnected == false) this.tslServerStatusText.ForeColor = Color.Red;
            else this.tslServerStatusText.ForeColor = Color.Blue;
            this.ShowMessageInfo(string.Format("与[{0}]服务器:{1}", this.SocketServerEndPoint.ToString(),
                                                                    this.tcpPassiveEngine.IsConnected == true ? "成功√√√" : "失败×××")
                                 );
        }

        /// <summary>
        /// 客户端向服务器发送唯一身份数据
        /// </summary>
        private void SendHearBeatMessageToServer()
        {
            if (tcpPassiveEngine != null && tcpPassiveEngine.IsConnected)
            {
                var sbMessage = new StringBuilder();
                sbMessage.AppendFormat(string.Format("{0} {1}", Client_Unique_Key, GlobalStatic.UserId + Environment.NewLine));
                var data = Encoding.UTF8.GetBytes(sbMessage.ToString());
                tcpPassiveEngine.Send(new ArraySegment<byte>(data, 0, data.Length));
            }
        }

        /// <summary>
        /// 显示主界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tspShowMain_Click(object sender, EventArgs e)
        {
            this.Show();
            this.ShowInTaskbar = true;
            this.notifyIcon.Visible = false;
        }

        /// <summary>
        /// 重新启动客户端
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tspRestart_Click(object sender, EventArgs e)
        {
            if (this.tcpPassiveEngine != null)
                this.tcpPassiveEngine.Close();
            this.Dispose(true);
            Application.Restart();
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tspExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确定退出系统吗?", "提示",
               MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (this.tcpPassiveEngine != null)
                    this.tcpPassiveEngine.Close();
                this.notifyIcon.Visible = false;
                this.Dispose(true);
                Application.ExitThread();
            }
        }

        /// <summary>
        /// 鼠标点击弹出事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void notifyIcon_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Show();
                this.ShowInTaskbar = true;
                this.notifyIcon.Visible = false;
                this.WindowState = FormWindowState.Normal;
            }
        }

        /// <summary>
        /// 检查WebService服务器状态(激活webservice服务器)。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tmrServiceRunStatus_Tick(object sender, EventArgs e)
        {
            try
            {
                if (this.tcpPassiveEngine == null
                    || this.tcpPassiveEngine.IsConnected == false)
                {
                    fastPrintNetClient.GetServiceRunStatus();
                    MainForm_Load(null, null);
                }
                else
                {
                    this.tmrServiceRunStatus.Enabled = false;
                    this.tmrServiceRunStatus.Stop();
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Instance.Error(ex.Message);
            }
        }

        private void WriteToolStripMsg(string msg)
        {
            WriteToolStripMsg(msg, Color.Blue);
        }

        private void WriteToolStripMsg(string msg, Color color)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbDelegate<string>(this.WriteToolStripMsg), msg, color);
            }
            else
            {
                this.tslServerConnectionMsg.Text = msg;
                this.tslServerConnectionMsg.ForeColor = color;
            }
        }

        public delegate void CbDelegate<T>(T obj);

        private void ShowMessageInfo(string msg)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new CbDelegate<string>(this.ShowMessageInfo), msg);
            }
            else
            {
                if (this.lvMessageInfo.Items.Count >= 1000) this.lvMessageInfo.Items.Clear();
                ListViewItem item = new ListViewItem(new string[] { DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss fffffff"), msg });
                this.lvMessageInfo.Items.Insert(0, item);
            }
        } 
    }
}
