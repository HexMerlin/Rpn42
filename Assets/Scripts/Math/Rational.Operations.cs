#nullable enable
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

    public static Rational operator <<(Rational a, int shift) =>
     shift >= 0
         ? new(a.Numerator << shift, a.Denominator, normalize: a.Numerator.IsEven)
         : a >> -shift;

    public static Rational operator >>(Rational a, int shift) =>
        shift >= 0
            ? new(a.Numerator, a.Denominator << shift, normalize: a.Denominator.IsEven)
            : a << -shift;

    public Rational Square() => new Rational(Numerator * Numerator, Denominator * Denominator, false);

    public Rational Abs => new(BigInteger.Abs(Numerator), Denominator, false);


    public Rational Pow(Rational exponent) => exponent.TryCastToInt32(out int exponentInt) ? Pow(exponentInt) : Invalid;

    public Rational Pow(int exponent) => exponent switch
    {
        > 0 => new Rational(BigInteger.Pow(Numerator, exponent), BigInteger.Pow(Denominator, exponent)),
        < 0 => new Rational(BigInteger.Pow(Denominator, -exponent), BigInteger.Pow(Numerator, -exponent)), //Numerator and Denominator are flipped
        _ => Rational.One,
    };

    public override bool Equals(object obj) => obj is Rational other && Equals(other);
    public override int GetHashCode() => HashCode.Combine(Numerator, Denominator);


}