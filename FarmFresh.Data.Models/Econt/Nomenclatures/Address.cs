using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FarmFresh.Data.Models.Econt.Nomenclatures
{
    public class Address : Entity_1<int?>
    {
        [ForeignKey(nameof(City))]
        public int? CityId { get; set; }

        public City? City { get; set; }

        public string? FullAddress { get; set; }

        public string? FullAddressEn { get; set; }

        public string? Quarter { get; set; }

        public string? Street { get; set; }

        public string? Num { get; set; }

        public string? Other { get; set; }

        public GeoLocation? Location { get; set; }

        public string? Zip { get; set; }

        public string? Hezid { get; set; }
    }
}
