using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using GA.SuperSocket.AppClient.Common;
using GA.SuperSocket.AppClient.RemoteFastPrintNetService;
using GA.SuperSocket.AppClient.Utility;
using GA.SuperSocket.Service.Model;

namespace GA.SuperSocket.AppClient
{
    public partial class LoginForm : Form
    {
        /// <summary>
        /// 注意：需要添加web引用，而非服务引用
        /// </summary>
        private readonly FastPrintNetService fastPrintNetClient = new FastPrintNetService();

        public LoginForm()
        {
            //初始化窗口组件
            InitializeComponent();

            //获取远程Web服务地址及初始化对象
            AppContext.AppServiceConfig = AppServiceConfigUtility.Load();
            fastPrintNetClient = new FastPrintNetService();
            fastPrintNetClient.Url = string.Concat(AppContext.AppServiceConfig.RemoteWebServiceURL.TrimEnd(new char[] { '/' }), "/FastPrintNetService.asmx");
            fastPrintNetClient.EnableDecompression = true;

            Control.CheckForIllegalCrossThreadCalls = false;
            this.Activated += (s, e) =>
            {
                this.txtUserName.Focus();
                this.ActiveControl = this.txtUserName;
            };
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string msg = string.Empty;

            Thread tdLogining = new Thread(() =>
            {
                try
                {
                    this.BeginInvoke(new MethodInvoker(() =>
                    {
                        this.btnLogin.Enabled = false;
                    }));
                    string _LoginID = string.Empty;
                    string _Password = string.Empty;
                    //验证登录框信息
                    if (VerifyInputTextBoxAreEmpty(out msg))
                    {
                        WriteToolStripMsg(msg, Color.Red);
                        return;
                    }
                    //获取控件上输入的登录用户和密码
                    _LoginID = this.txtUserName.Text.Trim();
                    _Password = this.txtPwd.Text.Trim();

                    WriteToolStripMsg("登录中,请等待...");

                    string message = string.Empty;
                    //获取服务端的所有在线客户端
                    var onlineClients = fastPrintNetClient.GetOnlineClientsInfo();
                    //若登录用户存在，表示已登录
                    if (onlineClients.ConvertToEntityList<OnlineClient>().Exists(s => s.UserID.ToLower() == _LoginID.ToLower()))
                    {
                        message = string.Format("账号[{0}]已经登录,不能重复登录!", _LoginID);
                        WriteToolStripMsg(message, Color.Red);
                    }
                    else
                    {                        
                        if (_LoginID == _Password)//这里是模拟的登录方式
                        {
                            GlobalStatic.UserId = _LoginID;
                            this.DialogResult = DialogResult.OK;
                        }
                        else
                        {
                            message = "账号或密码不正确";
                            WriteToolStripMsg(message, Color.Red);
                        }
                    }
                }
                finally
                {
                    if (this.IsHandleCreated)//指示控件是否有与他关联的句柄，如果已经为控件分配了句柄，则为 true；否则为 false。
                    {
                        this.BeginInvoke(new MethodInvoker(() =>
                        {
                            this.btnLogin.Enabled = true;
                        }));
                    }
                }
            });
            tdLogining.IsBackground = true;
            tdLogining.Start();
        }

        /// <summary>
        /// 验证输入文本框是否为空
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        private bool VerifyInputTextBoxAreEmpty(out string msg)
        {
            if (string.IsNullOrEmpty(this.txtUserName.Text))
            {
                msg = "请输入用户名...";
                this.txtUserName.Focus();
                return true;
            }
            if (string.IsNullOrEmpty(this.txtPwd.Text))
            {
                msg = "请输入密码...";
                this.txtPwd.Focus();
                return true;
            }
            msg = "";
            return false;
        }

        /// <summary>
        /// 初始化加载：该方法暂时未用到
        /// </summary>
        private void InitLoading()
        {
            Thread t = new Thread(() =>
            {
                string lastShowInfo = string.Empty;
                Stopwatch sw1 = new Stopwatch();
                sw1.Start();
                bool blFinish = true;

                try
                {
                    this.BeginInvoke(new MethodInvoker(() =>
                    {
                        this.btnLogin.Enabled = false;
                    }));

                    #region [=>（1）、检查版本更新]
                    WriteToolStripMsg("正在检查版本更新...");
                    #endregion

                    #region [=>（2）、检查服务连接状态]
                    WriteToolStripMsg("正在检查与服务器连接,请耐心等待...");

                    //=>WebService服务器
                    WriteToolStripMsg("正在检查与WebService服务器连接状态...");
                    string web_Result = string.Empty;
                    web_Result = fastPrintNetClient.GetServiceRunStatus();
                    if (web_Result == "ok")
                    {
                        ////=>DB数据库服务器
                        //WriteToolStripMsg("正在检查与数据库服务器连接状态...");
                        //string db_Result = string.Empty;
                        //db_Result = foundationService.GetDBServiceRunStatus();
                        ////=>aip服务器
                        //string api_Result = string.Empty;
                        //WriteToolStripMsg("正在检查与PHP API服务器连接状态...");
                        //api_Result = loginAPIService.GetAPIRunStatus();
                        //sw1.Stop();
                        //if (api_Result == "ok"
                        //    && db_Result == "ok")
                        //{

                        //}
                    }
                    else
                    {
                        WriteToolStripMsg(string.Format("Web服务请求拒绝:{0}", web_Result), Color.Red);
                        blFinish = false;
                        return;
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    WriteToolStripMsg("系统出错,请联系技术支持!", Color.Red);
                    this.BeginInvoke(new MethodInvoker(() =>
                    {
                        this.btnLogin.Enabled = false;
                    }));
                }
                finally
                {
                    if (this.IsHandleCreated)
                        this.BeginInvoke(new MethodInvoker(() =>
                        {
                            if (blFinish)
                            {
                                lastShowInfo = "与服务器连接正常,耗时:" + sw1.ElapsedMilliseconds + "ms";
                                WriteToolStripMsg(lastShowInfo);
                            }
                            this.BeginInvoke(new MethodInvoker(delegate()
                            {
                                this.btnLogin.Enabled = true;
                            }));
                        }));
                }
            });
            t.IsBackground = true;
            t.Start();
        }
        private void WriteToolStripMsg(string msg)
        {
            WriteToolStripMsg(msg, Color.Blue);
        }

        /// <summary>
        /// 将信息写入窗口组件的Label中展示与socket服务连接的状态信息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="color"></param>
        private void WriteToolStripMsg(string msg, Color color)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new MethodInvoker(delegate()
                {
                    toolStripMsg.Text = msg;
                    toolStripMsg.ForeColor = color;
                }));
            }
            else
            {
                toolStripMsg.Text = msg;
                toolStripMsg.ForeColor = color;
            }
        }
    }
}
