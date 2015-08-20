using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GoogleOAuthWebApi.Controllers
{
    public class IdTokenController : ApiController
    {
        [HttpPost]
        public IHttpActionResult SetToken(JObject obj)
        {
            // google token passed from client
            string idToken = (string)obj["id_token"];

            // decrypt the google token
            string jsonResponse;
            using (var client = new WebClient())
            {
                jsonResponse = client.DownloadString("https://www.googleapis.com/oauth2/v3/tokeninfo?id_token=" + idToken);
            }

            // parse the response from google
            JObject tokenInfo = JObject.Parse(jsonResponse);

            // add the userid to the payload of our new token
            var payload = new Dictionary<string, object>()
                {
                    { "userId", (string)tokenInfo["sub"] }
                };

            // encrypt the payload and create token and return it to the client
            var secretKey = ConfigurationManager.AppSettings["JWTSecret"]; ;
            string token = JWT.JsonWebToken.Encode(payload, secretKey, JWT.JwtHashAlgorithm.HS256);
            return Ok(token);
        }
    }
}
