using System;
using custom.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace custom.Controllers
{
    public class select_myOrderController : ApiController
    {
        public string Post(select_myOrder data)
        {
            string result = "";
            if (data == null)
            {
                result = custom.selectMyOrder("","");
            }
            else
            {
                result = custom.selectMyOrder(data.state,data.phone);
            }
            return result;
        }
    }
}

