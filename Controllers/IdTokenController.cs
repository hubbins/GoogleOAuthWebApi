using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GoogleOAuthWebApi.Controllers
{
    public class IdTokenController : ApiController
    {
        public IHttpActionResult SetToken([FromBody] string idToken)
        {
            string jsonResponse;
            using (var client = new WebClient())
            {
                jsonResponse = client.DownloadString("https://www.googleapis.com/oauth2/v3/tokeninfo?id_token=" + idToken);
            }

            JObject user = JObject.Parse(jsonResponse);
            // verify aud
            // sub is user id

            return Ok();
        }
    }
}
