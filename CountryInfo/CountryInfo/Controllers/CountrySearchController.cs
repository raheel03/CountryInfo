using System;
using System.Web.Mvc;
using System.Threading.Tasks;
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

            _countryInfoService = new CountryInfoService(
                new CountryCodeValidator(), 
                new WorldBankClient());
        }

        public ActionResult Index()
        {
            return View(new CountrySearchModel());
        }

        [HttpPost]
        public async Task<ActionResult> Search(CountrySearchModel countrySearchModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", new CountrySearchModel());
            }

            var searchModel = await SearchCountry(countrySearchModel.Query);

            return View("Index", searchModel);
        }

        private async Task<CountrySearchModel> SearchCountry(string countryCode)
        {
            Country searchResult = null;
            string errorMsg = null;

            try
            {
                searchResult = await _countryInfoService.GetCountryInfo(countryCode);
            }
            catch (Exception e)
            {
                errorMsg = e.Message;
            }

            var model = new CountrySearchModel
            {
                Query = countryCode,
                Result = searchResult,
                ErrorMessage = errorMsg,
            };

            return model;
        }
    }
}
