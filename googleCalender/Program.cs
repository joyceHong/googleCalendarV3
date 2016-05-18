using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace googleCalendarWinForm
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length > 0)
            {
                Application.Run(new Form1(args[0].ToString()));
            }
            else
            {
                Application.Run(new Form1());
            }
            //Application.Run(new Form1());
        }
    }
}
