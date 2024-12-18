using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.DTOs.ShipmentDTOs
{
    public class ClientProfileDTO(string name, List<string> phones)
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; } = name;

        [JsonProperty("nameEn")]
        public string? NameEn { get; set; }

        [JsonProperty("phones")]
        public List<string> Phones { get; set; } = phones;

        [JsonProperty("email")]
        public string? Email { get; set; }

        [JsonProperty("skypeAccounts")]
        public List<string>? SkypeAccounts { get; set; }

        [JsonProperty("clientNumber")]
        public string? ClientNumber { get; set; }

        [JsonProperty("clientNumberEn")]
        public string? ClientNumberEn { get; set; }

        [JsonProperty("juridicalEntity")]
        public bool? JuridicalEntity { get; set; }

        [JsonProperty("personalIDType")]
        public string? PersonalIDType { get; set; }

        [JsonProperty("personalIDNumber")]
        public string? PersonalIDNumber { get; set; }

        [JsonProperty("companyName")]
        public string? CompanyName { get; set; }

        [JsonProperty("ein")]
        public string? Ein { get; set; }

        [JsonProperty("ddsEinPrefix")]
        public string? DdsEinPrefix { get; set; }

        [JsonProperty("ddsEin")]
        public string? DdsEin { get; set; }

        [JsonProperty("registrationAddress")]
        public string? RegistrationAddress { get; set; }

        [JsonProperty("molName")]
        public string? MolName { get; set; }

        [JsonProperty("molEGN")]
        public string? MolEGN { get; set; }

        [JsonProperty("molNum")]
        public string? MolIDNum { get; set; }
    }
}
