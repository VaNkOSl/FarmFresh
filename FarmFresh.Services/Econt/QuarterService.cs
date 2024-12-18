using AutoMapper;
using FarmFresh.Data.Models.Econt.APIInterraction;
using FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs;
using FarmFresh.Data.Models.Econt.Nomenclatures;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Services.Contacts.Econt;
using FarmFresh.Services.Contacts.Econt.APIServices;

namespace FarmFresh.Services.Econt
{
    public class QuarterService(
        IEcontNumenclaturesService econtNumenclaturesService,
        IRepositoryManager repositoryManager,
        IMapper mapper)
        : IQuarterService
    {
        private readonly IEcontNumenclaturesService _econtNumenclaturesService = econtNumenclaturesService;
        private readonly IRepositoryManager _repositoryManager = repositoryManager;
        private readonly IMapper _mapper = mapper;

        public async Task UpdateQuartersAsync()
        {
            var quarterDTOs = await _econtNumenclaturesService.GetQuartersAsync(new GetQuartersRequest());
            var quarters = _mapper.Map<List<Quarter>>(quarterDTOs);
            await _repositoryManager.QuarterRepository.UpdateQuartersAsync(quarters);
        }

        public IQueryable<QuarterDTO> FindQuartersByCityId(int cityId)
        {
            var quarters = _repositoryManager.QuarterRepository
            .FindQuartersByCondition(q => q.CityID == cityId, false);

            var quartersDTOs = quarters.Select(q => MapToDTO(q, _mapper));
            return quartersDTOs;
        }

        private static QuarterDTO MapToDTO(Quarter quarter, IMapper mapper) => mapper.Map<QuarterDTO>(quarter);
    }
}
