using custom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace custom.Controllers
{
    public class cancel_orderController : ApiController
    {
        public string Post(cancel_order data)
        {
            string result = "";
            if (data == null)
            {
                result = custom.cancelOrder("");
            }
            else
            {
                result = custom.cancelOrder(data.order_id);
            }
            return result;
        }
    }
}
