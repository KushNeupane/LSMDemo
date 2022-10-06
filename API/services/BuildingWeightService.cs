using API.Models;
using API.Utility;
using System.Collections.Generic;
using System.Linq;

namespace API.services
{
    public class BuildingWeightService : IWeightRules
    {
        public RootViewModel CalculateWeights(RootViewModel viewModel, IdentityModel targetModel, string searchTerm)
        {
            Dictionary<string, int> corpusDict = Helper.getCorpus();
            foreach (var corpus in corpusDict)
            {
                if (targetModel.PropertyName.ToLower() == corpus.Key.ToLower())
                {
                    switch (targetModel.EntityName)
                    {
                        case "buildings":
                            if (targetModel.PropertyName.ToLower() == "buildings.name")
                            {
                                var buildings = viewModel.buildings.FirstOrDefault(x => x.id == targetModel.Id);
                                buildings.weight += corpus.Value * (targetModel.FullMatch == true ? 10 : 1);

                                //For transferring the transitive weights to the locks of this buildings.
                                var locks = viewModel.locks.Where(x => x.buildingId == targetModel.Id).ToList();
                                locks.ForEach(x => x.weight += 8 * (targetModel.FullMatch == true ? 10 : 1)); //transitive weight of name and shortcut from buildings model.
                            }
                            if (targetModel.PropertyName.ToLower() == "buildings.shortCut")
                            {
                                var buildings = viewModel.buildings.FirstOrDefault(x => x.id == targetModel.Id);
                                buildings.weight += 7 * (targetModel.FullMatch == true ? 10 : 1);
                            }
                            if (targetModel.PropertyName.ToLower() == "buildings.description")
                            {
                                var buildings = viewModel.buildings.FirstOrDefault(x => x.id == targetModel.Id);
                                buildings.weight += 5 * (targetModel.FullMatch == true ? 10 : 1);
                            }
                            break;

                        // when the search query is found on the different fields of locks
                        case "locks":
                            var lks = viewModel.locks.Where(x => x.id == targetModel.Id).ToList();
                            if (targetModel.PropertyName.ToLower() == "locks.name")                                                          
                                lks.ForEach(x => x.weight += corpus.Value * (targetModel.FullMatch == true ? 10 : 1));
                           
                            if (targetModel.PropertyName.ToLower() == "locks.type")
                                lks.ForEach(x => x.weight += 3 * (targetModel.FullMatch == true ? 10 : 1));

                            if (targetModel.PropertyName.ToLower() == "locks.serialNumber")
                                lks.ForEach(x => x.weight += 8 * (targetModel.FullMatch == true ? 10 : 1));

                            if (targetModel.PropertyName.ToLower() == "locks.floor")
                                lks.ForEach(x => x.weight += 6 * (targetModel.FullMatch == true ? 10 : 1));

                            if (targetModel.PropertyName.ToLower() == "locks.roomNumber")
                                lks.ForEach(x => x.weight += 6 * (targetModel.FullMatch == true ? 10 : 1));
                            break;


                        case "groups":
                            if (targetModel.PropertyName.ToLower() == "groups.name")
                            {
                                var groups = viewModel.groups.FirstOrDefault(x => x.id == targetModel.Id);
                                groups.weight += corpus.Value * (targetModel.FullMatch == true ? 10 : 1);
                                var medias = viewModel.media.Where(x => x.groupId == targetModel.Id).ToList();
                                medias.ForEach(x => x.weight += 8 * (targetModel.FullMatch == true ? 10 : 1));
                            }
                            break;

                        case "media":
                            var mds = viewModel.media.Where(x => x.id == targetModel.Id).ToList();
                            if (targetModel.PropertyName.ToLower() == "media.type")                                
                                mds.ForEach(x => x.weight += 3 * (targetModel.FullMatch == true ? 10 : 1));
                            if (targetModel.PropertyName.ToLower() == "media.owner")                               
                                mds.ForEach(x => x.weight += 10 * (targetModel.FullMatch == true ? 10 : 1));
                            if (targetModel.PropertyName.ToLower() == "media.serialNumber")                                
                                mds.ForEach(x => x.weight += 8 * (targetModel.FullMatch == true ? 10 : 1));
                            break;
                        default:
                            break;
                    }

                }
            }
            return viewModel;
        }
    }
}
