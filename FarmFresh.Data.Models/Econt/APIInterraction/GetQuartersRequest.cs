namespace FarmFresh.Data.Models.Econt.APIInterraction
{
    public class GetQuartersRequest : RequestBase
    {
        public int CityID { get; set; }

        public GetQuartersRequest() { }
        public GetQuartersRequest(int cityID) => CityID = cityID;
    }
}
