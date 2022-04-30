// See https://aka.ms/new-console-template for more information

using PO_104.Console;
using PO_104.Core.Pudelko;

var x = new Pudelko();
var y = new Pudelko(0.1, 0.1, 0.1);
// var box2 = new Pudelko(10.1, unit: UnitOfMeasure.Meter);

Console.WriteLine(x.ToString());
Console.WriteLine(x.ToString(Dimension.MetersFormat));
Console.WriteLine(x.ToString(Dimension.CentimetersFormat));
Console.WriteLine(x.ToString(Dimension.MillimetersFormat));

Console.WriteLine(x.Equals(y));


var p1 = new Pudelko(10, 15, 20, UnitOfMeasure.Centimeter);
var p2 = new Pudelko(100, 150, 200, UnitOfMeasure.Millimeter);

Console.WriteLine(p1.Equals(p2));

var p3 = new Pudelko(10, 15, 20, UnitOfMeasure.Centimeter);
var p4 = new Pudelko(20, 15, 10, UnitOfMeasure.Centimeter);

Console.WriteLine(p3.Equals(p4));

var p5 = new Pudelko(1, 2, 3);
var p6 = new Pudelko();

var p7 = p5.Compress();

var pudelka = new List<Pudelko> { p1, p2, p3, p4, p5, p6, p7 };

Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine("=== Nie posortowane ===");
Console.ResetColor();

foreach (var pudelko in pudelka)
{
    Console.WriteLine(pudelko);
}

pudelka.Sort(PudelkoSortComparisons.VolumeAreaEdgeComparison);

Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine("=== Posortowane ===");
Console.ResetColor();

foreach (var pudelko in pudelka)
{
    Console.WriteLine(pudelko);
}


Console.ForegroundColor = ConsoleColor.Red;
Console.WriteLine("=== Dodawanie ===");
Console.ResetColor();

var p8 = new Pudelko(1, 1, 1);
var p9 = new Pudelko(1, 1, 1);

var p10 = p8 + p9;


Console.WriteLine(p8);
Console.WriteLine(p9);
Console.WriteLine(p10);


var p11 = new Pudelko(1, 1, 1);
var p12 = new Pudelko(0.5, 0.5, 0.5);

var p13 = p11 + p12;

Console.WriteLine(p11);
Console.WriteLine(p12);
Console.WriteLine(p13);

var p14 = new Pudelko(0.4, 0.5, 0.6);
var p15 = new Pudelko(0.1, 0.2, 0.3);

var p16 = p14 + p15;

Console.WriteLine(p14);
Console.WriteLine(p15);
Console.WriteLine(p16);


var testParse = Pudelko.Parse("1.000 m × 1.0 cm × 1 mm");
var testParse2 = Pudelko.Parse("1.000 m × 1.000 m × 1.000 m");

Console.ReadKey();