using Google.Apis.Calendar.v3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace googleCalendarLibrary
{
    public interface IgoogleCalendarHelp
    {
        bool addEvent(viewCalendar viewCalendarObj);
        bool updateEvent(viewCalendar viewCalendarObj);
        bool deleteEvent(string strEventID);
        Event getEvent(string strEventID);     
    }
}
