namespace GA.SuperSocket.AppClient
{
    partial class MessageMainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MessageMainForm));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lvMessageInfo = new System.Windows.Forms.ListView();
            this.colTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.tslServerConnectionMsg = new System.Windows.Forms.ToolStripLabel();
            this.tslServerStatusText = new System.Windows.Forms.ToolStripLabel();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tspShowMain = new System.Windows.Forms.ToolStripMenuItem();
            this.tspRestart = new System.Windows.Forms.ToolStripMenuItem();
            this.tspExit = new System.Windows.Forms.ToolStripMenuItem();
            this.tmrServiceRunStatus = new System.Windows.Forms.Timer(this.components);
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.groupBox1.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.contextMenuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lvMessageInfo);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(948, 496);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "消息";
            // 
            // lvMessageInfo
            // 
            this.lvMessageInfo.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colTime,
            this.colMessage});
            this.lvMessageInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvMessageInfo.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lvMessageInfo.FullRowSelect = true;
            this.lvMessageInfo.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
            this.lvMessageInfo.Location = new System.Drawing.Point(4, 32);
            this.lvMessageInfo.Margin = new System.Windows.Forms.Padding(4);
            this.lvMessageInfo.Name = "lvMessageInfo";
            this.lvMessageInfo.ShowItemToolTips = true;
            this.lvMessageInfo.Size = new System.Drawing.Size(940, 460);
            this.lvMessageInfo.TabIndex = 10;
            this.lvMessageInfo.UseCompatibleStateImageBehavior = false;
            this.lvMessageInfo.View = System.Windows.Forms.View.Details;
            // 
            // colTime
            // 
            this.colTime.Text = "时间";
            this.colTime.Width = 300;
            // 
            // colMessage
            // 
            this.colMessage.Text = "消息内容";
            this.colMessage.Width = 638;
            // 
            // toolStrip
            // 
            this.toolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStrip.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslServerConnectionMsg,
            this.tslServerStatusText});
            this.toolStrip.Location = new System.Drawing.Point(0, 465);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Padding = new System.Windows.Forms.Padding(0, 0, 2, 0);
            this.toolStrip.Size = new System.Drawing.Size(948, 31);
            this.toolStrip.TabIndex = 11;
            this.toolStrip.Text = "toolStrip1";
            // 
            // tslServerConnectionMsg
            // 
            this.tslServerConnectionMsg.Name = "tslServerConnectionMsg";
            this.tslServerConnectionMsg.Size = new System.Drawing.Size(30, 28);
            this.tslServerConnectionMsg.Text = "--";
            // 
            // tslServerStatusText
            // 
            this.tslServerStatusText.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tslServerStatusText.Name = "tslServerStatusText";
            this.tslServerStatusText.Size = new System.Drawing.Size(30, 28);
            this.tslServerStatusText.Text = "--";
            // 
            // notifyIcon
            // 
            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
            this.notifyIcon.Text = "电脑";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseClick);
            // 
            // contextMenuStrip
            // 
            this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tspShowMain,
            this.tspRestart,
            this.tspExit});
            this.contextMenuStrip.Name = "contextMenuStrip";
            this.contextMenuStrip.Size = new System.Drawing.Size(171, 88);
            // 
            // tspShowMain
            // 
            this.tspShowMain.Image = ((System.Drawing.Image)(resources.GetObject("tspShowMain.Image")));
            this.tspShowMain.Name = "tspShowMain";
            this.tspShowMain.Size = new System.Drawing.Size(170, 28);
            this.tspShowMain.Text = "显示主界面";
            // 
            // tspRestart
            // 
            this.tspRestart.Image = ((System.Drawing.Image)(resources.GetObject("tspRestart.Image")));
            this.tspRestart.Name = "tspRestart";
            this.tspRestart.Size = new System.Drawing.Size(170, 28);
            this.tspRestart.Text = "重新启动";
            // 
            // tspExit
            // 
            this.tspExit.Image = ((System.Drawing.Image)(resources.GetObject("tspExit.Image")));
            this.tspExit.Name = "tspExit";
            this.tspExit.Size = new System.Drawing.Size(170, 28);
            this.tspExit.Text = "退出";
            this.tspExit.Click += new System.EventHandler(this.tspExit_Click);
            // 
            // tmrServiceRunStatus
            // 
            this.tmrServiceRunStatus.Interval = 10000;
            this.tmrServiceRunStatus.Tick += new System.EventHandler(this.tmrServiceRunStatus_Tick);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 24);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // MessageMainForm
            // 
            this.ClientSize = new System.Drawing.Size(948, 496);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MessageMainForm";
            this.Text = "基于SuperSocket实现网页与客户端通信实战项目演练";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.contextMenuStrip.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListView lvMessageInfo;
        private System.Windows.Forms.ColumnHeader colTime;
        private System.Windows.Forms.ColumnHeader colMessage;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem tspShowMain;
        private System.Windows.Forms.ToolStripMenuItem tspRestart;
        private System.Windows.Forms.ToolStripMenuItem tspExit;
        private System.Windows.Forms.Timer tmrServiceRunStatus;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripLabel tslServerConnectionMsg;
        private System.Windows.Forms.ToolStripLabel tslServerStatusText;

    }
}
