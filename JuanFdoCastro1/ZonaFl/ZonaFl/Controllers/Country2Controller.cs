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
    public class Country2Controller : Controller
    {


     
       
        public ActionResult Get(string SelectedCountryId)
        {

            List<Models.Country> countrylist = new List<Models.Country>();
            SCountry sco = new SCountry();
            List<Country> countries = sco.FindAll();
            Country countryr = sco.FindCountrybyId(int.Parse(SelectedCountryId));
            Models.Country countrym = new Models.Country();
            countrym.CountryID = countryr.Id;
            countrym.CountryName = countryr.Name;
            //var ret = countries.Select(x => new { x.Id, x.Name }).ToList();
            foreach (Country country in countries)
            {
                //if (country.Id == countryr.Id)
                //{
                countrylist.Add(new Models.Country()
                {
                    CountryID = country.Id,
                    CountryName = country.Name,
                    //Selected = true
                });


                //}
                //else
                //{
                //countrylist.Add(new Models.Country()
                //{
                //    CountryID = country.Id,
                //    CountryName = country.Name,
                //    Selected = false
                //});
                //}

            }

            //countrylist.Insert(0, countrym);

            return this.Json(countrylist, JsonRequestBehavior.AllowGet);

            //return new JsonResult { Data = countrylist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

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