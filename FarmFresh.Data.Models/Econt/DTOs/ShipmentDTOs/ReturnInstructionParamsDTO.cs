using FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs;
using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.DTOs.ShipmentDTOs
{
    public class ReturnInstructionParamsDTO
    {
        [JsonProperty("returnParcelDestination")]
        public string? ReturnParcelDestination { get; set; }

        [JsonProperty("returnParcellsDocument")]
        public bool? ReturnParcellsDocument { get; set; }

        [JsonProperty("returnParcellsEmptyPallet")]
        public bool? ReturnParcellsEmptyPallet { get; set; }

        [JsonProperty("emptyPalletEuro")]
        public int? EmptyPalletEuro { get; set; }

        [JsonProperty("emptyPallet80")]
        public int? EmptyPallet80 { get; set; }

        [JsonProperty("emptyPallet100")]
        public int? EmptyPallet100 { get; set; }

        [JsonProperty("emptyPallet120")]
        public int? EmptyPallet120 { get; set; }

        [JsonProperty("daysUntilReturn")]
        public int? DaysUntilReturn { get; set; }

        [JsonProperty("returnParcelPaymentSide")]
        public string? ReturnParcelPaymentSide { get; set; }

        [JsonProperty("returnParcelReceiverClient")]
        public ClientProfileDTO? ReturnParcelReceiverClient { get; set; }

        [JsonProperty("returnParcelReceiverAgent")]
        public ClientProfileDTO? ReturnParcelReceiverAgent { get; set; }

        [JsonProperty("returnParcelReceiverOfficeCode")]
        public string? ReturnParcelReceiverOfficeCode { get; set; }

        [JsonProperty("returnParcelReceiverAddress")]
        public AddressDTO? ReturnParcelReceiverAddress { get; set; }

        [JsonProperty("printReturnParcel")]
        public bool? PrintReturnParcel { get; set; }

        [JsonProperty("rejectAction")]
        public string? RejectAction { get; set; }

        [JsonProperty("rejectInstruction")]
        public string? RejectInstruction { get; set; }

        [JsonProperty("rejectConstact")]
        public string? RejectContact { get; set; }

        [JsonProperty("rejectReturnClient")]
        public ClientProfileDTO? RejectReturnClient { get; set; }

        [JsonProperty("rejectReturnAgent")]
        public ClientProfileDTO? RejectReturnAgent { get; set; }

        [JsonProperty("rejectReturnOfficeCode")]
        public string? RejectReturnOfficeCode { get; set; }

        [JsonProperty("rejectReturnAddress")]
        public AddressDTO? RejectReturnAddress { get; set; }

        [JsonProperty("rejectOriginalParcelPayside")]
        public string? RejectOriginalParcelPaySide { get; set; }

        [JsonProperty("rejectReturnParcelPayside")]
        public string? RejectReturnParcelPaySide { get; set; }

        [JsonProperty("signatureDocuments")]
        public bool? SignatureDocuments { get; set; }

        [JsonProperty("signaturePenColor")]
        public string? SignaturePenColor { get; set; }

        [JsonProperty("signatureCount")]
        public int? SignatureCount { get; set; }

        [JsonProperty("signaturePageNumbers")]
        public string? SignaturePageNumbers { get; set; }

        [JsonProperty("signatureOtherInstructions")]
        public string? SignatureOtherInstructions { get; set; }

        [JsonProperty("executeIfRejectedWithoutReview")]
        public bool? ExecuteIfRejectedWithoutReview { get; set; }

        [JsonProperty("useReturnAddressForInstruction")]
        public bool? UseReturnAddressForInstruction { get; set; }

        [JsonProperty("executeIfNotTaken")]
        public int? ExecuteIfNotTaken { get; set; }

        public ReturnInstructionParamsDTO()
        {
            ReturnParcelDestination = "sender";
            ReturnParcellsDocument = false;
            DaysUntilReturn = 7;
            ReturnParcelPaymentSide = "sender";
            PrintReturnParcel = false;
            RejectAction = "contact";
            RejectContact = "0888888888";
            ExecuteIfRejectedWithoutReview = true;
            ExecuteIfNotTaken = DaysUntilReturn;
        }
    }
}
