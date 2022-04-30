using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;

namespace PO_104.Core.Pudelko;

[ImmutableObject(true)]
public sealed class Pudelko : IEquatable<Pudelko>, IFormattable, IEnumerable<double>
{
    #region Private members

    private readonly Dimension _maxDimension = new(10);
    private readonly Dimension _defaultDimension = new(10, UnitOfMeasure.Centimeter);
    
    private readonly Dimension _a;
    private readonly Dimension _b;
    private readonly Dimension _c;

    #endregion

    #region Constructors

    public Pudelko()
    {
        _a = (Dimension) _defaultDimension.Clone();
        _b = (Dimension) _defaultDimension.Clone();
        _c = (Dimension) _defaultDimension.Clone();
    }

    public Pudelko(double? a = null, double? b = null, double? c = null, UnitOfMeasure unit = UnitOfMeasure.Meter)
    {
        A = a is null
            ? ((Dimension) _defaultDimension.Clone()).CalculateInDifferentUnit(unit)
            : new Dimension(a.Value, unit);

        B = b is null
            ? ((Dimension) _defaultDimension.Clone()).CalculateInDifferentUnit(unit)
            : new Dimension(b.Value, unit);

        C = c is null
            ? ((Dimension) _defaultDimension.Clone()).CalculateInDifferentUnit(unit)
            : new Dimension(c.Value, unit);
    }

    #endregion

    #region Public members

    public Dimension A
    {
        get => _a.CalculateInDifferentUnit();
        init
        {
            if (value.Value > 0 && value <= _maxDimension)
                _a = value;
            else
                throw new ArgumentOutOfRangeException();
        }
    }
    public Dimension B
    {
        get => _b.CalculateInDifferentUnit();
        init
        {
            if (value.Value > 0 && value <= _maxDimension) 
                _b = value;
            else
                throw new ArgumentOutOfRangeException();
        }
    }
    public Dimension C
    {
        get => _c.CalculateInDifferentUnit();
        init
        {
            if (value.Value > 0 && value <= _maxDimension) 
                _c = value;
            else
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public double Volume => Math.Round(A * B * C, 9);
    
    public double Area => Math.Round(2 * A * B + 2 * A * C + 2 * B* C, 6);
    

    #endregion
    
    #region Equality members

    public bool Equals(Pudelko? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return
            _a.Equals(other._a) && _b.Equals(other._b) && _c.Equals(other._c)
            || _a.Equals(other._b) && _b.Equals(other._a) && _c.Equals(other._c)
            || _a.Equals(other._c) && _b.Equals(other._b) && _c.Equals(other._a)
            || _a.Equals(other._a) && _b.Equals(other._c) && _c.Equals(other._b)
            || _a.Equals(other._b) && _b.Equals(other._c) && _c.Equals(other._a);
    }

    [DebuggerStepThrough]
    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || obj is Pudelko other && Equals(other);
    }

    [DebuggerStepThrough]
    public override int GetHashCode()
    {
        return HashCode.Combine(_a, _b, _c);
    }

    public static bool operator ==(Pudelko p1, Pudelko p2) => p1.Equals(p2);
    public static bool operator !=(Pudelko p1, Pudelko p2) => !(p1 == p2);

    #endregion

    #region Cast operators

    public static explicit operator double[](Pudelko p) => new double[] {p.A, p.B, p.C};

    public static implicit operator Pudelko(ValueTuple<int, int, int> dimensions) => new(dimensions.Item1,
        dimensions.Item2, dimensions.Item3, UnitOfMeasure.Millimeter);

    #endregion

    #region Addition operator

    public static Pudelko operator +(Pudelko p1, Pudelko p2)
    {
        var minVolume = double.MaxValue;
        (double a, double b, double c) minVolumeCombination = (0, 0, 0);

        for (var i = 0; i < 3; i++)
        {
            for (var j = 0; j < 3; j++)
            {
                var volume = 1d;
                
                volume *= p1[i] + p2[j];
                volume *= Math.Max(p1[(i + 1) % 3], p2[(j + 1) % 3]);
                volume *= Math.Max(p1[(i + 2) % 3], p2[(j + 2) % 3]);
                
                if (volume < minVolume)
                {
                    minVolume = volume;
                    minVolumeCombination = (p1[i] + p2[j], Math.Max(p1[(i + 1) % 3], p2[(j + 1) % 3]),
                        Math.Max(p1[(i + 2) % 3], p2[(j + 2) % 3]));
                }
            }
        }

        return new Pudelko(minVolumeCombination.a, minVolumeCombination.b, minVolumeCombination.c, p1._a.Unit);
    }

    #endregion

    #region IEnumerable implementation

    public IEnumerator<double> GetEnumerator()
    {
        yield return A;
        yield return B;
        yield return C;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }    

    #endregion
    
    #region Indexer
    public double this[int i]
    {
        get
        {
            if (i is < 0 or > 2)
                throw new ArgumentOutOfRangeException();

            return i switch
            {
                0 => A,
                1 => B,
                2 => C,
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }

    #endregion

    #region IFormattable implementation

    public string ToString(string? format, IFormatProvider? formatProvider = null)
    {
        formatProvider ??= CultureInfo.CurrentCulture;
        format ??= "G";

        return $"{A.ToString(format, formatProvider)} \u00d7 {B.ToString(format, formatProvider)} \u00d7 {C.ToString(format, formatProvider)}";
    }

    public override string ToString()
    {
        return ToString("G");
    }
    #endregion

    #region Parse

    public static Pudelko Parse(string s)
    {
        const string PUDELKO_STRING_REGEX_STRING = @"(?:(?:(?<meterValue>\d{1,2}\.\d{3})\s(?<meterUnit>m)(?:\s\u00d7\s)?)|(?:(?<centimeterValue>\d{1,4}\.\d{1})\s(?<centimeterUnit>cm)(?:\s\u00d7\s)?)|(?:(?<millimeterValue>\d{1,5})\s(?<millimeterUnit>mm)(?:\s\u00d7\s)?))";

        var regex = new Regex(PUDELKO_STRING_REGEX_STRING, RegexOptions.Compiled | RegexOptions.Multiline);

        if (string.IsNullOrEmpty(s))
            throw new ArgumentNullException();

        if (!regex.IsMatch(s))
            throw new FormatException();

        var matches = regex.Matches(s);

        if (matches.Count != 3)
            throw new FormatException();

        var dimensions = new Dimension[3];

        for (var i = 0; i < 3; i++)
        {
            var currentMatch = matches[i];

            string value;
            if (!string.IsNullOrEmpty(currentMatch.Groups["meterValue"].Value))
            {
                value = currentMatch.Groups["meterValue"].Value;
                dimensions[i] = new Dimension(double.Parse(value, CultureInfo.InvariantCulture));
            }
            else if (!string.IsNullOrEmpty(currentMatch.Groups["centimeterValue"].Value))
            {
                value = currentMatch.Groups["centimeterValue"].Value;
                dimensions[i] = new Dimension(double.Parse(value, CultureInfo.InvariantCulture), UnitOfMeasure.Centimeter);
            }
            else
            {
                value = currentMatch.Groups["millimeterValue"].Value;
                dimensions[i] = new Dimension(double.Parse(value, CultureInfo.InvariantCulture), UnitOfMeasure.Millimeter);
            }
        }
        
        return new Pudelko(dimensions[0].CalculateInDifferentUnit(), dimensions[1].CalculateInDifferentUnit(), dimensions[2].CalculateInDifferentUnit());
    }

    #endregion
}