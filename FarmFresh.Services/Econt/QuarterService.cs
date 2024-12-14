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
    public class QuarterService(
        IEcontNumenclaturesService econtNumenclaturesService,
        IRepositoryManager repositoryManager,
        IMapper mapper)
        : IQuarterService
    {
        public async Task UpdateQuartersAsync()
        {
            var quarterDTOs = await econtNumenclaturesService.GetQuartersAsync(new GetQuartersRequest());
            var quarters = mapper.Map<List<Quarter>>(quarterDTOs);
            await repositoryManager.QuarterRepository.UpdateQuartersAsync(quarters);
        }
    }
}
