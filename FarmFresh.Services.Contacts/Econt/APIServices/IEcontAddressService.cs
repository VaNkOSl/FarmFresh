using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Services.Contacts.Econt.APIServices
{
    public interface IEcontAddressService
    {
        Task<bool> ValidateAddressAsync();
    }
}
