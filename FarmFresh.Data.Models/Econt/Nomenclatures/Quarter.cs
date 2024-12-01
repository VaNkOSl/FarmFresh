using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.Nomenclatures
{
    public class Quarter : Entity_1<int?>
    {
        public int CityID { get; set; }

        public string? Name { get; set; }

        public string? NameEn { get; set; }
    }
}
