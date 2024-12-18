using FarmFresh.Data.Models.CustomConverters;
using FarmFresh.Data.Models.Enums;
using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.DTOs.ShipmentDTOs
{
    public class InstructionDTO
    {
        [JsonProperty("id")]
        public int? Id { get; set; }

        [JsonProperty("type")]
        public string? Type { get; set; }

        [JsonProperty("title")]
        public string? Title { get; set; }

        [JsonProperty("description")]
        public string? Description { get; set; }

        [JsonProperty("attatchments")]
        public List<HostedFileDTO>? Attatchments { get; set; }

        [JsonProperty("voiceDescription")]
        public HostedFileDTO? VoiceDescription { get; set; }

        [JsonProperty("returnInstructionParams")]
        public ReturnInstructionParamsDTO? ReturnInstructionParams { get; set; }

        [JsonProperty("name")]
        public string? Name { get; set; }

        [JsonProperty("applyToAllParcels")]
        public bool? ApplyToAllParcels { get; set; }

        [JsonProperty("applyToReceivers")]
        public string? ApplyToReceivers { get; set; }

        public InstructionDTO()
        {
            Title = "Return instruction";
            Description = "Return instruction";
            Type = InstructionTypeConverter.ConvertEnumToString(InstructionType.Give);
            ReturnInstructionParams = new ReturnInstructionParamsDTO();
        }
    }
}
