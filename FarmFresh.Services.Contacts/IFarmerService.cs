﻿using FarmFresh.Commons.RequestFeatures;
using FarmFresh.ViewModels.Farmer;

namespace FarmFresh.Services.Contacts;

public interface IFarmerService
{
    Task CreateFarmerAsync(FarmerCreateForm model, string userId, bool trackChanges);

    Task CreateFarmerLocationAsync(FarmerLocationDto model, Guid farmerId);

    Task<bool> DoesFarmerExistAsync(string egn, string phoneNumber, string userId, bool trackChanges);

    Task<(IEnumerable<FarmersViewModel> farmers, MetaData metaData)> GetAllFarmersAsync(FarmerParameters farmerParameters, bool trackChanges);

    Task<FarmersListViewModel> CreateFarmersListViewModelAsync(IEnumerable<FarmersViewModel> farmers, MetaData metaData, string? searchTerm);
}
