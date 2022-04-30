using System.ComponentModel;
using System.Globalization;
using static PO_104.Core.Pudelko.UnitOfMeasure;

namespace PO_104.Core.Pudelko;

[ImmutableObject(true)]
public sealed class Dimension : Tuple<double, UnitOfMeasure>, IComparable<Dimension>, IEquatable<Dimension>, IFormattable, ICloneable
{
    #region Constants

    public const string MetersFormat = "M";
    public const string CentimetersFormat = "CM";
    public const string MillimetersFormat = "MM";

    #endregion

    #region Properties

    public double Value => Unit switch
    {
        Meter => Item1.Floor(3),
        Centimeter => Item1.Floor(1),
        Millimeter => Math.Round(Item1),
        _ => throw new ArgumentException(),
    };
    public UnitOfMeasure Unit => Item2;

    #endregion
    
    #region Constructors

    public Dimension(double item1, UnitOfMeasure item2 = Meter) : base(item1, item2) { }

    #endregion

    #region Public members

    public Dimension CalculateInDifferentUnit(UnitOfMeasure unit = Meter) => (Unit, unit) switch
    {
        (Meter, Centimeter) => new Dimension(Value * 100, Centimeter),
        (Meter, Millimeter) => new Dimension(Value * 1000, Millimeter),
        (Centimeter, Meter) => new Dimension(Value / 100),
        (Centimeter, Millimeter) => new Dimension(Value * 10, Millimeter),
        (Millimeter, Meter) => new Dimension(Value / 1000),
        (Millimeter, Centimeter) => new Dimension(Value / 10, Centimeter),
        _ => this,
    };

    #endregion

    #region Equality members
    public int CompareTo(Dimension? other)
    {
        if (other is null) return 1;
        
        var value1 = Unit switch
        {
            Meter => Value * 1000,
            Centimeter => Value * 10,
            _ => Value,
        };
        
        var value2 = other.Unit switch
        {
            Meter => other.Value * 1000,
            Centimeter => other.Value * 10,
            _ => other.Value,
        };

        return value1.CompareTo(value2);
    }

    public bool Equals(Dimension? other)
    {
        if (other is null) return false;

        var value1 = Unit switch
        {
            Meter => Value * 1000,
            Centimeter => Value * 10,
            _ => Value,
        };
        
        var value2 = other.Unit switch
        {
            Meter => other.Value * 1000,
            Centimeter => other.Value * 10,
            _ => other.Value,
        };

        return value1.Equals(value2);
    }
    
    public override bool Equals(object? obj)
    {
        return Equals(obj as Dimension);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value, Unit);
    }

    public static bool operator <(Dimension d1, Dimension d2) => d1.CompareTo(d2) < 0;
    public static bool operator >(Dimension d1, Dimension d2) => d1.CompareTo(d2) > 0;
    public static bool operator <=(Dimension d1, Dimension d2) => d1.CompareTo(d2) <= 0;
    public static bool operator >=(Dimension d1, Dimension d2) => d1.CompareTo(d2) >= 0;
    public static bool operator ==(Dimension d1, Dimension d2) => d1.Equals(d2);
    public static bool operator !=(Dimension d1, Dimension d2) => !(d1 == d2);

    #endregion

    #region Cast operators

    public static implicit operator double(Dimension d) => d.Value;

    #endregion
    
    #region IFormattable implementation
    public string ToString(string? format, IFormatProvider? formatProvider = null)
    {
        formatProvider ??= CultureInfo.CurrentCulture;
        format ??= "G";

        switch (format.ToUpperInvariant())
        {
            case MillimetersFormat:
                return $"{CalculateInDifferentUnit(Millimeter).Value.ToString("F0", formatProvider)} mm";
            
            case CentimetersFormat:
                return $"{CalculateInDifferentUnit(Centimeter).Value.ToString("F1", formatProvider)} cm";
            
            case "G":
            case MetersFormat:
                return $"{CalculateInDifferentUnit().Value.ToString("F3", formatProvider)} m";
            
            default:
                throw new FormatException($"The {format} format string is not supported.");
        }
    }

    public override string ToString()
    {
        return ToString("G");
    }

    public object Clone()
    {
        return new Dimension(Value, Unit);
    }

    #endregion
}