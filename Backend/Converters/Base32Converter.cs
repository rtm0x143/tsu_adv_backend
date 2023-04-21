using SimpleBase;

namespace Backend.Mappers;

public static class Base32Converter
{
    public static bool TryToLong(string base32, out long result)
    {
        result = default;
        try
        {
            var decoded = Base32.Crockford.Decode(base32);
            if (decoded.Length > 8) return false;

            var norm = (BitConverter.IsLittleEndian ? decoded.Reverse() : decoded)
                .Concat(Enumerable.Repeat((byte)0, 8 - decoded.Length))
                .ToArray();

            result = BitConverter.ToInt64(norm);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static string Encode(long value) =>
        Base32.Crockford.Encode(BitConverter.GetBytes(value)).Replace("0", String.Empty);
}