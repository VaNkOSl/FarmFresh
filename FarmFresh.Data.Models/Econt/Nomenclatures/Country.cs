using Newtonsoft.Json;
using static FarmFresh.Commons.EntityValidationConstants.Country;
using System.ComponentModel.DataAnnotations;

namespace FarmFresh.Data.Models.Econt.Nomenclatures
{
    public class Country : Entity_1<int?>
    {
        [MaxLength(Code2MaxLength)]
        public string? Code2 { get; set; }

        [MaxLength(Code3MaxLength)]
        public string? Code3 { get; set; }

        public string? Name { get; set; }

        public string? NameEn { get; set; }

        public bool? IsEU { get; set; }
    }
}
