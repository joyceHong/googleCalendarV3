using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace googleCalendarLibrary
{
    public class viewCalendar
    {

        public string strEventID
        {
            get;
            set;
        }

       

        public string strSummary
        {
            get;
            set;
        }

        public string strGmailAccount
        {
            get;
            set;
        }

        public string strGmailPassword
        {
            get;
            set;
        }

       

        public DateTime timeStart
        {
            get;
            set;
        }

        public DateTime timeEnd
        {
            get;
            set;
        }

        /// <summary>
        ///場所
        /// </summary>
        public string strLocation
        {
            get;
            set;
        }

        /// <summary>
        ///描述
        /// </summary>
        public string strDesc
        {
            get;
            set;
        }

        /// <summary>
        ///事件顏色
        /// </summary>
        public  string strEventColor
        {
            get;
            set;
        }
    }
}
