using FarmFresh.Data.Models.Enums;

namespace FarmFresh.Data.Models.CustomConverters
{
    public static class ShipmentTypeConverter
    {
        private static readonly Dictionary<string, ShipmentType> _mapperStringToEnum = new()
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

        private static readonly Dictionary<ShipmentType, string> _mapperEnumToString = new()
        {
            { ShipmentType.Document, "document" },
            { ShipmentType.Pack, "pack" },
            { ShipmentType.PostPack, "post_pack" },
            { ShipmentType.Pallet, "pallet" },
            { ShipmentType.Cargo, "cargo" },
            { ShipmentType.DocumentPallet, "documentpallet" },
            { ShipmentType.BigLetter, "big_pallet" },
            { ShipmentType.SmallLetter, "small_letter" },
            { ShipmentType.MoneyTransfer, "money_transfer" },
            { ShipmentType.PostTransfer, "pp" }
        };

        public static ShipmentType ConvertStringToEnum(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Value cannot be null or empty. ", nameof(value));

            var valueToLower = value.ToLower();

            if(_mapperStringToEnum.TryGetValue(valueToLower, out var shipmentType))
                return shipmentType;

            throw new ArgumentException($"Invalid shipment string equivalent : {valueToLower}");
        }

        public static string ConvertEnumToString(ShipmentType value)
        {
            if (_mapperEnumToString.TryGetValue(value, out var shipmentType))
                return shipmentType;

            throw new ArgumentException($"Invalid shipment enum equivalent : {value}");
        }
    }
}
