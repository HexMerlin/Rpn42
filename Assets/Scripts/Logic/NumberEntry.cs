using System;
using System.Numerics;
using MathLib;

public class NumberEntry
{
    public Q Q { get; }

    public static readonly NumberEntry Invalid = new NumberEntry(Q.NaN);

    public NumberEntry() : this(new Q(0)) { }
    
    public NumberEntry(BigInteger number) : this(new Q(number))
    {
    }

    public NumberEntry(Q q)
    {
        this.Q = q;
        StringCanonical = new Lazy<string>(() => q.ToStringCanonical());
        StringFactorization = new Lazy<string>(() => q.ToStringFactorization());
        Base2Entry = new Lazy<BaseEntry>(() => new BaseEntry(q, 2));
        Base3Entry = new Lazy<BaseEntry>(() => new BaseEntry(q, 3));
        Base5Entry = new Lazy<BaseEntry>(() => new BaseEntry(q, 5));
        Base7Entry = new Lazy<BaseEntry>(() => new BaseEntry(q, 7));
        Base10Entry = new Lazy<BaseEntry>(() => new BaseEntry(q, 10));

    }
    private Lazy<string> StringCanonical { get; }

    private Lazy<string> StringFactorization { get; }

    private Lazy<BaseEntry> Base2Entry { get; }
    
    private Lazy<BaseEntry> Base3Entry { get; }

    private Lazy<BaseEntry> Base5Entry { get; }

    private Lazy<BaseEntry> Base7Entry { get; }

    private Lazy<BaseEntry> Base10Entry { get; }


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

    private static string Col0Title(Format _) => "Canonical";

    private string Col0Data(Format _) => StringCanonical.Value;

    private static string Col1Title(Format format) 
        => format.Mode switch
    {
        Mode.Normal => "",
        Mode.Periodic => "",
        Mode.Repetend => "Repetend",
        Mode.Period => "Period",
        _ => "",
    };

    private string Col1Data(Format format) 
        => format.Mode switch
    {
        Mode.Normal => "",
        Mode.Periodic => "",
        Mode.Rotations => "",
        Mode.Factorization => "",
        Mode.Repetend => BaseEntry(format).StringRepetend.Value,
        Mode.Period => BaseEntry(format).StringPeriod.Value,
        _ => throw new ArgumentOutOfRangeException(nameof(Format), format, "Unknown format"),
    };

    private static string Col2Title(Format format) => format.Mode switch
    {
        Mode.Normal => "Expanded",
        Mode.Periodic => "Periodic",
        Mode.Rotations => "Rotations",
        Mode.Factorization => "Factors",
        Mode.Repetend => "Repetend Factors",
        Mode.Period => "Period Factors",
        _ => throw new ArgumentOutOfRangeException(nameof(Format), format, "Unknown format"),
    };

    private string Col2Data(Format format) => format.Mode switch
    {
        Mode.Normal => BaseEntry(format).StringExpanded.Value,
        Mode.Periodic => BaseEntry(format).StringPeriodic.Value,
        Mode.Rotations => BaseEntry(format).StringRotations.Value,
        Mode.Factorization => StringFactorization.Value,
        Mode.Repetend => BaseEntry(format).StringRepetendFactorization.Value,
        Mode.Period => BaseEntry(format).StringPeriodFactorization.Value,
        _ => throw new ArgumentOutOfRangeException(nameof(Format), format, "Unknown format"),
    };

    private BaseEntry BaseEntry(Format format) => format.Base switch
    {
        2 => Base2Entry.Value,
        3 => Base3Entry.Value,
        5 => Base5Entry.Value,
        7 => Base7Entry.Value,
        10 => Base10Entry.Value,
        _ => throw new ArgumentOutOfRangeException(nameof(Format), format, "Unknown base"),
    };


}


