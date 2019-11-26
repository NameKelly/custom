using custom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace custom.Controllers
{
    public class select_orderSumController : ApiController
    {
        public string Post(select_orderSum data)
        {
            string result = "";
            if (data == null)
            {
                result = custom.selectOrderSum("","");
            }
            else
            {
                result = custom.selectOrderSum(data.state,data.date);
            }
            return result;
        }
    }
}
