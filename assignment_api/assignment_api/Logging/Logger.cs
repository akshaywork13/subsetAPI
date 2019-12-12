using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace assignment_api.Logging
{
    public class Logger
    {
        public static void WritetoFile(string strPath, string strMessage)
        {
            FileStream fileStream = null;
            StreamWriter streamWriter = null;
            try
            {
                if (Directory.Exists(strPath) == false)
                {
                    Directory.CreateDirectory(strPath);
                }

                if (Directory.Exists(strPath) == true)
                {
                    if (File.Exists(strPath + "\\" + "SubSet" + ".log") == false)
                    {
                        FileStream objFileStream = File.Create(strPath + "\\" + "SubSet" + ".log");
                        objFileStream.Close();
                    }
                }

                FileInfo fileInfo = new FileInfo(strPath + "\\" + "SubSet.log");
                long longFileLength = fileInfo.Length;

                if (longFileLength > (2048 * 2048))  //4MB
                {
                    File.Move(strPath + "\\" + "SubSet.log", strPath + "\\" + "ErrorLog" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".log");
                }

                fileStream = new FileStream(strPath + "\\" + "SubSet" + ".log", FileMode.Append);
                streamWriter = new StreamWriter(fileStream);

                string strLogData = strMessage;
                streamWriter.WriteLine(strLogData);
                //streamWriter.WriteLine("");
                streamWriter.Close();
                fileStream.Close();
            }
            catch (Exception)
            {

            }
        }
    }
}