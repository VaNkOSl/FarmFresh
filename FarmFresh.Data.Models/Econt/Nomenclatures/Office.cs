using FarmFresh.Data.Models.CustomConverters;
using FarmFresh.Data.Models.Econt.DTOs;
using FarmFresh.Data.Models.Enums;
using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.Nomenclatures
{
    public class Office : Entity_1<int?>
    {
        public bool? IsMPS { get; set; }

        public bool? IsAPS { get; set; }

        public string? Name { get; set; }

        public string? NameEn { get; set; }

        public List<string>? Phones { get; set; }

        public List<string>? Emails { get; set; }

        public Address? Address { get; set; }

        public string? Info { get; set; }

        public string? Currency { get; set; }

        public string? Language { get; set; }

        public TimeOnly NormalBusinessHoursFrom { get; set; }

        public TimeOnly NormalBusinessHoursTo { get; set; }

        public TimeOnly HalfDayBusinessHoursFrom { get; set; }

        public TimeOnly HalfDayBusinessHoursTo { get; set; }

        public TimeOnly SundayBusinessHoursFrom { get; set; }

        public TimeOnly SundayBusinessHoursTo { get; set; }

        public ShipmentType? ShipmentType { get; set; }

        public string? PartnerCode { get; set; }

        public string? HubCode { get; set; }

        public string? HubName { get; set; }

        public string? HubNameEn { get; set; }

        public bool? IsDrive { get; set; }
    }
}
