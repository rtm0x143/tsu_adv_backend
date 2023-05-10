using SimpleBase;

namespace Backend.Converters;

public static class OrderNumberFormatter
{
    public const int SectionSize = 2;

    public static bool TryDecode(string orderNumberString, out ulong result)
    {
        result = default;
        if (orderNumberString.Length < SectionSize) return false;
        try
        {
            var base32 = orderNumberString.Replace("-", string.Empty);
            var decoded = Base32.Crockford.Decode(base32);

            byte[] resultBytes;
            switch (decoded.Length)
            {
                case sizeof(ulong):
                    resultBytes = decoded;
                    break;
                case > sizeof(ulong): return false;
                default:
                    resultBytes = new byte[sizeof(ulong)];
                    if (!BitConverter.IsLittleEndian)
                    {
                        Array.Copy(decoded, 0, resultBytes, sizeof(ulong) - decoded.Length, decoded.Length);
                        break;
                    }

                    decoded = decoded.Reverse().ToArray();
                    Array.Copy(decoded, resultBytes, decoded.Length);
                    break;
            }

            result = BitConverter.ToUInt64(resultBytes);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static string Encode(ulong value)
    {
        var base32 = Base32.Crockford.Encode(BitConverter.GetBytes(value));

        var significantCount = Math.Max(SectionSize,
            base32.Length - base32.Reverse().TakeWhile(ch => ch == '0').Count());
        significantCount += significantCount % SectionSize != 0 ? SectionSize - significantCount % SectionSize : 0;

        var resultChars = new char[significantCount + significantCount / SectionSize - 1];
        for (int i = 0, destInd = 0; i < significantCount; i++)
        {
            if (i > 0 && i % SectionSize == 0) resultChars[destInd++] = '-';
            resultChars[destInd++] = base32.Length > i ? base32[i] : '0';
        }

        return new string(resultChars);
    }
}