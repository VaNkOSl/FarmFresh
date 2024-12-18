namespace FarmFresh.Data.Models.Econt.Nomenclatures
{
    public class Address : Entity_1<int>
    {
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

        public override bool Equals(object? obj)
        {
            if (obj is not Address other) return false;
            return CityId == other.CityId
                && FullAddress == other.FullAddress
                && Quarter == other.Quarter
                && Street == other.Street
                && Num == other.Num
                && Other == other.Other
                && Zip == other.Zip;
        }

        public override int GetHashCode() => HashCode.Combine(FullAddress, CityId);
    }
}
