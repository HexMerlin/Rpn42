using MathLib;
using MathLib.Prime;
using System;

internal class BaseEntry
{
    
    public BaseEntry(Q q, int base_)
    {
        Qb = new Lazy<Qb>(() => new Qb(q, new Base(base_)));
        Qp = new Lazy<Qp>(() => CreateQp(q, base_));

        StringQpGenerator = new Lazy<string>(() => Qp.Value.Generator.ToStringCanonical());

        StringExpanded = new Lazy<string>(() => Qb.Value.ToStringExpanded());
        StringQpExpanded = new Lazy<string>(() => Qp.Value.Generator.ToStringExpanded());

        StringPeriodic = new Lazy<string>(() => Qb.Value.ToStringPeriodic());
        StringQpPeriodic = new Lazy<string>(() => Qp.Value.ToStringPeriodic());
        StringRotations = new Lazy<string>(() => Qb.Value.ToStringRotations());
      
        StringRepetend = new Lazy<string>(() => Qb.Value.ToStringRepetend());
        StringRepetendFactorization = new Lazy<string>(() => Primes.Factorization(Qb.Value.PeriodicPart.IntValue).ToString());
        StringPeriod = new Lazy<string>(() => Qb.Value.Period.ToString());
        StringPeriodFactorization = new Lazy<string>(() => Primes.Factorization(Qb.Value.Period).ToString());
    }

    private static Qp CreateQp(Q q, int base_)
    {
        try {
            return new Qp(q, new Base(base_));
            
        }
        catch 
        {            
            return MathLib.Qp.NaN; 
        }
    }
    internal Lazy<string> StringQpGenerator { get; }

    internal Lazy<Qb> Qb { get; }

    internal Lazy<Qp> Qp { get; }

    internal Lazy<string> StringExpanded { get; }
    internal Lazy<string> StringQpExpanded { get; }
    
    internal Lazy<string> StringPeriodic { get; }

    internal Lazy<string> StringQpPeriodic { get; }

    internal Lazy<string> StringRotations { get; }



    internal Lazy<string> StringRepetend { get; }

    internal Lazy<string> StringRepetendFactorization { get; }

    internal Lazy<string> StringPeriod { get; }

    internal Lazy<string> StringPeriodFactorization { get; }
    
}

