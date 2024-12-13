using FarmFresh.Data.Models.CustomConverters;
using FarmFresh.Data.Models.Econt.DTOs;
using FarmFresh.Data.Models.Enums;
using Newtonsoft.Json;
using System;

namespace FarmFresh.Data.Models.Econt.Nomenclatures
{
    public class Office : Entity_1<int>
    {
        public bool? IsMPS { get; set; }

        public bool? IsAPS { get; set; }

        public string? Name { get; set; }

        public string? NameEn { get; set; }

        public List<string>? Phones { get; set; }

        public List<string>? Emails { get; set; }

        public int? AddressId { get; set; }

        public Address? Address { get; set; }

        public string? Info { get; set; }

        public string? Currency { get; set; }

        public string? Language { get; set; }

        public long? NormalBusinessHoursFrom { get; set; }

        public long? NormalBusinessHoursTo { get; set; }

        public long? HalfDayBusinessHoursFrom { get; set; }

        public long? HalfDayBusinessHoursTo { get; set; }

        public long? SundayBusinessHoursFrom { get; set; }

        public long? SundayBusinessHoursTo { get; set; }

        public List<string>? ShipmentTypes { get; set; }

        public string? PartnerCode { get; set; }

        public string? HubCode { get; set; }

        public string? HubName { get; set; }

        public string? HubNameEn { get; set; }

        public bool? IsDrive { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj is not Office other) return false;
            return Id == other.Id
                && NameEn == other.NameEn
                && AddressId == other.AddressId;
        }

        public override int GetHashCode() => HashCode.Combine(Id, NameEn);
    }
}
