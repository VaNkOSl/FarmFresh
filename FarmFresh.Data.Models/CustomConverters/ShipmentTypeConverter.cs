using Newtonsoft.Json;
using FarmFresh.Data.Models.Enums;

namespace FarmFresh.Data.Models.CustomConverters
{
    public static class ShipmentTypeConverter
    {
        private static readonly Dictionary<string, ShipmentType> _mapping = new()
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

        public static ShipmentType Convert(string econtStringEqivalent)
        {
            if (string.IsNullOrEmpty(econtStringEqivalent))
                throw new ArgumentException("Value cannot be null or empty. ", nameof(econtStringEqivalent));

            if(_mapping.TryGetValue(econtStringEqivalent, out var shipmentType))
                return shipmentType;

            throw new ArgumentException($"Invalid shipment string equivalent : {econtStringEqivalent}");
        }
    }
}
