using assignment_api.Logging;
using assignment_api.Models;
using assignment_api.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace assignment_api
{
    public class DB
    {
        DataTable dtHistoryData = new DataTable();
        static string connString = AppKeyVaidation.ValidateAppKey("DBConnectionString", "connectionStrings");

        /// <summary>
        /// This DB method return the table to History Data.
        /// </summary>
        /// <param name="StrErrMsg"></param>
        /// <returns></returns>
        public DataTable GetHistory(ref string StrErrMsg,Filter _Filter)
        {
            try
            {
                string query = string.Empty;
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    if(conn.State!= ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    if ((_Filter.Status == null || _Filter.Status == "") && (_Filter.Date == null || _Filter.Date == ""))
                    {
                        query = "SELECT Input_String,Size,Output FROM T_http_history";
                    }
                    if ((_Filter.Status != null || _Filter.Status != "") && (_Filter.Date == null || _Filter.Date == ""))
                    {
                        query = "SELECT Input_String,Size,Output FROM T_http_history Where Output '%" + _Filter.Status + "+%'";
                    }
                    if ((_Filter.Status == null || _Filter.Status == "") && (_Filter.Date != null || _Filter.Date != ""))
                    {
                        query = "SELECT Input_String,Size,Output FROM T_http_history Where CAST(EntryDateTime As Date) ='" + _Filter.Date + "'";
                    }
                    if ((_Filter.Status != null || _Filter.Status != "") && (_Filter.Date != null || _Filter.Date != ""))
                    {
                        query = "SELECT Input_String,Size,Output FROM T_http_history Where  Output '%" + _Filter.Status + "+%' AND CAST(EntryDateTime As Date) ='" + _Filter.Date + "'";
                    }
                    SqlCommand cmd = new SqlCommand(query, conn);
                    dtHistoryData.Load(cmd.ExecuteReader());
                    conn.Close();
                    conn.Dispose();
                }
            }
            catch (Exception ex)
            {
                StrErrMsg = ex.Message;
            }
            return dtHistoryData;
        }
        /// <summary>
        /// This method used to store the input and output of the HTTP request into DB.
        /// </summary>
        /// <param name="InputString"></param>
        /// <param name="Size"></param>
        /// <param name="Output"></param>
        /// <param name="StrErrMsg"></param>
        /// <returns></returns>
        public bool PushHttpRequest(string InputString, string Size, string Output, ref string StrErrMsg)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(connString))
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    conn.Open();
                    string query = "INSERT INTO T_http_history(Input_String,Size,Output) VALUES(" + "\'" + InputString + "\'" + "," + "\'" + Size + "\'" + "," + "\'" + Output + "\'" + ")";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                    conn.Dispose();
                }
                return true;
            }
            catch (Exception ex)
            {
                StrErrMsg = ex.Message;
                return false;
            }
        }

    }
    public class CountSubSet
    {
        static string LogPath = AppKeyVaidation.ValidateAppKey("LogPath", "appSettings");
        /// <summary>
        /// This Mehod return the count of subset according to the size input.
        /// </summary>
        /// <param name="StrInput"></param>
        /// <param name="Size"></param>
        /// <param name="StrErrMsg"></param>
        /// <param name="Count"></param>
        /// <returns></returns>
        public static bool GetCountSubset(string StrInput, int Size, ref string StrErrMsg,ref int Count)
        {
            try
            {
                int strLength = StrInput.Length;

                for (int i = Size; i <= strLength; i++)
                {
                    Count = Count + strLength - i + 1;
                }
                return true;
            }
            catch (Exception ex)
            {
                StrErrMsg = ex.Message;
                return false;
            }
        }
        /// <summary>
        /// This method return the list of history.
        /// </summary>
        /// <returns></returns>
        public static List<History> GetHistory(Filter _Filter)
        {
            DB objDB = new DB();
            List<History> _History = null;
            try
            {
                string StrErrMsg = String.Empty;
                DataTable dtData = objDB.GetHistory(ref StrErrMsg, _Filter);
                _History = dtData.AsEnumerable().Select(row =>
                 new History
                 {
                     Input_String = row.Field<string>("Input_String"),
                     Size = row.Field<string>("Size"),
                     Output = row.Field<string>("Output")
                 }).ToList();
            }
            catch (Exception ex)
            {
                Logger.WritetoFile(LogPath, "Error in History : " + ex.Message + " at DateTime : " + DateTime.Now);
            }
            return _History;
        }
    }
}