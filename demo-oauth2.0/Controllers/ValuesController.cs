using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Web.Http;

namespace demoOAuth2.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        [Authorize]
        public string Get()
        {
            ClaimsIdentity identity = User.Identity as ClaimsIdentity;
            Claim claim = identity.Claims.Where(o => o.Type == "userinfo").FirstOrDefault();
            if (claim != null)
            {
                return claim.Value;
            }
            return User.Identity.Name;
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }

    /// <summary>
    /// 接受测试代码
    /// </summary>
    public class CodesController : ApiController
    {
        [HttpGet]
        [Route("api/authorization_code")]
        public HttpResponseMessage Get(string code)
        {
            return new HttpResponseMessage()
            {
                Content = new StringContent(code, Encoding.UTF8, "text/plain")
            };
        }

        [HttpGet]
        [Route("api/access_token")]
        public HttpResponseMessage GetToken()
        {
            var url = Request.RequestUri;
            return new HttpResponseMessage()
            {
                Content = new StringContent("", Encoding.UTF8, "text/plain")
            };
        }
    }
}
