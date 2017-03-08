using IshServices.Auth;
using IshServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace IshServices.Controllers
{
    [ApiKeyAuth]
    public class RegistrationController : ApiController
    {
        // POST api/<controller>
        public IHttpActionResult Post([FromBody]RegistrationRequest registration)
        {
            //TODO: convert registration to RegistrationResult if valid data
            //save registration
            return Ok(registration);
        }

    }
}