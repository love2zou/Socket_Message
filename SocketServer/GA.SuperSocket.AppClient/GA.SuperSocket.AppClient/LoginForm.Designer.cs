namespace GA.SuperSocket.AppClient
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.lblContent = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtUserName = new System.Windows.Forms.TextBox();
            this.txtPwd = new System.Windows.Forms.TextBox();
            this.picLogin = new System.Windows.Forms.PictureBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.tspStatusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripMsg = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.picLogin)).BeginInit();
            this.tspStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblContent
            // 
            this.lblContent.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblContent.ForeColor = System.Drawing.Color.Green;
            this.lblContent.Location = new System.Drawing.Point(63, 11);
            this.lblContent.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(617, 60);
            this.lblContent.TabIndex = 6;
            this.lblContent.Text = "基于SuperSocket实现网页与客户端通信实战项目演练";
            this.lblContent.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Green;
            this.label2.Location = new System.Drawing.Point(79, 189);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 36);
            this.label2.TabIndex = 8;
            this.label2.Text = "密  码:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Green;
            this.label1.Location = new System.Drawing.Point(87, 121);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 36);
            this.label1.TabIndex = 7;
            this.label1.Text = "账 号:";
            // 
            // txtUserName
            // 
            this.txtUserName.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtUserName.ForeColor = System.Drawing.Color.Green;
            this.txtUserName.Location = new System.Drawing.Point(189, 118);
            this.txtUserName.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtUserName.Name = "txtUserName";
            this.txtUserName.Size = new System.Drawing.Size(467, 42);
            this.txtUserName.TabIndex = 9;
            this.txtUserName.Text = "admin";
            // 
            // txtPwd
            // 
            this.txtPwd.Font = new System.Drawing.Font("微软雅黑", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPwd.ForeColor = System.Drawing.Color.Green;
            this.txtPwd.Location = new System.Drawing.Point(191, 189);
            this.txtPwd.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtPwd.Name = "txtPwd";
            this.txtPwd.PasswordChar = '*';
            this.txtPwd.Size = new System.Drawing.Size(467, 42);
            this.txtPwd.TabIndex = 10;
            this.txtPwd.Text = "admin";
            // 
            // picLogin
            // 
            this.picLogin.Dock = System.Windows.Forms.DockStyle.Top;
            this.picLogin.Image = global::GA.SuperSocket.AppClient.Properties.Resources.system;
            this.picLogin.Location = new System.Drawing.Point(0, 0);
            this.picLogin.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.picLogin.Name = "picLogin";
            this.picLogin.Size = new System.Drawing.Size(703, 80);
            this.picLogin.TabIndex = 5;
            this.picLogin.TabStop = false;
            // 
            // btnLogin
            // 
            this.btnLogin.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogin.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLogin.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnLogin.Location = new System.Drawing.Point(135, 259);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(0);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(137, 61);
            this.btnLogin.TabIndex = 14;
            this.btnLogin.Text = "登 录(&O)";
            this.btnLogin.UseVisualStyleBackColor = true;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Location = new System.Drawing.Point(503, 259);
            this.btnClose.Margin = new System.Windows.Forms.Padding(0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(137, 61);
            this.btnClose.TabIndex = 15;
            this.btnClose.Text = "关 闭(&E)";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // tspStatusStrip
            // 
            this.tspStatusStrip.BackColor = System.Drawing.Color.Transparent;
            this.tspStatusStrip.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tspStatusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tspStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMsg});
            this.tspStatusStrip.Location = new System.Drawing.Point(0, 379);
            this.tspStatusStrip.Name = "tspStatusStrip";
            this.tspStatusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 19, 0);
            this.tspStatusStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.tspStatusStrip.Size = new System.Drawing.Size(703, 22);
            this.tspStatusStrip.TabIndex = 13;
            // 
            // toolStripMsg
            // 
            this.toolStripMsg.Name = "toolStripMsg";
            this.toolStripMsg.Size = new System.Drawing.Size(0, 17);
            // 
            // LoginForm
            // 
            this.AcceptButton = this.btnLogin;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(703, 401);
            this.Controls.Add(this.tspStatusStrip);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.txtPwd);
            this.Controls.Add(this.txtUserName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblContent);
            this.Controls.Add(this.picLogin);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "基于SuperSocket实现网页与客户端通信实战项目演练";
            ((System.ComponentModel.ISupportInitialize)(this.picLogin)).EndInit();
            this.tspStatusStrip.ResumeLayout(false);
            this.tspStatusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picLogin;
        private System.Windows.Forms.Label lblContent;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtUserName;
        private System.Windows.Forms.TextBox txtPwd;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.StatusStrip tspStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripMsg;
    }
}

