using assignment_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace assignment_api.Validation
{
    public class RequestValidation
    {
        public static bool ValidateRequest(Request _Request,  ref string strException)
        {
            try
            {
                if (_Request.StrInput == "" || _Request.StrInput == null)
                {
                    throw new Exception("String Input Can not be empty.");
                }
                int value;
                if (!int.TryParse(_Request.Size, out value))
                {
                    throw new Exception("Size can only be integer.");
                }

                if (Convert.ToInt32(_Request.Size) == 0)
                {
                    throw new Exception("Subset 0 can not be calculated.");
                }

                if (_Request.StrInput.Length < Convert.ToInt32(_Request.Size))
                {
                    throw new Exception("Length of string can not be less than size.");
                }
                
                return true;
            }
            catch (Exception ex)
            {
                strException = ex.Message;
                return false;
            }
        }
    }
}