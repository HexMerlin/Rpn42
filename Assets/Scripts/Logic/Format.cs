﻿

using System;

public struct Format : IEquatable<Format>
{
    public readonly Mode Mode;

    public readonly int Base;

    public Format(Mode mode, int base_)
    {
        this.Mode = mode;
        if (base_ is not 2 and not 3 and not 5 and not 10)
        {
            throw new ArgumentOutOfRangeException(nameof(base_), base_, "Base must be 2, 3, 5, or 10");
        }
        this.Base = base_;
    }

    public bool Equals(Format other) => Mode == other.Mode && Base == other.Base;
}

public enum Mode 
{ 
    Normal,    //Fractional, Expanded,
    Periodic, //Fractional, Normal,
    PAdic,
    Rotations, 
    Factorization,
    Repetend,
    Period, 
}

//public enum Format { Normal, Bin, Repetend, RotationsBin, Factor, Period, Partition }
