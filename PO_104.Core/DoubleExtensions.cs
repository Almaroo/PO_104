namespace PO_104.Core;

public static class DoubleExtensions
{
    public static double Floor(this double d, int decimals) => Math.Floor(d * Math.Pow(10, decimals)) / Math.Pow(10, decimals);
}