using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Google.Apis.Calendar.v3.Data;
using googleCalendarLibrary;
using System.IO;

namespace googleCalendarWinForm
{
    public partial class Form1 : Form
    {

        controlCalendarcs controlCalendarObj = new controlCalendarcs();
        WriteEvent.WrittingEventLog writtingEventLogObj = new WriteEvent.WrittingEventLog();

        
        string _StartIkey = "";
        string _EndIkey = "";
        string _CooperPath = "";

        public Form1()
        {
            InitializeComponent();
        }

        public Form1(string strCooperParameter)
        {
            string[] strParameters = strCooperParameter.ToString().Split(',');

            if (strParameters.Count() > 0)
            {
                _StartIkey = strParameters[0];
                _EndIkey = strParameters[1];
                _CooperPath = strParameters[2];
            }
           
            InitializeComponent();
        }
       
        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                int intStartIkey = 0;
                int intEndIkey = 0;
                int.TryParse(_StartIkey, out intStartIkey);
                int.TryParse(_EndIkey, out intEndIkey);
                controlCalendarObj.start(_CooperPath, intStartIkey, intEndIkey);
                this.Close();
            }
            catch (Exception ex)
            {
                writtingEventLogObj.writeToFile(DateTime.Today.ToString("yyyyMMdd") + "_calendarError", Directory.GetCurrentDirectory(), "LINE 46----errorStart--CooperPath" + _CooperPath + " startIkey:" + _StartIkey  + "endIkey"+_EndIkey + ex.Message + DateTime.Now);
                this.Close();
                //writtingEventLogObj.writeToFile(ex.Message);
                //try
                //{
                //    writtingEventLogObj.writeToFile(DateTime.Today.ToString("yyyyMMdd") + "_calendarError", Directory.GetCurrentDirectory(), "LINE 50 " + DateTime.Now);
                //    Clipboard.Clear();
                //    writtingEventLogObj.writeToFile(DateTime.Today.ToString("yyyyMMdd") + "_calendarError", Directory.GetCurrentDirectory(), "LINE 52 " + DateTime.Now);
                //    Clipboard.SetText("有錯誤發生請檢查日誌檔 CooperServiceErrorLog.txt. 錯誤:" + ex.Message + controlCalendarObj.strClipboardText);
                //}
                //catch 
                //{
                //    writtingEventLogObj.writeToFile(DateTime.Today.ToString("yyyyMMdd") + "_calendarError", Directory.GetCurrentDirectory(), "LINE 57 " + DateTime.Now);
                //    this.Close();
                //}
                //writtingEventLogObj.writeToFile(DateTime.Today.ToString("yyyyMMdd") + "_calendarError", Directory.GetCurrentDirectory(), "LINE 60---- errorEnd--- "+DateTime.Now);
                //this.Close();
            }
        }
    }
}
