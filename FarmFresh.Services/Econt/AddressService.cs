using FarmFresh.Data.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Services.Econt
{
    public class AddressService(IRepositoryManager repositoryManager) : IAddressService
    {
        private readonly IRepositoryManager _repositoryManager = repositoryManager;

        public async Task DeleteOrphanedAddressesAsync()
            => await _repositoryManager.AddressRepository.DeleteOrphanedAddressesAsync();
    }
}
