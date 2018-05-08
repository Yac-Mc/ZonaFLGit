using System.Collections.Generic;
using System.Web.Http;
using ZonaFl.Persistence.Entities;
using ZonaFl.Business;
using ZonaFl.Business.SubSystems;

using System.Web.Http.Results;
using System.Web.Mvc;
using System.Linq;

using Omu.ValueInjecter;
using System.Web.Http;
namespace ZonaFl.Controllers
{
    public class City2Controller : Controller
    {


     
        public ActionResult Get(int id)
        {



            List<City> citylist = new List<City>();
            SCountry sco = new SCountry();
            citylist = sco.FindCitiesByCountry(id);
            List<Models.City> citiesm = new List<Models.City>();
            citiesm = citylist.Select(e => new Models.City().InjectFrom(e)).Cast<Models.City>().ToList();
            //return new JsonResult { Data = citylist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //return citiesm;
            return this.Json(citiesm, JsonRequestBehavior.AllowGet);

        }

        //[System.Web.Http.HttpGet]
        //public JsonResult Get(string name)
        //{




        //    List<City> citylist = new List<City>();
        //    SCountry sco = new SCountry();
        //    System.Char delimiter = '-';
        //    var ciudad = sco.FindCityByName(name.Split(delimiter)[1]);

        //    citylist = sco.FindCitiesByCountry(int.Parse(name.Split(delimiter)[0])).Where(e=>e.Id!= ciudad.Id).ToList();
        //    citylist.Insert(0, ciudad);
        //    return new JsonResult { Data = citylist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        //}
    }
}