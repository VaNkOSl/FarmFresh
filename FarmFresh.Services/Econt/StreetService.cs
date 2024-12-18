using AutoMapper;
using FarmFresh.Data.Models.Econt.APIInterraction;
using FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs;
using FarmFresh.Data.Models.Econt.Nomenclatures;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Services.Contacts.Econt;
using FarmFresh.Services.Contacts.Econt.APIServices;

namespace FarmFresh.Services.Econt
{
    public class StreetService(
        IEcontNumenclaturesService econtNumenclaturesService,
        IRepositoryManager repositoryManager,
        IMapper mapper)
        : IStreetService
    {
        private readonly IEcontNumenclaturesService _econtNumenclaturesService = econtNumenclaturesService;
        private readonly IRepositoryManager _repositoryManager = repositoryManager;
        private readonly IMapper _mapper = mapper;

        public async Task UpdateStreetsAsync()
        {
            var streetDTOs = await _econtNumenclaturesService.GetStreetsAsync(new GetStreetsRequest());
            var streets = _mapper.Map<List<Street>>(streetDTOs);
            await _repositoryManager.StreetRepository.UpdateStreetsAsync(streets);
        }

        public IQueryable<StreetDTO> FindStreetsByCityId(int cityId)
        {
            var streets = _repositoryManager.StreetRepository
            .FindStreetsByCondition(s => s.CityID == cityId, false);

            var streetsDTOs = streets.Select(s => MapToDTO(s, _mapper));
            return streetsDTOs;
        }

        private static StreetDTO MapToDTO(Street street, IMapper mapper) => mapper.Map<StreetDTO>(street);
    }
}
