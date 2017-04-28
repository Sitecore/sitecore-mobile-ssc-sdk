using Sitecore.MobileSDK.API;
using Sitecore.MobileSDK.API.Items;
using SSCExtensions;
using System;
using System.Threading.Tasks;

namespace SCHelpers
{
    [Serializable]

    public class ScNetworkHelper
    {

        public async Task<ScItemsResponse> GetAllCities()
        {
            string query = "/sitecore/content/Home//*[@@templatename='City Item']";

            ScItemsResponse response = await this.GetItemsByQyery(query);

            return response;
        }

        public async Task<ScItemsResponse> GetAllCountries()
        {
            string query = "/sitecore/content/Home//*[@@templatename='Country Item']";

            ScItemsResponse response = await this.GetItemsByQyery(query);

            return response;
        }

        public async Task<ScItemsResponse> GetAllRegions()
        {
            string query = "/sitecore/content/Home//*[@@templatename='Region Item']";

            ScItemsResponse response = await this.GetItemsByQyery(query);

            return response;
        }



        public async Task<ISitecoreItem> GetRegionNamed(string gerionTitle)
        {
            string query = "/sitecore/content/Home//*[@@templatename='Region Item' and contains(@Title, '" + gerionTitle + "')]";

            ScItemsResponse response = await this.GetItemsByQyery(query);

            if (response == null || response.ResultCount == 0)
            {
                return null;
            }

            return response[0];
        }

        public async Task<ISitecoreItem> GetCountryNamed(string countryTitle)
        {
            string query = "/sitecore/content/Home//*[@@templatename='Country Item' and contains(@Title, '" + countryTitle + "')]";

            ScItemsResponse response = await this.GetItemsByQyery(query);

            if (response == null || response.ResultCount == 0)
            {
                return null;
            }

            return response[0];
        }

        public async Task<ISitecoreItem> GetCityNamed(string cityTitle)
        {
            string query = "/sitecore/content/Home//*[@@templatename='City Item' and contains(@Title, '" + cityTitle + "')]";

            ScItemsResponse response = await this.GetItemsByQyery(query);

            if (response == null || response.ResultCount == 0)
            {
                return null;
            }

            return response[0];
        }

        public async Task<ScItemsResponse> GetCountriesForRegion(string regionTitle)
        {
            ISitecoreItem region = await this.GetRegionNamed(regionTitle);

            if (region == null) {
                return null;
            }

            ScItemsResponse response = await this.GetItemChildren(region);

            return response;
        }

        public async Task<ScItemsResponse> GetCitiesForCountry(string countryTitle)
        {
            ISitecoreItem country = await this.GetCountryNamed(countryTitle);

            if (country == null)
            {
                return null;
            }

            ScItemsResponse response = await this.GetItemChildren(country);

            return response;
        }

        private async Task<ScItemsResponse> GetItemsByQyery(string query)
        {
            using (var credentials = ScNetworkSettings.Credentials())
            {

                using (var session = ScNetworkSettings.Session(credentials))
                {
                    var ext = ExtendedSessionBuilder.ExtendedSessionWith(session)
                                                    .PathForTemporaryItems("/sitecore/content/Home")
                                                    .Build();

                    var request = ExtendedSSCRequestBuilder.SitecoreQueryRequest(query)
                                                           .PageNumber(0)
                                                           .PageNumber(0)
                                                           .ItemsPerPage(500)//max
                                                           .Build();

                    var response = await ext.SearchBySitecoreQueryAsync(request);

                    return response;
                }
            }
        }

        private async Task<ScItemsResponse> GetItemChildren(ISitecoreItem parentItem)
        {
            using (var credentials = ScNetworkSettings.Credentials())
            {

                using (var session = ScNetworkSettings.Session(credentials))
                {

                    var request = ItemSSCRequestBuilder.ReadChildrenRequestWithId(parentItem.Id)
                        .Build();

                    var response = await session.ReadChildrenAsync(request);

                    return response;
                }
            }
        }

    }
}