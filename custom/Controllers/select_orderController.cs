using custom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace custom.Controllers
{
    public class select_orderController : ApiController
    {
        //public string Post(select_order data)
        //{
        //    string result = custom.selectOrder(data.id);
        //    return result;
        //}

        public string Post(select_order data)
        {
            string result = "";
            if (data == null)
            {
                result = custom.selectOrder("");
            }
            else
            {
                result = custom.selectOrder(data.order_id);
            }
            return result;
        }
    }
}
