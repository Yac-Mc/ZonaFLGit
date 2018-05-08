using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZonaFl.Entities.CustomEntities
{
   public class Pagos
    {
        public int  merchantId { get; set; }
        public string ApiLogin { get; set; }
        public string ApiKey { get; set; }
        public int accountId { get; set; }
        public string description { get; set; }
        public string amount { get; set; }
        public string tax { get; set; }
        public string taxReturnBase { get; set; }
        public string currency { get; set; }
        public string signature { get; set; }
        public int  test { get; set; }
        public string buyerEmail { get; set; }
        public string responseUrl { get; set; }
        public string confirmationUrl { get; set; }
        public string urlprueba { get; set;}
        public string refVenta { get; set; }
        public string referenceCode { get;  set; }

        public Pagos()
        {
            merchantId = 568308;
            ApiLogin = "jSfrrMR0Dwa8v85";
            ApiKey = "4Vj8eK4rloUd272L48hsrarnUA";
            referenceCode = "";
            accountId = 570915;
            description = "";
            amount = "";
            tax = "0.00";
            taxReturnBase = "0.00";
            currency = "COP";
            signature = "";
            test = 0;
            buyerEmail = "";
            responseUrl = "http://zonafl.com/Projects/DetailsPay";
           // confirmationUrl = "http://www.test.com/confirmation";
            //urlprueba = "https://sandbox.gateway.payulatam.com/ppp-web-gateway/";




        }

    }
}
