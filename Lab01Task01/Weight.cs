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

public class Weight: IEquatable<Weight>, IComparable<Weight>
{
    public readonly double Value;
    public readonly WeightUnits Unit;

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
        if (obj is Weight weight)
        {
            return Equals(weight);
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Value, Unit);
    }

    /*public int CompareTo(Weight? other)
    {
        if (ReferenceEquals(this, other)) return 0;

        return 0;
    }*/

    public int CompareTo(Weight other)
    {
        if (other == null) return 1; // Zgodnie z konwencją, każdy obiekt jest większy niż null

        double thisInGrams = ToGram();
        double otherInGrams = other.ToGram();

        if (thisInGrams > otherInGrams) return 1; // obiekt jest większy
        if (thisInGrams < otherInGrams) return -1; // obiekt jest mniejszy

        return 0; // obiekty są równe
    }
}
