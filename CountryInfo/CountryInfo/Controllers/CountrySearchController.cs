using System;
using System.Web.Mvc;
using Business;
using Business.Interface;
using CountryInfo.Models;

namespace CountryInfo.Controllers
{
    public class CountrySearchController : Controller
    {
        private readonly ICountryInfoService _countryInfoService;

        public CountrySearchController()
        {
            // TODO: Inject in the services
            _countryInfoService = new CountryInfoService(new WorldBankClient());
        }

        public ActionResult Index()
        {
            return View(new CountrySearchModel());
        }

        [HttpPost]
        public ActionResult Search(CountrySearchModel countrySearchModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", new CountrySearchModel());
            }

            var searchModel = CountrySearchModel(countrySearchModel.Query);

            return View("Index", searchModel);
        }

        private CountrySearchModel CountrySearchModel(string searchQuery)
        {
            Country searchResult = null;
            string errorMsg = null;

            try
            {
                searchResult = _countryInfoService.GetCountryInfo(searchQuery);
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
            }

            var model = new CountrySearchModel
            {
                Query = searchQuery,
                Result = searchResult,
                ErrorMessage = errorMsg,
            };

            return model;
        }
    }
}
