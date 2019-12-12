using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace assignment_api.Models
{


    public class Request
    {
        public string StrInput { get; set; }
        public string Size { get; set; }
    }


    public class SResponse
    {
        public string status { get; set; }

        public int Count { get; set; }
    }

    public class DResponse
    {
        public string status { get; set; }
        public List<History> Data { get; set; }
    }
    public class History
    {
        public string Input_String { get; set; }
        public string Size { get; set; }
        public string Output { get; set; }
    }

    public class Filter
    {
        public string Status { get; set; }
        public string Date { get; set; }
    }
}