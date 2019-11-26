using custom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace custom.Controllers
{
    public class select_stateController : ApiController
    {
        public string Post(select_state data)
        {
            string result = "";
            if (data == null)
            {
                result = custom.selectState("", "");
            }
            else
            {
                result = custom.selectState(data.state, data.date);
            }
            return result;
        }
    }
}
