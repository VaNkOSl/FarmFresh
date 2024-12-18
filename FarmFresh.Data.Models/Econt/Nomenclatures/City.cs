namespace FarmFresh.Data.Models.Econt.Nomenclatures
{
    public class City : Entity_1<int>
    {
        public int? CountryId { get; set; }

        public Country? Country { get; set; }

        public string? PostCode { get; set; }

        public string? Name { get; set; }

        public string? NameEn { get; set; }

        public string? RegionName { get; set; }

        public string? RegionNameEn { get; set; }

        public string? PhoneCode { get; set; }

        public GeoLocation? Location { get; set; }

        public bool? ExpressCityDeliveries { get; set; }

        public bool? Monday { get; set; }

        public bool? Tuesday { get; set; }

        public bool? Wednesday { get; set; }

        public bool? Thursday { get; set; }

        public bool? Friday { get; set; }

        public bool? Saturday { get; set; }

        public bool? Sunday { get; set; }

        public int? ServiceDays { get; set; }

        public int? ZoneId { get; set; }

        public string? ZoneName { get; set; }

        public string? ZoneNameEn { get; set; }

        public List<ServingOfficeElement>? ServingOffices { get; set; }
    }
}
