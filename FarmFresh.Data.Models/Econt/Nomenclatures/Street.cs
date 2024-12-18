namespace FarmFresh.Data.Models.Econt.Nomenclatures
{
    public class Street : Entity_1<int>
    {
        public int? CityID { get; set; }

        public string? Name { get; set; }

        public string? NameEn { get; set; }

        public override bool Equals(object? obj)
        {
            if(obj is not Street other) return false;
            return CityID == other.CityID
                && Name == other.Name
                && NameEn == other.NameEn;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, CityID, Name);
        }
    }
}
