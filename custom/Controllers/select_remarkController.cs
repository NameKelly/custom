using System;
using custom.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace custom.Controllers
{
    public class select_remarkController : ApiController
    {
        public string Post(select_remark data)
        {
            string result = "";
            if (data == null)
            {
                result = custom.selectRemark("");
            }
            else
            {
                result = custom.selectRemark(data.name);
            }
            return result;
        }
    }
}
