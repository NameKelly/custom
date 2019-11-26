using custom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace custom.Controllers
{
    public class getDataController : ApiController
    {

        public string Post(getData data)
        {
   
            custom get = new custom();
            string result = "";
            if (data == null)
            {
                result = get.getData("");
            }
            else
            {
                result = get.getData(data.code);
                
            }
            return result;
        }

    }
}
