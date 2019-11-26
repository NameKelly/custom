using custom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace custom.Controllers
{
    public class select_goodsController : ApiController
    {
        public string Post(select_goods data)
        {
            string result = "";
            if (data == null)
            {
                result = custom.selectGoods("");
            }
            else
            {
                result = custom.selectGoods(data.date);
            }
            return result;
        }
    }
}
