using System;
using custom.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace custom.Controllers
{
    public class add_remarkController : ApiController
    {
        public string Post(add_remark data)
        {
            string result = "";
            if (data == null)
            {
                result = custom.addRemark("","","","","","","","","");
            }
            else
            {
                result = custom.addRemark(data.best,data.score_look,data.score_color,data.score_smell,data.score_taste,data.score_chef,data.comment,data.phone,data.name);
            }
            return result;
        }
    }
}
