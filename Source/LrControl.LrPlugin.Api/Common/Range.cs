using System;

namespace LrControl.LrPlugin.Api.Common
{
    public class Range
    {
        private const double ComparisonTolerance = 0.01d;
        
        public Range(double minimum, double maximum)
        {
            if (maximum < minimum) throw new ArgumentException("Maximum must be larger than minimum", nameof(maximum));

            Minimum = minimum;
            Maximum = maximum;
        }

        public Range() : this(0,0)
        {
        }

        public double Minimum { get; set; }
        public double Maximum { get; set; }

        public bool IsWithin(double value)
        {
            return value <= Maximum && value >= Minimum;
        }

        public bool IsMaximum(double value)
        {
            return Math.Abs(Maximum - value) < ComparisonTolerance;
        }

        public bool IsMinimum(double value)
        {
            return Math.Abs(Minimum - value) < ComparisonTolerance;
        }

        public double FromRange(Range range, double value)
        {
            if (!range.IsWithin(value))
                throw new ArgumentException($"{value} is not within {range}");

            return Minimum + (Maximum - Minimum)*((value - range.Minimum)/(range.Maximum - range.Minimum));
        }

        public override string ToString()
        {
            return $"[{Minimum},{Maximum}]";
        }

        public static bool operator <(double value, Range range)
        {
            return value < range.Minimum;
        }

        public static bool operator <=(double value, Range range)
        {
            return value <= range.Minimum;
        }

        public static bool operator >(double value, Range range)
        {
            return value > range.Maximum;
        }

        public static bool operator >=(double value, Range range)
        {
            return value >= range.Maximum;
        }
    }
}