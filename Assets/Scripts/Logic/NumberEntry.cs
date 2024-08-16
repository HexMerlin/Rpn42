
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
        StringRotationsBin = new Lazy<string>(() => rational.ToStringRotationsBin());
        StringPartition = new Lazy<string>(() => rational.ToStringPartition());
        StringPeriod = new Lazy<string>(() => rational.ToStringPeriod());
        StringRepetendAsInteger = new Lazy<string>(() => rational.ToStringRepetendAsInteger());
        StringFactorization = new Lazy<string>(() => rational.ToStringFactorization());
        StringRepetendFactorization = new Lazy<string>(() => Primes.Factorization(rational.RepetendAsInteger).ToString());
        StringPeriodFactorization = new Lazy<string>(() => Primes.Factorization(rational.Period).ToString());
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

    private string Col0Data(Format _) => StringFraction.Value;

    private static string Col1Title(Format numberFormat) => numberFormat switch
    {  
        Format.Bin or Format.Repetend or Format.Factor => "Repetend Int",
        Format.Period or Format.Partition => "Period",
        _ => "",
    };

    private string Col1Data(Format format) => format switch
    {
        Format.Bin or Format.Repetend or Format.Factor => StringRepetendAsInteger.Value,
        Format.Period or Format.Partition => StringPeriod.Value,
        _ => "",
    };

    private static string Col2Title(Format numberFormat) => numberFormat switch
    {
        Format.Normal => "Decimal",
        Format.Bin => "Binary",
        Format.Repetend => "Repetend factors",
        Format.RotationsBin => "Rotations",
        Format.Factor => "Factors",
        Format.Period => "Period factors",
        Format.Partition => "Partitions",
        _ => $"Unhandled format '{numberFormat}'",
    };


    private string Col2Data(Format format) => format switch
    {
        Format.Normal => StringDecimal.Value,
        Format.Bin => StringBin.Value,
        Format.Repetend => StringRepetendFactorization.Value,
        Format.RotationsBin => StringRotationsBin.Value,
        Format.Factor => StringFactorization.Value,
        Format.Period => StringPeriodFactorization.Value,
        Format.Partition => StringPartition.Value,
        _ => throw new ArgumentOutOfRangeException(nameof(Format), format, "Unknown format"),
    };

    private Lazy<string> StringFraction { get; }

    private Lazy<string> StringDecimal { get; }

    private Lazy<string> StringBin { get; }

    private Lazy<string> StringRotationsBin { get; }

    private Lazy<string> StringPartition { get; }

    private Lazy<string> StringPeriod { get; }

    private Lazy<string> StringRepetendAsInteger { get; }
    
    private Lazy<string> StringFactorization { get; }
    
    private Lazy<string> StringRepetendFactorization { get; }
    
    private Lazy<string> StringPeriodFactorization { get; }

    public override string ToString() => StringFraction.Value;

}
