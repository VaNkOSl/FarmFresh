using FarmFresh.Data.Models;

namespace FarmFresh.Repositories.Extensions;

public static class FarmerRepositoryExtensions
{
    public static IQueryable<Farmer> Search(this IQueryable<Farmer> farmers, string searchTerm)
    {
        if(string.IsNullOrWhiteSpace(searchTerm))
        {
            return farmers;
        }

        var lowerCaseTerm = searchTerm.Trim().ToLower();
        return farmers.Where(f => f.User.FirstName.Trim().ToLower().Contains(lowerCaseTerm) ||
                                  f.User.LastName.Trim().ToLower().Contains(lowerCaseTerm) ||
                                  (f.User.FirstName.Trim() + " " + f.User.LastName.Trim()).ToLower().Contains(lowerCaseTerm) ||
                                  f.PhoneNumber.Trim().ToLower().Contains(lowerCaseTerm));
    }
}