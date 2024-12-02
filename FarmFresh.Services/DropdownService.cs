using FarmFresh.Data;
using FarmFresh.Services.Contacts;
using FarmFresh.ViewModels.DropDown;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.Services
{
    public class DropdownService : IDropdownService
    {
        private readonly FarmFreshDbContext _context;

        public DropdownService(FarmFreshDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FarmerDropdownViewModel>> GetAllFarmersForDropdownAsync()
        {
            return await _context.Farmers
                .Include(f => f.User)
                .Select(f => new FarmerDropdownViewModel
                {
                    Id = f.Id,
                    UserName = f.User.UserName
                })
                .ToListAsync();
        }
    }
}
