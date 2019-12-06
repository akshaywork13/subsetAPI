using assignment_api.Models;
using MDMWebApi.Handlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace assignment_api.Controllers
{
    [BasicAuthentication]
    [RoutePrefix("api/Info")]
    public class SubsetController : ApiController
    {
        /// <summary>
        /// This is a post method to get the _Request from the user and in response provide the subset count.
        /// </summary>
        /// <param name="_Request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getcount")]
        public HttpResponseMessage GetSubSetCount(Request _Request)
        {
            SResponse _SResponse = new SResponse();
            DB objDB = new DB();
            string StrErrMsg = String.Empty;
            int subSetCount = 0;
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

                if(_Request.StrInput.Length< Convert.ToInt32(_Request.Size))
                {
                    throw new Exception("Length of string can not be less than size.");
                }

                if (!CountSubSet.GetCountSubset(_Request.StrInput, Convert.ToInt32(_Request.Size), ref StrErrMsg, ref subSetCount))
                    throw new Exception(StrErrMsg);

                _SResponse.Count = subSetCount;
                _SResponse.status = "Success";
                if (!objDB.PushHttpRequest(_Request.StrInput, _Request.Size, _SResponse.status + ", Count : " + _SResponse.Count, ref StrErrMsg))
                { 
                    // we can create Log File to store the exception.
                };
                return Request.CreateResponse<SResponse>(HttpStatusCode.OK, _SResponse);
                
            }
            catch (Exception ex)
            {
                _SResponse.status = ex.Message;
                _SResponse.Count = subSetCount;
                if (!objDB.PushHttpRequest(_Request.StrInput, _Request.Size, _SResponse.status + " : " + _SResponse.Count, ref StrErrMsg))
                {
                    // we can create Log File to store the exception.
                };
                return Request.CreateResponse<SResponse>(HttpStatusCode.BadRequest, _SResponse);
            }
        }
        /// <summary>
        /// This Get provide the History of HTTP reuest made till now.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("gethistory")]
        public HttpResponseMessage GetHistory()
        {
            DResponse _DResponse = new DResponse();
            try
            {
                _DResponse.Data = CountSubSet.GetHistory();
                _DResponse.status = "success";
            }
            catch (Exception ex)
            {
                // we can create Log File to store the exception.
            }
            return Request.CreateResponse<DResponse>(HttpStatusCode.OK, _DResponse);
        }
    }

}
