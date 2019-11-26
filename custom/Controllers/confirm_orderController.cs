using System;
using custom.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace custom.Controllers
{
    public class confirm_orderController : ApiController
    {
        public string Post(confirm_order data)
        {
            string result = "";
            if (data == null)
            {
                result = custom.confirmOrder("");
            }
            else
            {
                result = custom.confirmOrder(data.order_id);
            }
            return result;
        }
    }
}
