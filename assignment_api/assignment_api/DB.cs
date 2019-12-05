using assignment_api.Models;
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
        public DataTable GetHistory(ref string StrErrMsg)
        {
            DataTable dtHistoryData = new DataTable();
            try
            {
                SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString);
                conn.Open();

                string query = "SELECT Input_String,Size,Output FROM T_http_history";

                SqlCommand cmd = new SqlCommand(query, conn);
                dtHistoryData.Load(cmd.ExecuteReader());
            }
            catch (Exception ex)
            {
                StrErrMsg = ex.Message;
            }
            return dtHistoryData;
        }

        public bool PushHttpRequest(string InputString, string Size, string Output, ref string StrErrMsg)
        {
            try
            {
                SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString);
                conn.Open();

                string query = "INSERT INTO T_http_history(Input_String,Size,Output) VALUES(" + "\'"+ InputString + "\'" + "," + "\'" +Size + "\'"+ "," + "\'"+Output +"\'" + ")";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.ExecuteNonQuery();

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
        public static List<History> GetHistory()
        {
            DB objDB = new DB();
            string StrErrMsg = String.Empty;
            DataTable dtData = objDB.GetHistory(ref StrErrMsg);
            List<History> _History = dtData.AsEnumerable().Select(row => 
             new History
                {
                    Input_String = row.Field<string>("Input_String"),
                    Size = row.Field<string>("Size"),
                    Output = row.Field<string>("Output")
                }).ToList();

            return _History;
        }
    }
}