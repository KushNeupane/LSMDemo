using API.Models;
using API.Utility;
using System;
using System.Collections.Generic;
using System.Linq;

namespace API.services
{
    public class SearchService : ISearchService
    {
        private readonly IWeightRules _weightService;

        public SearchService()
        {
            _weightService = new BuildingWeightService();
        }
        public RootViewModel searchItem(string SearchQuery)
        {
            var identityModels = IdentityModel(SearchQuery, Helper.DeserializeAndFlatten(Helper.GetSerializedJSON()));
            var viewModel = Newtonsoft.Json.JsonConvert.DeserializeObject<RootViewModel>(Helper.GetSerializedJSON());
            foreach (var identityModel in identityModels)
            {
                 viewModel = _weightService.CalculateWeights(
                       viewModel,
                       identityModel,
                       SearchQuery);
            }

            return new RootViewModel
            {
                buildings = viewModel.buildings.Where(x => x.weight > 0).OrderByDescending(x => x.weight).ToList(),
                locks = viewModel.locks.Where(x => x.weight > 0).OrderByDescending(x => x.weight).ToList(),
                groups = viewModel.groups.Where(x => x.weight > 0).OrderByDescending(x => x.weight).ToList(),
                media = viewModel.media.Where(x => x.weight > 0).OrderByDescending(x => x.weight).ToList()
            };
        }

        private static List<IdentityModel> IdentityModel(string searchQuery, Dictionary<string, object> dataDictionary)
        {
            var identityModels = new List<IdentityModel>();
            var id = "";
            foreach (var item in dataDictionary)
            {
                var arr = item.Key.Split(new string[] { "." }, StringSplitOptions.None);                
                if (arr.Contains("id"))
                    id = item.Value.ToString();
                if (item.Value?.ToString().ToLower().Contains(searchQuery.ToLower()) ?? false)
                {
                    var identityModel = new IdentityModel();
                    identityModel.Id = id;
                    if (item.Value?.ToString().ToLower() == searchQuery.ToString().ToLower().Trim())
                    {
                        identityModel.EntityName = arr[0];
                        identityModel.PropertyName = arr[0] + "." + arr[2];
                        identityModel.FullMatch = true;                       
                    }
                    else
                    {
                        identityModel.EntityName = arr[0];
                        identityModel.PropertyName = arr[0] + "." + arr[2];
                        identityModel.FullMatch = false;
                    }
                    identityModels.Add(identityModel);
                }
            }
            return identityModels;
        }
    }
}