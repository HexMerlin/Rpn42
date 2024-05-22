
using System;
using System.Linq;
using System.Numerics;
using System.Text;

public partial struct Rational
{
    private static char? ToSeparatorChar(Separator separator) => separator switch
    {
        Separator.RadixPoint => '⠄',
        Separator.RepetendBegin => '⠁',
        Separator.RadixPointAndRepetendBegin => '⠅',
        //Separator.RepetendEnd => '…',
        _ => null
    };

    public string ToStringNormal() => Denominator == 1 ? Numerator.ToString() : $"{Numerator}/{Denominator}";


    public string ToStringPartition() => $"{string.Join(" ", Partition.Select(r => r.ToString()))}";

    public string ToStringBin()
    {
        StringBuilder sb = new StringBuilder();
        foreach ((Rational r, Separator s) in RotationsBin)
        {
            if (s != Separator.None)
                sb.Append(ToSeparatorChar(s));
            sb.Append(r >= Half ? '1' : '0');
        }
        return sb.ToString();
    }

    public string ToStringBalBin() => "Not implemented";

    public string ToStringRotationsBin()
    {
        StringBuilder sb = new StringBuilder();
        foreach ((Rational r, Separator s) in RotationsBin)
        {
            if (s != Separator.None)
                sb.Append(ToSeparatorChar(s));
            sb.Append(r.Denominator == Denominator ? (r.Numerator + "/") : r.ToString());
            sb.Append(' ');
        }
        return sb.ToString();

    }


    public string ToStringRotationsBalBin() => "Not implemented";

    public string ToStringRepInfo()
    {

        return $"P={Period}";
    }

    public override string ToString() => Denominator == 1 ? Numerator.ToString() : $"{Numerator}/{Denominator}";


}