using FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs;
using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.DTOs.ShipmentDTOs
{
    public class CDPayOptionsDTO
    {
        [JsonProperty("num")]
        public string? Num { get; set; }

        [JsonProperty("client")]
        public ClientProfileDTO? Client { get; set; }

        [JsonProperty("moneyTransfer")]
        public bool? MoneyTransfer { get; set; }

        [JsonProperty("express")]
        public bool Express { get; set; }

        [JsonProperty("method")]
        public string? Method { get; set; }

        [JsonProperty("address")]
        public AddressDTO? Address { get; set; }

        [JsonProperty("officeCode")]
        public string? OfficeCode { get; set; }

        public string? IBAN { get; set; }

        public string? BIC { get; set; }

        [JsonProperty("bankCurrency")]
        public string? BankCurrency { get; set; }

        [JsonProperty("payDays")]
        public List<int>? PayDays { get; set; }

        [JsonProperty("payWeekDays")]
        public List<string>? PayWeekDays { get; set; }

        [JsonProperty("additionalInstructions")]
        public string? AdditionalInstructions { get; set; }

        [JsonProperty("departmentNum")]
        public int? DepartmentNum { get; set; }
    }
}
