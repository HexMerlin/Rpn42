
using System;
using System.Numerics;

public partial class Rational
{
    public bool IsZero => Numerator.IsZero;

    public bool IsOne => Numerator == Denominator;


    public static implicit operator Rational(int a) => new(a);

    public static implicit operator Rational(BigInteger a) => new(a);

    public static bool operator ==(Rational a, Rational b) => a.Equals(b);
    public static bool operator !=(Rational a, Rational b) => !a.Equals(b);

    public bool Equals(Rational other) => Numerator.Equals(other.Numerator) && Denominator.Equals(other.Denominator);

    public int CompareTo(Rational other) => (Numerator * other.Denominator).CompareTo(other.Numerator * Denominator);

    public static bool operator <(Rational a, Rational b) => a.CompareTo(b) < 0;

    public static bool operator <=(Rational a, Rational b) => a.CompareTo(b) <= 0;
    public static bool operator >(Rational a, Rational b) => a.CompareTo(b) > 0;

    public static bool operator >=(Rational a, Rational b) => a.CompareTo(b) >= 0;

    public Rational Reciprocal => Numerator > BigInteger.Zero ? new Rational(Denominator, Numerator) : Invalid;

    public static Rational operator ++(Rational a) => new(a.Numerator + a.Denominator, a.Denominator);

    public static Rational operator --(Rational a) => new(a.Numerator - a.Denominator, a.Denominator);

    public static Rational operator -(Rational a) => new(-(a.Numerator), a.Denominator, false);

    public static Rational operator +(Rational a, Rational b) => new(a.Numerator * b.Denominator + b.Numerator * a.Denominator, a.Denominator * b.Denominator);


    public static Rational operator -(Rational a, Rational b) => new(a.Numerator * b.Denominator - b.Numerator * a.Denominator, a.Denominator * b.Denominator);

    public static Rational operator *(Rational a, Rational b) => new(a.Numerator * b.Numerator, a.Denominator * b.Denominator);

    public static Rational operator /(Rational a, Rational b) => new(a.Numerator * b.Denominator, a.Denominator * b.Numerator);

    /// <summary>
    /// Performs the modulus operation on two rational numbers
    /// </summary>
    /// <remarks>Formula: (an / ad) % (bn / bd) = (an * bd) % (ad * bn) / (ad * bd)</remarks>
    /// <param name="a">The left operand</param>
    /// <param name="b">The right operand</param>
    /// <returns>The modulus of a and b</returns>
    public static Rational operator %(Rational a, Rational b)
    {
        if (b.Numerator == 0)
            return Invalid; //cannot do modulus by zero

        BigInteger newNumerator = (a.Numerator * b.Denominator) % (a.Denominator * b.Numerator);
        BigInteger newDenominator = a.Denominator * b.Denominator;

        return new(newNumerator, newDenominator);
    }
    public static Rational operator >>(Rational a, int shift)
    {
        if (shift <= 0) return shift == 0 ? a : a << -shift;

        bool normalizeNeeded = a.Numerator.IsEven;
        return new(a.Numerator, a.Denominator << shift, normalizeNeeded);
    }

    public static Rational operator <<(Rational a, int shift)
    {
        if (shift <= 0) return shift == 0 ? a : a >> -shift;
        bool normalizeNeeded = a.Denominator.IsEven;
        return new(a.Numerator << shift, a.Denominator, normalizeNeeded);
    }

    public Rational Abs => new(BigInteger.Abs(Numerator), Denominator, false);

    public Rational DivideByMersenneCeiling()
    {
        BigInteger div = MersenneCeiling(Numerator);
        return this / div;

    }

    public static BigInteger MersenneCeiling(BigInteger num)
    {
        num = BigInteger.Abs(num);
        BigInteger ceiling = 1;

        while (ceiling <= num)
            ceiling = (ceiling << 1) + 1;

        return ceiling;
    }

    public override bool Equals(object obj) => obj is Rational other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Numerator, Denominator);


}