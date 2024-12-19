using AutoMapper;
using FarmFresh.Data.Models.Econt.APIInterraction;
using FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs;
using FarmFresh.Data.Models.Econt.Nomenclatures;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Services.Contacts.Econt;
using FarmFresh.Services.Contacts.Econt.APIServices;

namespace FarmFresh.Services.Econt
{
    public class OfficeService(
        IEcontNumenclaturesService econtNumenclaturesService,
        IRepositoryManager repositoryManager,
        IMapper mapper)
        : IOfficeService
    {
        private readonly IEcontNumenclaturesService _econtNumenclaturesService = econtNumenclaturesService;
        private readonly IRepositoryManager _repositoryManager = repositoryManager;
        private readonly IMapper _mapper = mapper;

        public async Task UpdateOfficesAsync()
        {
            string[] codes = ["bgr", "grc", "rou"];

            var tasks = codes.Select(code =>
                _econtNumenclaturesService.GetOfficesAsync(new GetOfficesRequest(code))
                .ContinueWith(task => _mapper.Map<List<Office>>(task.Result))
            ).ToArray();

            var results = await Task.WhenAll(tasks);
            var offices = results.SelectMany(o => o).ToList();

            await _repositoryManager.OfficeRepository.UpdateOfficesAsync(offices);
        }

        public IQueryable<OfficeDTO> FindOfficesByCityId(int cityId)
        {
            var offices = _repositoryManager.OfficeRepository
            .FindOfficesByCondition(o => o.Address!.CityId == cityId, false);

            var officesDTOs = offices.Select(o => MapToDTO(o, _mapper));
            return officesDTOs;
        }

        private static OfficeDTO MapToDTO(Office office, IMapper mapper) => mapper.Map<OfficeDTO>(office);
    }
}
