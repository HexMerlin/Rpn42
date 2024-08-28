//#nullable enable
//using System;
//using System.Numerics;

//public partial class Q
//{
//    public bool IsZero => Numerator.IsZero;

//    public bool IsOne => Numerator == Denominator;


//    public static implicit operator Q(int a) => new(a);

//    public static implicit operator Q(BigInteger a) => new(a);

//    public static bool operator ==(Q a, Q b) => a.Equals(b);
//    public static bool operator !=(Q a, Q b) => !a.Equals(b);

//    public bool Equals(Q other) => Numerator.Equals(other.Numerator) && Denominator.Equals(other.Denominator);

//    public int CompareTo(Q other) => (Numerator * other.Denominator).CompareTo(other.Numerator * Denominator);

//    public static bool operator <(Q a, Q b) => a.CompareTo(b) < 0;

//    public static bool operator <=(Q a, Q b) => a.CompareTo(b) <= 0;
//    public static bool operator >(Q a, Q b) => a.CompareTo(b) > 0;

//    public static bool operator >=(Q a, Q b) => a.CompareTo(b) >= 0;

//    public Q Reciprocal => Numerator > BigInteger.Zero ? new Q(Denominator, Numerator) : Invalid;

//    public static Q operator ++(Q a) => new(a.Numerator + a.Denominator, a.Denominator);

//    public static Q operator --(Q a) => new(a.Numerator - a.Denominator, a.Denominator);

//    public static Q operator -(Q a) => new(-(a.Numerator), a.Denominator, false);

//    public static Q operator +(Q a, Q b) => new(a.Numerator * b.Denominator + b.Numerator * a.Denominator, a.Denominator * b.Denominator);

//    public static Q operator -(Q a, Q b) => new(a.Numerator * b.Denominator - b.Numerator * a.Denominator, a.Denominator * b.Denominator);

//    public static Q operator *(Q a, Q b) => new(a.Numerator * b.Numerator, a.Denominator * b.Denominator);

//    public static Q operator /(Q a, Q b) => new(a.Numerator * b.Denominator, a.Denominator * b.Numerator);

//    /// <summary>
//    /// Performs the modulus operation on two rational numbers
//    /// </summary>
//    /// <remarks>Formula: (an / ad) % (bn / bd) = (an * bd) % (ad * bn) / (ad * bd)</remarks>
//    /// <param name="a">The left operand</param>
//    /// <param name="b">The right operand</param>
//    /// <returns>The modulus of a and b</returns>
//    public static Q operator %(Q a, Q b)
//    {
//        if (b.Numerator == 0)
//            return Invalid; //cannot do modulus by zero

//        BigInteger newNumerator = (a.Numerator * b.Denominator) % (a.Denominator * b.Numerator);
//        BigInteger newDenominator = a.Denominator * b.Denominator;

//        return new(newNumerator, newDenominator);
//    }

//    public static Q operator <<(Q a, int shift) =>
//     shift >= 0
//         ? new(a.Numerator << shift, a.Denominator, normalize: a.Numerator.IsEven)
//         : a >> -shift;

//    public static Q operator >>(Q a, int shift) =>
//        shift >= 0
//            ? new(a.Numerator, a.Denominator << shift, normalize: a.Denominator.IsEven)
//            : a << -shift;

//    public Q Square() => new Q(Numerator * Numerator, Denominator * Denominator, false);

//    public Q Abs => new(BigInteger.Abs(Numerator), Denominator, false);


//    public Q Pow(Q exponent) => exponent.TryCastToInt32(out int exponentInt) ? Pow(exponentInt) : Invalid;

//    public Q Pow(int exponent) => exponent switch
//    {
//        > 0 => new Q(BigInteger.Pow(Numerator, exponent), BigInteger.Pow(Denominator, exponent)),
//        < 0 => new Q(BigInteger.Pow(Denominator, -exponent), BigInteger.Pow(Numerator, -exponent)), //Numerator and Denominator are flipped
//        _ => Q.One,
//    };

//    public override bool Equals(object obj) => obj is Q other && Equals(other);
//    public override int GetHashCode() => HashCode.Combine(Numerator, Denominator);


//}