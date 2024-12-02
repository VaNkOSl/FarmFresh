using FarmFresh.ViewModels.DropDown;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Services.Contacts
{
    public interface IDropdownService
    {
        Task<IEnumerable<FarmerDropdownViewModel>> GetAllFarmersForDropdownAsync();
    }
}
