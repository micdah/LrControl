using System;

namespace LrControl.Api.Common
{
    public class Range
    {
        public Range(double minimum, double maximum)
        {
            if (maximum <= minimum) throw new ArgumentException("Maximum must be larger than minimum", nameof(maximum));

            Minimum = minimum;
            Maximum = maximum;
        }

        public Range()
        {
        }

        public double Minimum { get; set; }
        public double Maximum { get; set; }

        public bool IsWithin(double value)
        {
            return value <= Maximum && value >= Minimum;
        }

        public double FromRange(Range range, double value)
        {
            if (!range.IsWithin(value))
                throw new ArgumentException($"{value} is not within {range}");

            return Minimum + (Maximum - Minimum)*((value - range.Minimum)/(range.Maximum - range.Minimum));
        }

        public double ClampToRange(double value)
        {
            if (value < Minimum)
                return Minimum;
            if (value > Maximum)
                return Maximum;
            return value;
        }

        public override string ToString()
        {
            return $"[{Minimum},{Maximum}]";
        }
    }
}