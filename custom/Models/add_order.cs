using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Newtonsoft.Json;

namespace custom.Models
{
    public class add_order
    {
        //public string name { get; set; }
        //public double quantity { get; set; }
        public string order_id { get; set; }
        public string record { get; set; }
        public string phone { get; set; }

    }
}