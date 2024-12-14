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
    public class StreetService(
        IEcontNumenclaturesService econtNumenclaturesService,
        IRepositoryManager repositoryManager,
        IMapper mapper)
        : IStreetService
    {
        public async Task UpdateStreetsAsync()
        {
            var streetDTOs = await econtNumenclaturesService.GetStreetsAsync(new GetStreetsRequest());
            var streets = mapper.Map<List<Street>>(streetDTOs);
            await repositoryManager.StreetRepository.UpdateStreetsAsync(streets);
        }
    }
}
