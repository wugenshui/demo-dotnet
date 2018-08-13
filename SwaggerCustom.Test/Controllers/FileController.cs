using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace SwaggerCustom.Test.Controllers
{
    public class FileController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Post([FromBody]HttpPostedFileBase file)
        {
            return Json(file.FileName);
        }
    }
}
