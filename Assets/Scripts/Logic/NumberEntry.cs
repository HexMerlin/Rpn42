
using Newtonsoft.Json;
using System;
using System.Numerics;

[JsonObject(MemberSerialization.OptIn)]
public class NumberEntry
{

    public NumberEntry() : this(0) { }
    public NumberEntry(BigInteger number) : this(new Rational(number))
    {
    }

    [JsonConstructor]
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
        StringRepetendAsInteger = new Lazy<string>(() => rational.ToStringRepetendAsInteger());
    }


    [JsonProperty]
    public Rational Rational { get; }

    public static readonly NumberEntry Invalid = new NumberEntry(Rational.Invalid);




    public static string ColumnTitle(int columnIndex, Format format) => columnIndex switch
    {
        0 => Col0Title(format),
        1 => Col1Title(format),
        2 => Col2Title(format),
        _ => throw new ArgumentOutOfRangeException(nameof(columnIndex), columnIndex, "Unknown column"),
    };


    public string ColumnData(int columnIndex, Format format) => columnIndex switch
    {
        0 => Col0Data(format),
        1 => Col1Data(format),
        2 => Col2Data(format),
        _ =>  throw new ArgumentOutOfRangeException(nameof(columnIndex), columnIndex, "Unknown column"),
    };

    private static string Col0Title(Format _) => "Fraction";


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

    private static string Col1Title(Format numberFormat) => numberFormat switch
    {
        Format.Bin => "RepetendInt",
        _ => "Attr",
    };

    private string Col1Data(Format format) => format switch
    {
        Format.Normal => string.Empty,
        Format.Bin => StringRepetendAsInteger.Value,
        Format.BalBin => StringRepInfo.Value,
        Format.RotationsBin => StringRepInfo.Value,
        Format.RotationsBalBin => StringRepInfo.Value,
        Format.Partition => string.Empty,
        _ => throw new ArgumentOutOfRangeException(nameof(Format), format, "Unknown format"),
    };


    private static string Col2Title(Format numberFormat) => numberFormat switch
    {
        Format.Normal => "Decimal",
        Format.Bin => "Binary",
        Format.BalBin => "Bal Binary",
        Format.RotationsBin => "Rotations",
        Format.RotationsBalBin => "Rotations",
        Format.Partition => "Partitions",
        _ => throw new ArgumentException($"Unhandled format '{numberFormat}'"),
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

    private Lazy<string> StringRepetendAsInteger { get; }
    
    public override string ToString() => StringFraction.Value;

}
