namespace FarmFresh.Data.Models.Econt.APIInterraction
{
    public class GetStreetsRequest : RequestBase
    {
        public int CityID { get; set; }

        public GetStreetsRequest() { }
        public GetStreetsRequest(int cityID) => CityID = cityID;
    }
}
