using Newtonsoft.Json;
using FarmFresh.Data.Models.Enums;

namespace FarmFresh.Data.Models.CustomConverters
{
    public class ShipmentTypeConverter : JsonConverter<ShipmentType>
    {
        private static readonly Dictionary<string, ShipmentType> _mapping = new Dictionary<string, ShipmentType>
        {
            { "document", ShipmentType.Document },
            { "pack", ShipmentType.Pack },
            { "post_pack", ShipmentType.PostPack },
            { "pallet", ShipmentType.Pallet },
            { "cargo", ShipmentType.Cargo },
            { "documentpallet", ShipmentType.DocumentPallet },
            { "big_letter", ShipmentType.BigLetter },
            { "small_letter", ShipmentType.SmallLetter },
            { "money_transfer", ShipmentType.MoneyTransfer },
            { "pp", ShipmentType.PostTransfer }
        };

        public override ShipmentType ReadJson(JsonReader reader, Type objectType, ShipmentType existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = reader.Value?.ToString().ToLower();

            if(_mapping.ContainsKey(value))
                return _mapping[value];

            throw new JsonSerializationException($"Invalid shipment type value: {value}");
        }
        public override void WriteJson(JsonWriter writer, ShipmentType value, JsonSerializer serializer)
        {
            string shipmentTypeStr = value switch
            {
                ShipmentType.Document => "document",
                ShipmentType.Pack => "pack",
                ShipmentType.PostPack => "post_pack",
                ShipmentType.Pallet => "pallet",
                ShipmentType.Cargo => "cargo",
                ShipmentType.DocumentPallet => "documentpallet",
                ShipmentType.BigLetter => "big_letter",
                ShipmentType.SmallLetter => "small_letter",
                ShipmentType.MoneyTransfer => "money_transfer",
                ShipmentType.PostTransfer => "pp",
                _ => throw new JsonSerializationException($"Unknown shipping type: {value}")
            };
        }
    }
}
