using FarmFresh.Data.Models.CustomConverters;
using FarmFresh.Data.Models.Econt.DTOs.NumenclatureDTOs;
using FarmFresh.Data.Models.Enums;
using Newtonsoft.Json;

namespace FarmFresh.Data.Models.Econt.DTOs.ShipmentDTOs
{
    public class ShippingLabelDTO
    {
        [JsonProperty("shipmentNumber")]
        public string? ShipmentNumber { get; set; }

        [JsonProperty("previousShipmentNumber")]
        public string? PreviousShipmentNumber { get; set; }

        [JsonProperty("previousShipmentReceiverPhone")]
        public string? PreviousShipmentReceiverPhone { get; set; }

        [JsonProperty("senderClient")]
        public ClientProfileDTO? SenderClient { get; set; }

        [JsonProperty("senderAgent")]
        public ClientProfileDTO? SenderAgent { get; set; }

        [JsonProperty("senderAddress")]
        public AddressDTO? SenderAddress { get; set; }

        [JsonProperty("senderOfficeCode")]
        public string? SenderOfficeCode { get; set; }

        [JsonProperty("emailOnDelivery")]
        public string? EmailOnDelivery { get; set; }

        [JsonProperty("smsOnDelivery")]
        public string? SmsOnDelivery { get; set; }

        [JsonProperty("receiverClient")]
        public ClientProfileDTO? ReceiverClient { get; set; }

        [JsonProperty("receiverAgent")]
        public ClientProfileDTO? ReceiverAgent { get; set; }

        [JsonProperty("receiverAddress")]
        public AddressDTO? ReceiverAddress { get; set; }

        [JsonProperty("receiverOfficeCode")]
        public string? ReceiverOfficeCode { get; set; }

        [JsonProperty("receiverProviderID")]
        public int? ReceiverProviderID { get; set; }

        [JsonProperty("receiverBIC")]
        public string? ReceiverBIC { get; set; }

        [JsonProperty("receiverIBAN")]
        public string? ReceiverIBAN { get; set; }

        [JsonProperty("envelopeNumbers")]
        public string? EnvelopeNumbers { get; set; }

        [JsonProperty("packCount")]
        public int? PackCount { get; set; }

        [JsonProperty("packs")]
        public List<PackElementDTO>? Packs { get; set; }

        [JsonProperty("shipmentType")]
        public string? ShipmentType { get; set; }

        [JsonProperty("weight")]
        public double? Weight { get; set; }

        [JsonProperty("sizeUnder60cm")]
        public bool? SizeUnder60cm { get; set; }

        [JsonProperty("shipmentDimensionsL")]
        public double? ShipmentDimensionsL { get; set; }

        [JsonProperty("shipmentDimensionsW")]
        public double? ShipmentDimensionsW { get; set; }

        [JsonProperty("shipmentDimensionsH")]
        public double? ShipmentDimensionsH { get; set; }

        [JsonProperty("shipmentDescription")]
        public string? ShipmentDescription { get; set; }

        [JsonProperty("orderNumber")]
        public string? OrderNumber { get; set; }

        [JsonProperty("sendDate")]
        public int? SendDate { get; set; }

        [JsonProperty("holidayDeliveryDay")]
        public string? HolidayDeliveryDay { get; set; }

        [JsonProperty("keepUpright")]
        public bool? KeepUpright { get; set; }

        [JsonProperty("services")]
        public ShippingLabelServicesDTO? Services { get; set; }

        [JsonProperty("instructions")]
        public List<InstructionDTO>? Instructions { get; set; }

        [JsonProperty("payAfterAccept")]
        public bool? PayAfterAccept { get; set; }

        [JsonProperty("payAfterTest")]
        public bool? PayAfterTest { get; set; }

        [JsonProperty("packingListType")]
        public string? PackingListType { get; set; }

        [JsonProperty("packingList")]
        public List<PackingListElementDTO>? PackingList { get; set; }

        [JsonProperty("partialDelivery")]
        public bool? PartialDelivery { get; set; }

        [JsonProperty("paymentSenderMethod")]
        public string? PaymentSenderMethod { get; set; }

        [JsonProperty("paymentReceiverMethod")]
        public string? PaymentReceiverMethod { get; set; }

        [JsonProperty("paymentReceiverAmount")]
        public double? PaymentReceiverAmount { get; set; }

        [JsonProperty("paymentReceiverAmountIsPercent")]
        public bool? PaymentReceiverAmountIsPercent { get; set; }

        [JsonProperty("paymentOtherClientNumber")]
        public string? PaymentOtherClientNumber { get; set; }

        [JsonProperty("mediator")]
        public string? Mediator { get; set; }

        [JsonProperty("paymentToken")]
        public string? PaymentToken { get; set; }

        [JsonProperty("customsList")]
        public List<CustomsListElementDTO>? CustomsList { get; set; }

        [JsonProperty("customsInvoice")]
        public string? CustomsInvoice { get; set; }

        public ShippingLabelDTO(
            ClientProfileDTO senderClient,
            AddressDTO senderAddress,
            ClientProfileDTO receiverClient,
            AddressDTO receiverAddress,
            int packCount,
            int weigth,
            ShipmentType shipmentType,
            string shipmentDescription,
            double orderPrice)
        {
            SenderClient = senderClient;
            SenderAddress = senderAddress;

            ReceiverClient = receiverClient;
            ReceiverAddress = receiverAddress;

            PackCount = packCount;
            Weight = weigth;

            ShipmentType = ShipmentTypeConverter.ConvertEnumToString(shipmentType);

            ShipmentDescription = shipmentDescription;

            Instructions =
            [
                new InstructionDTO()
            ];

            Services = new()
            {
                DeclaredValueCurrency = "BGN",
                DeclaredValueAmount = orderPrice
            };

            PaymentReceiverMethod = "cash";
        }

        public ShippingLabelDTO(
            ClientProfileDTO senderClient,
            AddressDTO senderAddress,
            ClientProfileDTO receiverClient,
            AddressDTO receiverAddress,
            int packCount,
            int weigth,
            ShipmentType shipmentType,
            string shipmentDescription,
            double orderPrice,
            bool smsNotification)
        {
            SenderClient = senderClient;
            SenderAddress = senderAddress;

            ReceiverClient = receiverClient;
            ReceiverAddress = receiverAddress;

            PackCount = packCount;
            Weight = weigth;

            ShipmentType = ShipmentTypeConverter.ConvertEnumToString(shipmentType);

            ShipmentDescription = shipmentDescription;

            Instructions =
            [
                new InstructionDTO()
            ];

            Services = new()
            {
                DeclaredValueCurrency = "BGN",
                DeclaredValueAmount = orderPrice,
                SmsNotification = smsNotification
            };

            PaymentReceiverMethod = "cash";
        }
    }
}
