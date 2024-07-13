using System;
using System.Numerics;
using Unity.VisualScripting;

public static class BigIntegerExtensions
{
    public static bool IsOdd(this BigInteger value) => !value.IsEven;

    public static BigInteger Abs(this BigInteger value) => BigInteger.Abs(value);

    public static long GetBitLength(this BigInteger value)
    {
        // Handle the case where the value is zero or minus one
        if (value == BigInteger.Zero || value == BigInteger.MinusOne)
            return 0;

        // Get the bytes of the BigInteger and calculate the number of bits
        var bytes = value.ToByteArray();
        var highestByte = bytes[bytes.Length - 1];
        var bits = (bytes.Length - 1) * 8;
        bits += GetLeadingBitIndex(highestByte) + 1;

        // Adjust for negative values (two's complement)
        if (value.Sign < 0)
        {
            // Check if the value is a power of two, which in two's complement is represented as a single bit followed by zeros
            if (IsPowerOfTwo(highestByte) && Array.TrueForAll(bytes, b => b == 0 || b == highestByte))
            {
                bits--;
            }
        }

        return bits;
    }

    private static int GetLeadingBitIndex(byte b)
    {
        int index = 7;
        while (index >= 0 && (b & (1 << index)) == 0)
            index--;
        return index;
    }

    private static bool IsPowerOfTwo(byte b) => (b & (b - 1)) == 0;
}
