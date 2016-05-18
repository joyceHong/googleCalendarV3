using Google.Apis.Calendar.v3.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;


namespace googleCalendarLibrary
{
    public class controlCalendarcs 
    {

        /// P0:BSSOT
        /// P1:1/2/3;  單筆寫入，單筆修改，單筆刪除
        /// P2:gmail帳號
        /// P3:KeyNo:18bytes(病歷編號+9位recno)
        /// P4:主旨
        /// P5:開始日期
        /// P6:開始時間
        /// P7:結束日期
        /// P8:結束時間
        /// P9:地點
        /// P10:描述
        /// P11:事件顏色
        /// 


        /// <summary>
        /// 前置字
        /// </summary>
        public string strClipboardText
        {
            get;
            set;
        }
        public int currentGoogleIkey
        {
            get;
            set;
        }
        //WriteEvent.WrittingEventLog writeObj = new WriteEvent.WrittingEventLog();
        public IgoogleCalendarHelp googleCalendarHelpObj;
        
        public void start(string strCooperPath, int intStartIkey, int intEndIkey)
        {
            string[] Parameters;
            viewCalendar viewCalendarObj = new viewCalendar();       
            entryGoogleData allGoogleData = new entryGoogleData();
            PublicInfo.Cooper_Path = strCooperPath;
            int intCurrentGoogleIkey = 0;
            //writeObj.writeToFile(DateTime.Today.ToString("yyyyMMdd") + "_calendarError", Directory.GetCurrentDirectory(), "------------start-----------------");
            
            try
            {              
                DataTable dt = allGoogleData.readGoogleData(intStartIkey, intEndIkey);

                //writeObj.writeToFile(DateTime.Today.ToString("yyyyMMdd") + "_calendarError", Directory.GetCurrentDirectory(), "LINE : 60");

                //step3:  讀取資料
                if (dt.Rows.Count > 0)
                {
                    //writeObj.writeToFile(DateTime.Today.ToString("yyyyMMdd") + "_calendarError", Directory.GetCurrentDirectory(), "LINE : 65");

                    foreach (DataRow dr in dt.Rows)
                    {
                        int.TryParse( dr["ikey"].ToString(), out intCurrentGoogleIkey);
                        currentGoogleIkey = intCurrentGoogleIkey;
                        Parameters = dr["上傳參數"].ToString().Split(new String[] { char.ConvertFromUtf32(13) + char.ConvertFromUtf32(10) }, System.StringSplitOptions.RemoveEmptyEntries);

                        //writeObj.writeToFile(DateTime.Today.ToString("yyyyMMdd") + "_calendarError", Directory.GetCurrentDirectory(), "LINE : 73");

                        if (Parameters[0] == "BSSOTA")
                        {
                            viewCalendarObj.strGmailAccount = Parameters[2];
                            viewCalendarObj.strEventID = Parameters[3];
                            viewCalendarObj.strSummary = Parameters[4];

                            viewCalendarObj.timeStart = DateTime.ParseExact(Parameters[5].Trim() + Parameters[6].Trim(), "yyyyMMddHHmm", null);
                            viewCalendarObj.timeEnd = DateTime.ParseExact(Parameters[7].Trim() + Parameters[8].Trim(), "yyyyMMddHHmm", null);
                            viewCalendarObj.strLocation = Parameters[9];
                            viewCalendarObj.strDesc = Parameters[10];
                            viewCalendarObj.strEventColor = Parameters[11];

                            //googleCalendarHelpObj = new googleCalendarHelp(viewCalendarObj.strGmailAccount); //for service account
                            googleCalendarHelpObj = new googleCalendarTaken(viewCalendarObj.strGmailAccount); // for web Authentication

                            if (Parameters[1] == "1")
                            {
                                //新增
                                //writeObj.writeToFile(DateTime.Today.ToString("yyyyMMdd") + "_calendarError", Directory.GetCurrentDirectory(), "LINE : 93");
                                addSingleEvent(viewCalendarObj);
                            }
                            else if (Parameters[1] == "2")
                            {
                                //修改
                                //writeObj.writeToFile(DateTime.Today.ToString("yyyyMMdd") + "_calendarError", Directory.GetCurrentDirectory(), "LINE : 99");
                                updateSingleEvent(viewCalendarObj);
                            }
                            else if (Parameters[1] == "3")
                            {
                                //刪除
                                //writeObj.writeToFile(DateTime.Today.ToString("yyyyMMdd") + "_calendarError", Directory.GetCurrentDirectory(), "LINE : 105");
                                deleteSingleEvent(viewCalendarObj.strEventID);
                            }
                        }

                        int intGoogleIkey = 0;
                        int.TryParse(dr["ikey"].ToString(), out intGoogleIkey);
                        //讀取參數
                        allGoogleData.intIkey = intGoogleIkey;
                        allGoogleData.strSendMessage = "上傳完成";
                        allGoogleData.strSendResult = "Y";
                        allGoogleData.saveGoogleMsg();
                        //writeObj.writeToFile(DateTime.Today.ToString("yyyyMMdd") + "_calendarError", Directory.GetCurrentDirectory(), "LINE : 117------end------");
                    }
                }
            }
            catch (Exception ex)
            {
                //return to foxpro db 
                 string strJson = JsonConvert.SerializeObject(viewCalendarObj);

                allGoogleData.intIkey = intCurrentGoogleIkey;
                allGoogleData.strSendMessage =  "錯誤內容:" + ex.Message;
                allGoogleData.strSendResult = "N";
                allGoogleData.saveGoogleMsg();
                
                throw new Exception("controlCalendarcs.start 發生錯誤(1), 上傳資料： " +ex.Message);
            }
        }
     
        /// <summary>
        /// 新增
        /// </summary>
        public void addSingleEvent(viewCalendar viewCalendarObj)
        {
            try
            {
                 googleCalendarHelpObj.addEvent(viewCalendarObj);
            }
            catch (Exception ex)
            {
                string strJson = JsonConvert.SerializeObject(viewCalendarObj);
                throw new Exception("controlCalendarcs.addSingleEvent 發生錯誤(2), 上傳資料： " + strJson + ex.Message);
            }
        }

        /// <summary>
        /// 刪除
        /// </summary>
        public void deleteSingleEvent(string strEventID)
        {
            try
            {
                Event googleEventObj = googleCalendarHelpObj.getEvent(strEventID);
                if (googleEventObj != null)
                {
                    if (googleEventObj.Status != "cancelled")
                    {
                        googleCalendarHelpObj.deleteEvent(strEventID);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("controlCalendarcs.deleteSingleEvent 發生錯誤, strEventID： " + strEventID + " "+ex.Message);
            }
        }

        public void updateSingleEvent( viewCalendar viewCalendarObj)
        {
            try
            {
                Event googleEventObj = googleCalendarHelpObj.getEvent(viewCalendarObj.strEventID);
                //googleCalendarHelpObj.updateEvent(viewCalendarObj);
                if (googleEventObj == null)
                {
                    googleCalendarHelpObj.addEvent(viewCalendarObj);
                }
                else
                {
                    googleCalendarHelpObj.updateEvent(viewCalendarObj);
                }
            }
            catch (Exception ex)
            {
                string strJson = JsonConvert.SerializeObject(viewCalendarObj);
                throw new Exception("controlCalendarcs.updateSingleEvent 更新資料：" + strJson + " " + ex.Message);
            }
        }
       
    }
}
