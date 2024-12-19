using FarmFresh.Data.Models.Enums;

namespace FarmFresh.Data.Models.CustomConverters
{
    public static class InstructionTypeConverter
    {
        private static readonly Dictionary<string, InstructionType> _mapperStringToEnum = new()
        {
            { "take", InstructionType.Take },
            { "give", InstructionType.Give },
            { "return", InstructionType.Return },
            { "service", InstructionType.Service }
        };

        private static readonly Dictionary<InstructionType, string> _mapperEnumToString = new()
        {
            { InstructionType.Take, "take" },
            { InstructionType.Give, "give" },
            { InstructionType.Return, "return" },
            { InstructionType.Service, "service" }
        };

        public static InstructionType ConvertStringToEnum(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Value cannot be null or empty. ", nameof(value));

            var valueToLower = value.ToLower();

            if (_mapperStringToEnum.TryGetValue(valueToLower, out var instructionType))
                return instructionType;

            throw new ArgumentException($"Invalid shipment string equivalent : {valueToLower}");
        }

        public static string ConvertEnumToString(InstructionType value)
        {
            if (_mapperEnumToString.TryGetValue(value, out var instructionType))
                return instructionType;

            throw new ArgumentException($"Invalid shipment enum equivalent : {value}");
        }
    }
}
