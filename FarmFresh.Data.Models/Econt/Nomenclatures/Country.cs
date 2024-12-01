using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace FarmFresh.Data.Models.Econt.Nomenclatures
{
    public class Country : Entity_1<int?>
    {
        [MaxLength(2)]
        public string? Code2 { get; set; }

        [MaxLength(3)]
        public string? Code3 { get; set; }

        public string? Name { get; set; }

        public string? NameEn { get; set; }

        public bool? IsEU { get; set; }
    }
}
