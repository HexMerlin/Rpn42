using MathLib;
using MathLib.Prime;
using System;

internal class BaseEntry
{
    
    public BaseEntry(Q q, int base_)
    {
        Qb = new Lazy<Qb>(() => new Qb(q, base_));
   
        StringExpanded = new Lazy<string>(() => Qb.Value.ToStringExpanded());
       
        StringPeriodic = new Lazy<string>(() => Qb.Value.ToStringPeriodic());
      
        StringRotations = new Lazy<string>(() => Qb.Value.ToStringRotations());
      
        StringRepetend = new Lazy<string>(() => Qb.Value.ToStringRepetend());
        StringRepetendFactorization = new Lazy<string>(() => Primes.Factorization(Qb.Value.PeriodicPart.IntValue).ToString());
        StringPeriod = new Lazy<string>(() => Qb.Value.Period.ToString());
        StringPeriodFactorization = new Lazy<string>(() => Primes.Factorization(Qb.Value.Period).ToString());
    }


    internal Lazy<Qb> Qb { get; }

    internal Lazy<string> StringExpanded { get; }
 
    internal Lazy<string> StringPeriodic { get; }

    internal Lazy<string> StringRotations { get; }

    internal Lazy<string> StringRepetend { get; }

    internal Lazy<string> StringRepetendFactorization { get; }

    internal Lazy<string> StringPeriod { get; }

    internal Lazy<string> StringPeriodFactorization { get; }

}

