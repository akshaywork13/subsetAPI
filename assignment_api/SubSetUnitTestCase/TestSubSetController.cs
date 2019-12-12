using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading.Tasks;
using System.Collections.Generic;
using assignment_api.Models;
using assignment_api.Controllers;

namespace SubSetUnitTestCase
{
    [TestClass]
    public class TestSubSetController
    {
        [TestMethod]
        public void GetAllRequest_ShouldReturnAllRequest()
        {
        }
        private List<Request> GetTestRequest()
        {
            var _Request = new List<Request>();
            _Request.Add(new Request { StrInput = "abcdef", Size = "2"});
            _Request.Add(new Request { StrInput = "asdkfjg", Size = "2.2"});
            _Request.Add(new Request { StrInput = "", Size = "4"});
            _Request.Add(new Request { StrInput = "", Size = ""});
            return _Request;
        }
    }
}
