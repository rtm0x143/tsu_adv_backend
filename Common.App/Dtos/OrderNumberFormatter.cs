using System.Buffers;
using System.Buffers.Binary;
using OneOf;
using SimpleBase;

namespace Common.App.Dtos;

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
            var safeByteCountForDecoding = Base32.Crockford.GetSafeByteCountForDecoding(base32);
            var output = new Span<byte>(new byte[safeByteCountForDecoding < sizeof(ulong)
                ? sizeof(ulong)
                : safeByteCountForDecoding]);
            if (!Base32.Crockford.TryDecode(base32, output, out var numBytesWritten)
                || numBytesWritten > 8)
                return false;

            result = BitConverter.ToUInt64(output);
            result = BitConverter.IsLittleEndian ? result : BinaryPrimitives.ReverseEndianness(result);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public static string Encode(ulong value)
    {
        value = BitConverter.IsLittleEndian ? value : BinaryPrimitives.ReverseEndianness(value);
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

    /// <summary>
    /// Multiple strings can represent same order number, so that method re-encodes <paramref name="orderNumberString"/>
    /// </summary>
    public static OneOf<string, ArgumentException> ToInvariantString(string orderNumberString)
    {
        if (!TryDecode(orderNumberString, out var result))
            return new ArgumentException("Specified string wasn't valid order number", nameof(orderNumberString));
        return Encode(result);
    }
}