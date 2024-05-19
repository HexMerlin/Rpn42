
using System;
using System.Numerics;

public enum Format { Normal, Mixed, Bin, BalBin, RotationsBin, RotationsBalBin }
public class NumberEntry
{

    public NumberEntry(BigInteger number) : this(new Rational(number))
    {
    }

    public NumberEntry(Rational rational)
    {
        this.Rational = rational;
        StringNormal = new Lazy<string>(() => rational.ToStringNormal());
        StringMixed = new Lazy<string>(() => rational.ToStringMixed());
        StringBin = new Lazy<string>(() => rational.ToStringBin());
        StringBalBin = new Lazy<string>(() => rational.ToStringBalBin());
        StringRotationsBin = new Lazy<string>(() => rational.ToStringRotationsBin());
        StringRotationsBalBin = new Lazy<string>(() => rational.ToStringRotationsBalBin());
        StringRepInfo = new Lazy<string>(() => rational.ToStringRepInfo());
    }



    public Rational Rational { get; }

    public string Col0Data(Format format) => format switch
    {
        Format.Normal => string.Empty,
        Format.Mixed => string.Empty,
        Format.Bin => StringNormal.Value,
        Format.BalBin => StringNormal.Value,
        Format.RotationsBin => StringNormal.Value,
        Format.RotationsBalBin => StringNormal.Value,
        _ => throw new ArgumentOutOfRangeException(nameof(Format), format, "Unknown format"),
    };

    public string Col1Data(Format format) => format switch
    {
        Format.Normal => string.Empty,
        Format.Mixed => string.Empty,
        Format.Bin => StringRepInfo.Value,
        Format.BalBin => StringRepInfo.Value,
        Format.RotationsBin => StringRepInfo.Value,
        Format.RotationsBalBin => StringRepInfo.Value,
        _ => throw new ArgumentOutOfRangeException(nameof(Format), format, "Unknown format"),
    };

    public string Col2Data(Format format) => format switch
    {
        Format.Normal => StringNormal.Value,
        Format.Mixed => StringMixed.Value,
        Format.Bin => StringBin.Value,
        Format.BalBin => StringBalBin.Value,
        Format.RotationsBin => StringRotationsBin.Value,
        Format.RotationsBalBin => StringRotationsBalBin.Value,
        _ => throw new ArgumentOutOfRangeException(nameof(Format), format, "Unknown format"),
    };

    private Lazy<string> StringNormal { get; }

    private Lazy<string> StringMixed { get; }

    private Lazy<string> StringBin { get; }

    private Lazy<string> StringBalBin { get; }

    private Lazy<string> StringRotationsBin { get; }

    private Lazy<string> StringRotationsBalBin { get; }

    private Lazy<string> StringRepInfo { get; }


    public override string ToString() => StringNormal.Value;

}
