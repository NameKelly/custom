using custom.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace custom.Controllers
{
    public class QRCodeController : ApiController
    {
        public string Post(QRCode data)
        {
            string result = "";
            if (data == null)
            {
                result = custom.selectTime("");
            }
            else
            {
                result = custom.selectTime(data.order_id);
            }
            return result;
        }
    }
}
