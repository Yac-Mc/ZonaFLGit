using System.Collections.Generic;
using System.Web.Http;
using ZonaFl.Persistence.Entities;
using ZonaFl.Business;
using ZonaFl.Business.SubSystems;

using System.Web.Http.Results;
using System.Web.Mvc;
using System.Linq;

using Omu.ValueInjecter;

namespace ZonaFl.Controllers
{
    public class CountryController : ApiController
    {
        
        public JsonResult Get()
        {

            List<Models.Country> countrylist = new List<Models.Country>();
            SCountry sco = new SCountry();
            List<Country> countries = sco.FindAll();

            //var ret = countries.Select(x => new { x.Id, x.Name }).ToList();
            foreach (Country country in countries)
            {
                countrylist.Add( new Models.Country()
                {
                    CountryID = country.Id,
                    CountryName = country.Name
                   
                });
            }
            
            return new JsonResult { Data = countrylist,  JsonRequestBehavior = JsonRequestBehavior.AllowGet };
           
        }

        [System.Web.Http.AcceptVerbs("GET", "POST")]
        [System.Web.Http.HttpGet]
        public JsonResult Get(string SelectedCountryId)
        {

            List<Models.Country> countrylist = new List<Models.Country>();
            SCountry sco = new SCountry();
            List<Country> countries = sco.FindAll();
            Country countryr = sco.FindCountrybyId(int.Parse(SelectedCountryId));
            Models.Country countrym = new Models.Country();
            countrym.InjectFrom(countryr);
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

            countrylist.Insert(0, countrym);

            return new JsonResult { Data =countrylist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }

        public JsonResult FindCitiesByCountry(int countryId)
        {



            List<City> citylist = new List<City>();
            SCountry sco = new SCountry();
            citylist = sco.FindCitiesByCountry(countryId);
            return new JsonResult { Data = citylist, JsonRequestBehavior = JsonRequestBehavior.AllowGet };

        }
    }
}