using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZonaFl.Persistence.Entities;
using ZonaFl.Persistence.Repository;

namespace ZonaFl.Business.SubSystems
{
    public class SCountry
    {

        public List<ZonaFl.Persistence.Entities.Country> FindAll()
        {

            CountryRepository countryrepo = new CountryRepository();
            return countryrepo.FindAll().ToList();


        }
        public List<ZonaFl.Persistence.Entities.City> FindCitiesByCountry(int CountryId)
        {

            CountryRepository countryrepo = new CountryRepository();
            return countryrepo.FindCitiesByCountry(CountryId).ToList();
        }

        public ZonaFl.Persistence.Entities.City FindFirstCityByCountry(int CountryId)
        {

            CountryRepository countryrepo = new CountryRepository();
            return countryrepo.FindFirstCityByCountry(CountryId);
        }


        public Country FindCountrybyName(string selectedCountryName)
        {
            CountryRepository countryrepo = new CountryRepository();
            return countryrepo.FindCountryByName(selectedCountryName);
        }

        public Country FindCountrybyId(int selectedCountryId)
        {
            CountryRepository countryrepo = new CountryRepository();
            return countryrepo.FindByID(selectedCountryId);
        }

        public ZonaFl.Persistence.Entities.City FindCityByName(string name)
        {

            CountryRepository countryrepo = new CountryRepository();
            return countryrepo.FindCityByName(name);
        }

        public ZonaFl.Persistence.Entities.City FindCityById(int Id)
        {

            CountryRepository countryrepo = new CountryRepository();
            return countryrepo.FindCityById(Id);
        }
    }
}
