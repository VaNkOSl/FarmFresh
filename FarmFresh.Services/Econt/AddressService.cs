using AutoMapper;
using FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs;
using FarmFresh.Data.Models.Econt.Nomenclatures;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Services.Contacts.Econt;

namespace FarmFresh.Services.Econt
{
    public class AddressService(
        IRepositoryManager repositoryManager,
        IMapper mapper) : IAddressService
    {
        private readonly IRepositoryManager _repositoryManager = repositoryManager;
        private readonly IMapper _mapper = mapper;

        public async Task DeleteOrphanedAddressesAsync()
            => await _repositoryManager.AddressRepository.DeleteOrphanedAddressesAsync();

        private static AddressDTO MapToDTO(Address address, IMapper mapper) => mapper.Map<AddressDTO>(address);
    }
}
