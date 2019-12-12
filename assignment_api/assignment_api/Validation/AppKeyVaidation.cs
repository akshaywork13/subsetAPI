using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace assignment_api.Validation
{
    public class AppKeyVaidation
    {
        public static string ValidateAppKey(string AppKey, string KeyType)
        {
            string _AppKey = string.Empty;
            try
            {
                if (KeyType == "appSettings")
                {
                    if (ConfigurationManager.AppSettings[AppKey] != null)
                    {
                        _AppKey = ConfigurationManager.AppSettings["LogPath"].ToString();
                    }
                    else
                    {
                        throw new ArgumentNullException();
                    }
                }
                if(KeyType == "connectionStrings")
                {
                    if (ConfigurationManager.ConnectionStrings[AppKey] != null)
                    {
                        _AppKey = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
                    }
                    else
                    {
                        throw new ArgumentNullException();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _AppKey;
        }
    }
}