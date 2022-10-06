using API.Models;

namespace API.services
{
    public interface IWeightRules
    {
        RootViewModel CalculateWeights(RootViewModel viewModel, IdentityModel targetModel, string searchTerm);
    }
}
