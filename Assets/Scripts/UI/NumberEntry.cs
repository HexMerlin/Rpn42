
using System;
using System.Numerics;

public enum Format { Normal, Bin, BalBin, RotationsBin, RotationsBalBin, Partition }
public class NumberEntry
{

    public NumberEntry(BigInteger number) : this(new Rational(number))
    {
    }

    public NumberEntry(Rational rational)
    {
        this.Rational = rational;
        StringFraction = new Lazy<string>(() => rational.ToStringFraction());
        StringDecimal = new Lazy<string>(() => rational.ToStringDecimal());
        StringBin = new Lazy<string>(() => rational.ToStringBin());
        StringBalBin = new Lazy<string>(() => rational.ToStringBalBin());
        StringRotationsBin = new Lazy<string>(() => rational.ToStringRotationsBin());
        StringRotationsBalBin = new Lazy<string>(() => rational.ToStringRotationsBalBin());
        StringPartition = new Lazy<string>(() => rational.ToStringPartition());
        StringRepInfo = new Lazy<string>(() => rational.ToStringRepInfo());
    }


    public Rational Rational { get; }

    public static readonly NumberEntry Invalid = new NumberEntry(Rational.Invalid);

    public string ColumnData(int columnIndex, Format format) => columnIndex switch
    {
        0 => Col0Data(format),
        1 => Col1Data(format),
        2 => Col2Data(format),
        _ =>  throw new ArgumentOutOfRangeException(nameof(columnIndex), columnIndex, "Unknown column"),
    };

    private string Col0Data(Format format) => format switch
    {
        Format.Normal => StringFraction.Value,
        Format.Bin => StringFraction.Value,
        Format.BalBin => StringFraction.Value,
        Format.RotationsBin => StringFraction.Value,
        Format.RotationsBalBin => StringFraction.Value,
        Format.Partition => StringFraction.Value,
        _ => throw new ArgumentOutOfRangeException(nameof(Format), format, "Unknown format"),
    };

    private string Col1Data(Format format) => format switch
    {
        Format.Normal => string.Empty,
        Format.Bin => StringRepInfo.Value,
        Format.BalBin => StringRepInfo.Value,
        Format.RotationsBin => StringRepInfo.Value,
        Format.RotationsBalBin => StringRepInfo.Value,
        Format.Partition => string.Empty,
        _ => throw new ArgumentOutOfRangeException(nameof(Format), format, "Unknown format"),
    };

    private string Col2Data(Format format) => format switch
    {
        Format.Normal => StringDecimal.Value,
        Format.Bin => StringBin.Value,
        Format.BalBin => StringBalBin.Value,
        Format.RotationsBin => StringRotationsBin.Value,
        Format.RotationsBalBin => StringRotationsBalBin.Value,
        Format.Partition => StringPartition.Value,
        _ => throw new ArgumentOutOfRangeException(nameof(Format), format, "Unknown format"),
    };

    private Lazy<string> StringFraction { get; }

    private Lazy<string> StringDecimal { get; }

    private Lazy<string> StringBin { get; }

    private Lazy<string> StringBalBin { get; }

    private Lazy<string> StringRotationsBin { get; }

    private Lazy<string> StringRotationsBalBin { get; }

    private Lazy<string> StringPartition { get; }

    private Lazy<string> StringRepInfo { get; }


    public override string ToString() => StringFraction.Value;

}
