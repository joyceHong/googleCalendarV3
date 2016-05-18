using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using googleCalendarHelp;
namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            googleCalendarHelp.googleCalendarHelp googleCalendarObj = new googleCalendarHelp.googleCalendarHelp();

            viewCalendar viewCalendarObj = new viewCalendar()
            {
                 strDesc="測試v3google 行事曆",
                  strEventColor="1",
                   strEventID="joyce",
                    strLocation="google v3 場所測試",
                 strSummary = "google v3 summary 主旨測試"
            };

            

            //googleCalendarObj.viewCalendarObj
            //googleCalendarObj.addEvent();

        }
    }
}
