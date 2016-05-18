using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Responses;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Diagnostics;
//using System.Linq;
//using System.Text;
using System.Threading;
using WriteEvent;

namespace googleCalendarLibrary
{
    public class googleCalendarTaken :IgoogleCalendarHelp
    {
        public CalendarService _service;

        WrittingEventLog writeObj = new WriteEvent.WrittingEventLog();
        
        public googleCalendarTaken(string strGmailAccount)
        {
            startBuild(strGmailAccount);
        }

        public void startBuild(string strUser)
        {
          
            string strTestLine = "line 26 ";
            //writeObj.writeToFile(strTestLine);

            UserCredential credential;
            try
            {
                strTestLine += ", line 30";                
                
                //請記得修改成自已的google行事曆的v3 帳號
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                new ClientSecrets
                {
                    ClientId = "apps.googleusercontent.com",
                    ClientSecret = "Axlp",
                },
               new[] { CalendarService.Scope.Calendar },
               strUser, CancellationToken.None, new FileDataStore("Calendar.Auth.Store")).Result;

                strTestLine += ", line 40";
                //writeObj.writeToFile(strTestLine);
                BaseClientService.Initializer initializer = new BaseClientService.Initializer();

                strTestLine += ", line 44";
                //writeObj.writeToFile(strTestLine);
                initializer.HttpClientInitializer = credential;
                initializer.ApplicationName = "Cooper"; //申請API Project Name   

                strTestLine += ", line 49";
                //writeObj.writeToFile(strTestLine);
                _service = new CalendarService(initializer);

                strTestLine += ", line 53";
                //writeObj.writeToFile(strTestLine);
            }
            catch (Exception ex)
            {
                //throw new Exception("startBuild 失敗" + ex.Message);

                // Get stack trace for the exception with source file information
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                throw new Exception("startBuild 失敗"+line.ToString() +"行號 " + ex.Message+"行數:"+ strTestLine);
            }
        }

        public bool addEvent(viewCalendar viewCalendarObj)
        {
            try
            {
               var  strTestLine = ", line 81  新增之前";
                //writeObj.writeToFile(strTestLine);

                Event eventGoogle = new Event()
                {
                    Kind = "calendar#event",
                    Id = viewCalendarObj.strEventID,
                    Summary = viewCalendarObj.strSummary,
                    Location = viewCalendarObj.strLocation,
                    Description = viewCalendarObj.strDesc,                    
                    Start = new EventDateTime()
                    {
                        DateTime = viewCalendarObj.timeStart,
                    },

                    End = new EventDateTime()
                    {
                        DateTime = viewCalendarObj.timeEnd,
                    },                   
                };

                _service.Events.Insert(eventGoogle, "primary").Execute();

                strTestLine = "新增之後";
                //writeObj.writeToFile(strTestLine);

                Event googleEventObj = getEvent(viewCalendarObj.strEventID);
                
                if (googleEventObj != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("addEvent  新增事件失敗：" + ex.Message);
            }
        }

        /// <summary>
        /// 刪除google 行事曆事件
        /// </summary>
        /// <param name="strCalendarID">google 行事曆的名稱</param>
        /// <param name="strEventID">google calendar's id</param>
        public bool deleteEvent(string strEventID)
        {
            try
            {
                _service.Events.Delete("primary", strEventID).Execute();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception("deleteEvent 刪除失敗" + ex.Message);
            }
        }

        /// <summary>
        /// 更新行事曆
        /// </summary>
        /// <param name="strEventID"></param>
        /// <param name="viewCalendarObj"></param>
        /// <returns></returns>
        public bool updateEvent(viewCalendar viewCalendarObj)
        {
            try
            {
                var strTestLine = "update Event之前";
                //writeObj.writeToFile(strTestLine);

                // First retrieve the event from the API.
                Event googleEventObj = _service.Events.Get("primary", viewCalendarObj.strEventID).Execute();

                if (!string.IsNullOrEmpty(viewCalendarObj.strSummary))
                    googleEventObj.Summary = viewCalendarObj.strSummary;

                if (!string.IsNullOrEmpty(viewCalendarObj.strLocation))
                    googleEventObj.Location = viewCalendarObj.strLocation;

                if (!string.IsNullOrEmpty(viewCalendarObj.strDesc))
                    googleEventObj.Description = viewCalendarObj.strDesc;

                if (viewCalendarObj.timeStart != null)
                {
                    googleEventObj.Start = new EventDateTime()
                    {
                        DateTime = viewCalendarObj.timeStart
                    };
                }

                if (viewCalendarObj.timeEnd != null)
                {
                    googleEventObj.End = new EventDateTime()
                    {
                        DateTime = viewCalendarObj.timeEnd
                    };
                }

                googleEventObj.Status = "confirmed"; //即使已刪除，也會回復原本狀態
                
                Event updatedEvent = _service.Events.Update(googleEventObj, "primary", googleEventObj.Id).Execute();
                
                strTestLine = "update Event之後_LINE 177";
                //writeObj.writeToFile(strTestLine);

                //比對所有資料是否一致，才代表更改成功
                Event confirmEventObj = getEvent(viewCalendarObj.strEventID);
                if ((confirmEventObj.Start.DateTime == googleEventObj.Start.DateTime) 
                    && (confirmEventObj.End.DateTime == googleEventObj.End.DateTime) 
                    && (confirmEventObj.Summary == googleEventObj.Summary)
                    && (confirmEventObj.Location == googleEventObj.Location) 
                    && (confirmEventObj.Description == googleEventObj.Description) )
                {
                    return true;
                }else
                {
                    return false;
                }                
            }
            catch (Exception ex)
            {
                throw new Exception("updateEvent: 更新失敗　" + ex.Message);
            }
        }


        public Event getEvent(string strEventID)
        {
            try
            {
                 Event eventObj= _service.Events.Get("primary", strEventID).Execute();
                 return eventObj;
            }
            catch (Exception ex)
            {
                return null;
                //throw new Exception(ex.Message);
            }
        }
    }
}
