using System.ComponentModel;
using System.Diagnostics;
using AggregateException = System.AggregateException;

namespace Lab01Task01;
// Hubert Haładyna 15313
public enum WeightUnits
{
    G = 1,
    DAG = 10,
    KG = 1000,
    T = 1000000,
    LB = 454,
    OZ = 28
}
// Wiem że funty i uncje nie działają ;')
public class Weight: IEquatable<Weight>, IComparable<Weight>
{
    public double Value { get; init; }
    public WeightUnits Unit { get; init; }

    private Weight()
    {
        Value = 0;
        Unit = WeightUnits.G;
    }
    private Weight(double value, WeightUnits unit)
    {
        Value = value;
        Unit = unit;
    }
    public static Weight Of(double value, WeightUnits unit)
    {
        if((value >= 0)&&(Enum.IsDefined(typeof(WeightUnits), unit)))
        {
            return new Weight(value, unit);
        }
        else
        { throw new ArgumentException(); }
    }
    public static Weight Parse(string input)
    {
        string[] part = input.Split(' ');
        WeightUnits unit;
        if (!Enum.TryParse(part[1], true, out unit))
        { throw new ArgumentException("Nieznana jednostka masy!"); }

        double value;
        if (!double.TryParse(part[0], out value))
        { throw new ArgumentException("Niepoprawny format liczby określającej masę!"); }
        if (value < 0)
        { throw new ArgumentException("Ujemna wartość masy!"); }
        return Of(value, unit);
    }
    private double ToGram()
    {
        return Value * (double)Unit;
    }
    public bool Equals(Weight? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        
        if (Value == other.Value && Unit == other.Unit)
        {
            return true; 
        }
        else
        {
            double firstValue = ToGram();
            double secondValue = other.ToGram();
            
            return firstValue.Equals(secondValue);
        }
    }
    public override bool Equals(object obj)
    {
        return Equals(obj as Weight);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(Value, Unit);
    }
    public int CompareTo(Weight other)
    {
        if (other == null) return 1; 

        double thisInGrams = ToGram();
        double otherInGrams = other.ToGram();

        if (thisInGrams > otherInGrams) return 1; 
        if (thisInGrams < otherInGrams) return -1; 

        return 0; 
    }
    // Operator większy >
    public static bool operator >(Weight left, Weight right)
    {
        return left.CompareTo(right) > 0;
    }
    // Oprator mniejszy <
    public static bool operator <(Weight left, Weight right)
    {
        return left.CompareTo(right) < 0;
    }
    // Operator większy lub równy >=
    public static bool operator >=(Weight left, Weight right)
    {
        return left.CompareTo(right) >= 0;
    }
    // Operator mniejszy lub równy <=
    public static bool operator <=(Weight left, Weight right)
    {
        return left.CompareTo(right) <= 0;
    }
    // Operator równy ==
    public static bool operator ==(Weight left, Weight right)
    {
        if (object.ReferenceEquals(left, null))
        {
            return object.ReferenceEquals(right, null);
        }

        return left.Equals(right);
    }
    // Operator różny !=
    public static bool operator !=(Weight left, Weight right)
    {
        return !(left == right);
    }
}