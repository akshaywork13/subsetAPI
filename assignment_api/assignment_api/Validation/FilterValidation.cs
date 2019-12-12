using assignment_api.Models;
using System;
using System.Globalization;

namespace assignment_api.Validation
{
    public class FilterValidation
    {
        public static bool ValidateFilter(Filter _Filter, ref string strException)
        {
            try
            {
                if (_Filter.Date!=null && _Filter.Date!= "")
                {
                    DateTime dt = DateTime.Parse(_Filter.Date);
                }
                return true;
            }
            catch (Exception)
            {
                strException = "Please Enter Valid Date Format.";
                return false;
            }
        }
    }
}