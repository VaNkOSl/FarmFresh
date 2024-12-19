using FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs;
using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.DTOs.ShipmentDTOs
{
    public class ShipmentStatusDTO
    {
        [JsonProperty("shipmentNumber")]
        public string? ShipmentNumber { get; set; }

        [JsonProperty("storageOfficeName")]
        public string? StorageOfficeName { get; set; }

        [JsonProperty("storagePersonName")]
        public string? StoragePersonName { get; set; }

        [JsonProperty("createdTime")]
        public double? CreatedTime { get; set; }

        [JsonProperty("sendTime")]
        public double? SendTime { get; set; }

        [JsonProperty("deliveryTime")]
        public int? DeliveryTime { get; set; }

        [JsonProperty("shipmentType")]
        public string? ShipmentType { get; set; }

        [JsonProperty("packCount")]
        public int? PackCount { get; set; }

        [JsonProperty("shipmentDescription")]
        public string? ShipmentDescription { get; set; }

        [JsonProperty("weight")]
        public double? Weight { get; set; }

        [JsonProperty("senderDeliveryType")]
        public string? SenderDeliveryType { get; set; }

        [JsonProperty("senderClient")]
        public ClientProfileDTO? SenderClient { get; set; }

        [JsonProperty("senderAgent")]
        public ClientProfileDTO? SenderAgent { get; set; }

        [JsonProperty("senderOfficeCode")]
        public string? SenderOfficeCode { get; set; }

        [JsonProperty("senderAddress")]
        public AddressDTO? SenderAddress { get; set; }

        [JsonProperty("receiverDeliveryType")]
        public string? ReceiverDeliveryType { get; set; }

        [JsonProperty("receiverClient")]
        public ClientProfileDTO? ReceiverClient { get; set; }

        [JsonProperty("receiverAgent")]
        public ClientProfileDTO? ReceiverAgent { get; set; }

        [JsonProperty("receiverOfficeCode")]
        public string? ReceiverOfficeCode { get; set; }

        [JsonProperty("receiverAddress")]
        public AddressDTO? ReceiverAddress { get; set; }

        [JsonProperty("cdCollectedAmount")]
        public double? CDCollectedAmount { get; set; }

        [JsonProperty("cdCollectedCurrency")]
        public string? CDCollectedCurrency { get; set; }

        [JsonProperty("cdCollectedTime")]
        public double? CDCollectedTime { get; set; }

        [JsonProperty("cdPaidAmount")]
        public double? CDPaidAmount { get; set; }

        [JsonProperty("cdPaidCurrency")]
        public string? CDPaidCurrency { get; set; }

        [JsonProperty("cdPaidTime")]
        public double? CDPaidTime { get; set; }

        [JsonProperty("totalPrice")]
        public double? TotalPrice { get; set; }

        [JsonProperty("currency")]
        public string? Currency { get; set; }

        [JsonProperty("discountPercent")]
        public double? DiscountPercent { get; set; }

        [JsonProperty("discountAmount")]
        public double? DiscountAmount { get; set; }

        [JsonProperty("discountDescription")]
        public string? DiscountDescription { get; set; }

        [JsonProperty("senderDueAmount")]
        public double? SenderDueAmount { get; set; }

        [JsonProperty("receiverDueAmount")]
        public double? ReceiverDueAmount { get; set; }

        [JsonProperty("otherDueAmount")]
        public double? OtherDueAmount { get; set; }

        [JsonProperty("delveryAttemptCount")]
        public int? DeliveryAttemptCount { get; set; }

        [JsonProperty("previousShipmentNumber")]
        public string? PreviousShipmentNumber { get; set; }

        [JsonProperty("services")]
        public List<ShipmentStatusServiceDTO>? Services { get; set; }

        [JsonProperty("lastProcessedInstruction")]
        public string? LastProcessedInstruction { get; set; }

        [JsonProperty("nextShipments")]
        public List<NextShipmentElementDTO>? NextShipments { get; set; }

        [JsonProperty("trackingEvents")]
        public List<ShipmentTrackingEventDTO>? TrackingEvents { get; set; }

        [JsonProperty("pdfURL")]
        public string? PDFURL { get; set; }

        [JsonProperty("expectedDeliveryDate")]
        public double? ExpectedDeliveryDate { get; set; }

        [JsonProperty("returnShipmentURL")]
        public string? ReturnShipmentURL { get; set; }

        [JsonProperty("rejectOriginalPaySide")]
        public string? RejectOriginalParcelPaySide { get; set; }

        [JsonProperty("rejectReturnParcelPaySide")]
        public string? RejectReturnParcelPaySide { get; set; }

        [JsonProperty("shipmentEdition")]
        public ShipmentEditionResponseElementDTO? ShipmentEdition { get; set; }

        [JsonProperty("previousShipment")]
        public PreviousShipmentDTO? PreviousShipment { get; set; }

        [JsonProperty("warnings")]
        public string? Warnings { get; set; }

        [JsonProperty("shortDeliveryStatus")]
        public string? ShortDeliveryStatus { get; set; }

        [JsonProperty("shortDeliveryStatusEn")]
        public string? ShortDeliveryStatusEn { get; set; }
    }
}
