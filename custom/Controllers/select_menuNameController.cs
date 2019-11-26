using System;
using custom.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace custom.Controllers
{
    public class select_menuNameController : ApiController
    {
        public string Post(select_menuName data)
        {
            string result = "";
            if (data == null)
            {
                result = custom.selectMenuName("");
            }
            else
            {
                result = custom.selectMenuName(data.menu_type);
            }
            return result;
        }
    }
}

