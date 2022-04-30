using PO_104.Core;
using PO_104.Core.Pudelko;

namespace PO_104.Console;

public static class PudelkoExtensions
{
    public static Pudelko Compress(this Pudelko p)
    {
        var cubeEdge = Math.Pow(p.Volume, (double) 1 / 3).Floor(3);
        return new Pudelko(cubeEdge, cubeEdge, cubeEdge);
    }
}