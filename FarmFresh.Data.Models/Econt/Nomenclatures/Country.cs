namespace FarmFresh.Data.Models.Econt.Nomenclatures
{
    public class Country : Entity_1<int>
    {
        public required string Code2 { get; set; }

        public string? Code3 { get; set; }

        public string? Name { get; set; }

        public string? NameEn { get; set; }

        public bool? IsEU { get; set; }
    }
}
