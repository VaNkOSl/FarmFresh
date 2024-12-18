namespace FarmFresh.Data.Models.Econt.APIInterraction
{
    public class GetOfficesRequest(string code3) : RequestBase
    {
        public string CountryCode { get; set; } = code3;
        public int? CityID { get; set; }
        public bool? ShowCargoReceptions { get; set; }
        public bool? ShowLC { get; set; }
        public bool? ServingReceptions { get; set; }
    }
}
