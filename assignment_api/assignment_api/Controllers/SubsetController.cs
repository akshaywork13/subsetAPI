using assignment_api.Models;
using assignment_api.Validation;
using MDMWebApi.Handlers;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using assignment_api.Logging;
using System.Collections.Generic;

namespace assignment_api.Controllers
{
    [BasicAuthentication]
    [RoutePrefix("api/Info")]
    public class SubsetController : ApiController
    {  
        DB objDB = new DB();
        static string LogPath = AppKeyVaidation.ValidateAppKey("LogPath", "appSettings");
        string StrErrMsg = String.Empty;
        
        /// <summary>
        /// This is a post method to get the _Request from the user and in response provide the subset count.
        /// </summary>
        /// <param name="_Request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("getcount")]
        public HttpResponseMessage GetSubSetCount(Request _Request)
        {
            Logger.WritetoFile(LogPath, "Requested String : " + _Request.StrInput + ", Requested Size : " + _Request.Size);

            SResponse _SResponse = new SResponse();
            int subSetCount = 0;
            try
            {
                if (!RequestValidation.ValidateRequest(_Request, ref StrErrMsg))
                    throw new Exception(StrErrMsg);
                
                if (!CountSubSet.GetCountSubset(_Request.StrInput, Convert.ToInt32(_Request.Size), ref StrErrMsg, ref subSetCount))
                    throw new Exception(StrErrMsg);
                _SResponse.Count = subSetCount;
                _SResponse.status = "success";
                if (!objDB.PushHttpRequest(_Request.StrInput, _Request.Size, _SResponse.status + ", Count : " + _SResponse.Count, ref StrErrMsg))
                {
                    Logger.WritetoFile(LogPath, "Error in Database : " + StrErrMsg + " at DateTime : " + DateTime.Now);
                };

                Logger.WritetoFile(LogPath, "Success in Request : " + _SResponse.status + " at DateTime : " + DateTime.Now);

                return Request.CreateResponse<SResponse>(HttpStatusCode.OK, _SResponse);
                
            }
            catch (Exception ex)
            {
                _SResponse.status = ex.Message;
                _SResponse.Count = subSetCount;

                Logger.WritetoFile(LogPath, "Error in Request : " + ex.Message + " at DateTime : " + DateTime.Now);

                if (!objDB.PushHttpRequest(_Request.StrInput, _Request.Size, "fail "+ _SResponse.status + " : " + _SResponse.Count, ref StrErrMsg))
                {
                    Logger.WritetoFile(LogPath, "Error in Database : " + StrErrMsg+ " at DateTime : "+DateTime.Now);
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
        public HttpResponseMessage GetHistory(Filter _Filter)
        {
            DResponse _DResponse = new DResponse();
            try
            {
                if (!FilterValidation.ValidateFilter(_Filter, ref StrErrMsg))
                    throw new Exception(StrErrMsg);

                _DResponse.Data = CountSubSet.GetHistory(_Filter);
                _DResponse.status = "success";

                Logger.WritetoFile(LogPath, "Success in Request History: " + _DResponse.status + " at DateTime : " + DateTime.Now);

                return Request.CreateResponse<DResponse>(HttpStatusCode.OK, _DResponse);
            }
            catch (Exception ex)
            {
                _DResponse.status = ex.Message;

                Logger.WritetoFile(LogPath, "Error in History : " + ex.Message + " at DateTime : " + DateTime.Now);
                return Request.CreateResponse<DResponse>(HttpStatusCode.BadRequest, _DResponse);
            }
        }
    }

}
