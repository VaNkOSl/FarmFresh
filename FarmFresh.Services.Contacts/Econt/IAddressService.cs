using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Services.Econt
{
    public interface IAddressService
    {
        Task DeleteOrphanedAddressesAsync();
    }
}
