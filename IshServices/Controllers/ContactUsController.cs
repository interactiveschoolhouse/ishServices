using IshServices.Auth;
using IshServices.Models;
using IshServices.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IshServices.Controllers
{
    [ApiKeyAuth]
    public class ContactUsController : ApiController
    {

        // POST api/<controller>
        public IHttpActionResult Post([FromBody]ContactUs value)
        {
            ContactUsService cus = new ContactUsService();
            cus.Send(value);

            return Ok(value);
        }

    }
}