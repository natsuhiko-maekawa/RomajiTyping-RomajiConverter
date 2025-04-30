using System;

namespace RomajiConverter
{
    [Flags]
    public enum RomanizationStyle : byte
    {
        None = 0,
        Default = JapanStyle | StartsWithX,
        JapanStyle = 1,
        HepburnStyle = 2,
        StartsWithL = 4,
        StartsWithX = 8,
    }
}