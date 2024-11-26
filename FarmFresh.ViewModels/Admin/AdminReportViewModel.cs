using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmFresh.ViewModels.Admin
{
    public class AdminReportViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalProducts { get; set; }
        public int TotalFarmers { get; set; }
        public List<TopProductViewModel> TopProducts { get; set; } = new List<TopProductViewModel>();
    }

    public class TopProductViewModel
    {
        public string Name { get; set; } = string.Empty;
        public double AverageRating { get; set; }
    }
}
