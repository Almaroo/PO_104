using PO_104.Core.Pudelko;

namespace PO_104.Console;

public static class PudelkoSortComparisons
{
    public static int VolumeAreaEdgeComparison(Pudelko p1, Pudelko p2)
    {
        var retval = p1.Volume.CompareTo(p2.Volume);

        if (retval != 0)
            return retval;

        retval = p1.Area.CompareTo(p2.Area);
        
        if (retval != 0)
            return retval;

        return (p1.A.CalculateInDifferentUnit() + p1.B.CalculateInDifferentUnit() + p1.C.CalculateInDifferentUnit())
            .CompareTo(p2.A.CalculateInDifferentUnit() + p2.B.CalculateInDifferentUnit() + p2.C.CalculateInDifferentUnit());
    }
}