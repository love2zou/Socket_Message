using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GA.SuperSocket.AppClient
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            LoginForm lg = new LoginForm();
            if (lg.ShowDialog() == DialogResult.OK)//如果登录成功，进入到下一个窗口
            {
                lg.Close();//关闭登录框
                Application.Run(new MessageMainForm());
            }
        }
    }
}
