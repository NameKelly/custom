using custom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace custom.Controllers
{
    public class select_menuController : ApiController
    {
        public string Post(select_menu data)
        {
            string result = "";
            if (data == null)
            {
                result = custom.selectMenu("");
            }
            else
            {
                result = custom.selectMenu(data.goods_type);
            }
            return result;
        }
    }
}
