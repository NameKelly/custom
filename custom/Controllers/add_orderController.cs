using System;
using custom.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace custom.Controllers
{
    public class add_orderController : ApiController
    {
        //public string Post(add_order data)
        //{
        //    string result = custom.addOrder(/*data.name, data.quantity,*/data.id,data.record);
        //    return result;
        //}

        public string Post(add_order data)
        {
            string result = "";
            if (data == null)
            {
                result = custom.addOrder("","","");
            }
            else
            {
                result = custom.addOrder(data.order_id, data.record,data.phone);
            }
            return result;
        }
    }
}
