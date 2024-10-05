

using System;

public struct Format : IEquatable<Format>
{
    public readonly Mode Mode;

    public readonly int Base;

    public Format(Mode mode, int base_)
    {
        this.Mode = mode;
        if (base_ is not 2 and not 3 and not 5 and not 7 and not 10)
        {
            throw new ArgumentOutOfRangeException(nameof(base_), base_, "Base must be 2, 3, 5, 7, or 10");
        }
        this.Base = base_;
    }

    public bool Equals(Format other) => Mode == other.Mode && Base == other.Base;
}
