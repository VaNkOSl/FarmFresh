using AutoMapper;
using FarmFresh.Data.Models.Econt.APIInterraction;
using FarmFresh.Data.Models.Econt.Nomenclatures;
using FarmFresh.Repositories.Contacts;
using FarmFresh.Services.Contacts.Econt;
using FarmFresh.Services.Contacts.Econt.APIServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Services.Econt
{
    public class CityService(
        IEcontNumenclaturesService econtNumenclaturesService,
        IRepositoryManager repositoryManager,
        IMapper mapper)
        : ICityService
    {
        private readonly IEcontNumenclaturesService _econtNumenclaturesService = econtNumenclaturesService;
        private readonly IRepositoryManager _repositoryManager = repositoryManager;
        private readonly IMapper _mapper = mapper;

        public async Task UpdateCitiesAsync()
        {
            string[] codes = ["bgr", "grc", "rou"];

            var tasks = codes.Select(code =>
                _econtNumenclaturesService.GetCitiesAsync(new GetCitiesRequest(code))
                .ContinueWith(task => _mapper.Map<List<City>>(task.Result))
            ).ToArray();

            var results = await Task.WhenAll(tasks);
            var cities = results.SelectMany(c => c).ToList();

            await _repositoryManager.CityRepository.UpdateCitiesAsync(cities);
        }
    }
}
