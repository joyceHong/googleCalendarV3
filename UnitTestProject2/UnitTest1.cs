using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using googleCalendarLibrary;
using System.Diagnostics;
using System.Windows.Forms;
using System.Data;
namespace UnitTestProject2
{
    [TestClass]
    public class UnitTest1
    {
        controlCalendarcs controlCalendarObj = new controlCalendarcs();

        [TestMethod]
        public void TestMethod1()
        {
           
            //DateTime strChinaDate = controlCalendarObj.strConvertToChinaDate("1031107", "0810");

            //期望值
            DateTime dtExpressDateTime = new DateTime(2014, 11, 7, 8, 10, 0);
            //Assert.AreEqual(dtExpressDateTime, strChinaDate);
        }


        [TestMethod]
        public void TestMethod2()
        {

            entryGoogleData test = new entryGoogleData();

            test.intIkey = 1;
            test.strSendMessage = "testSendMessage";
            test.strSendResult = "Y";
            test.saveGoogleMsg();

            //Process p = new Process();
            //p.StartInfo.FileName = @"C:\Users\RDCP01\Desktop\新增資料夾\test\Debug\googleCalender.exe";
            //string CRLF = (string)(char.ConvertFromUtf32(13) + char.ConvertFromUtf32(10));

            //for (int i = 1; i <= 10; i++)
            //{
            //    Clipboard.SetText("BSSOT" + CRLF + "1" + CRLF + "joyce@gw1.drcooper.com.tw" + CRLF + "12345678" + CRLF + "201411081" + i + CRLF + "sampleSummary_update_123456" + CRLF + "1031109" + CRLF + "1000" + CRLF + "1031109" + CRLF + "1100" + CRLF + "location" + CRLF + " description" + CRLF + "1");
            //    p.Start();
            //}

           
        }

        [TestMethod]
        public void testTethod3()
        {
            entryGoogleData test = new entryGoogleData();

            test.intIkey = 1;
            test.strSendMessage = "testSendMessage";
            test.strSendResult = "Y";            
            test.saveGoogleMsg();
        }
    }
}
