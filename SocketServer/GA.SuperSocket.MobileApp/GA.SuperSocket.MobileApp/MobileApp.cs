using GA.SuperSocket.MobileApp.RemoteFastPrintNetService;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GA.SuperSocket.MobileApp
{
    public partial class MobileApp : Form
    {
        private FastPrintNetService fastPrintNetClient = new FastPrintNetService();

        public MobileApp()
        {
            InitializeComponent();
            AppContext.AppServiceConfig = AppServiceConfigUtility.Load();
            fastPrintNetClient = new FastPrintNetService();
            fastPrintNetClient.Url = string.Concat(AppContext.AppServiceConfig.RemoteWebServiceURL.TrimEnd(new char[] { '/' }), "/FastPrintNetService.asmx");
            fastPrintNetClient.EnableDecompression = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MobileApp_Load(object sender, EventArgs e)
        {
            picRefresh_Click(null, null);
        }
        /// <summary>
        /// 刷新在线用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picRefresh_Click(object sender, EventArgs e)
        {
            this.cmbOnlieClient.Items.Clear();
            foreach (var item in fastPrintNetClient.GetOnlineClientsInfo())
            {
                this.cmbOnlieClient.Items.Add(item.UserID);
            }
            if (this.cmbOnlieClient.Items.Count > 0) this.cmbOnlieClient.SelectedIndex = 0;
        }
        /// <summary>
        /// 给指定用户发送数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            var responseResult = fastPrintNetClient.SendMessage(this.cmbOnlieClient.Text, this.txtMessage.Text);
            if (responseResult.RequestStatus == 100)
            {
                MessageBox.Show("发送成功！！！");
            }
            else
            {
                MessageBox.Show(responseResult.Msg);
            }
        }
    }
}
