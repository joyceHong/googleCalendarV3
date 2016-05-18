using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace googleCalendarLibrary
{
    public class entryGoogleData:commonDB
    {
        /// <summary>
        /// 上傳資料的ikey
        /// </summary>
        public int intIkey
        {
            get;
            set;
        }

        

        /// <summary>
        /// 回傳結果是否成功
        /// </summary>
        public string strSendResult
        {
            get;
            set;
        }

        /// <summary>
        /// 回傳結果的訊息
        /// </summary>
        public string strSendMessage
        {
            get;
            set;
        }

        public DataTable readGoogleData(int intStartIkey, int intEndIkey)
        {
            WriteEvent.WrittingEventLog wirteObj = new WriteEvent.WrittingEventLog();

            try
            {
                
                DataTable dt = commonDB.selectQueryWithDataTable("select * from cooper_g3 where ikey >=" + intStartIkey + " AND ikey <=" + intEndIkey);                
                return dt;
            }
            catch (Exception ex)
            {
                wirteObj.writeToFile("LINE:53"+ex.Message );
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 將上傳資料回寫到資料庫
        /// </summary>
        public void saveGoogleMsg()
        {
            try
            {
                List<columnsData> liColumnDataObjs = new List<columnsData>();

                liColumnDataObjs.Add(new columnsData()
                {
                    strFileName = "ikey",
                    strValue = intIkey.ToString(),
                    oledbTypeValue = OleDbType.Integer,
                });

                liColumnDataObjs.Add(new columnsData()
                {
                    strFileName = "上傳完成",
                    strValue = strSendResult,
                    oledbTypeValue = OleDbType.Char
                });

                liColumnDataObjs.Add(new columnsData()
                {
                    strFileName = "結果訊息",
                    strValue = strSendMessage,
                    oledbTypeValue = OleDbType.Char
                });

                 liColumnDataObjs.Add(new columnsData()
                {
                    strFileName = " 上傳日時",
                    strValue = DateTime.Now.ToString("yyyyMMddHHmmss"),
                    oledbTypeValue = OleDbType.Char
                });

                 commonDB.updateWithParameter("cooper_g3", liColumnDataObjs);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
