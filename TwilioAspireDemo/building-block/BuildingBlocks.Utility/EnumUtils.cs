namespace BuildingBlocks.Utility;

public static class EnumUtils
{
    public static TEnum GetEnumValue<TEnum>(this string value) where TEnum : struct, Enum
    {
        if (Enum.TryParse<TEnum>(value, true, out var result))
        {
            return result;
        }
        throw new ArgumentException($"Invalid value '{value}' for enum '{typeof(TEnum).Name}'.");
    }
}
