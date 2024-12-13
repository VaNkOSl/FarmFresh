using AutoMapper;
using FarmFresh.Data.Models.Econt.APIInterraction;
using FarmFresh.Data.Models.Econt.Nomenclatures;
using FarmFresh.Data.Models.Repositories;
using FarmFresh.Services.Contacts.Econt;
using FarmFresh.Services.Contacts.Econt.APIServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
