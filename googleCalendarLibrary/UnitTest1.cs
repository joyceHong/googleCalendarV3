using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace googleCalendarLibrary
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            try
            {
                

                viewCalendar viewCalendarObj = new viewCalendar()
                {
                    strDesc = "aaa",
                    strEventColor = "1",
                    strEventID = "joyce",
                    strLocation = "joyce location",
                    strSummary = "joyce summary",
                    timeStart = DateTime.Now,
                    timeEnd = DateTime.Now.AddHours(2),
                    strGmailAccount="joycehong0827@gmail.com"
                };

              
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
        }
    }
}
