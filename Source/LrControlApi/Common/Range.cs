namespace micdah.LrControlApi.Common
{
    public class Range
    {
        public Range(double minimum, double maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public double Minimum { get; }
        public double Maximum { get; }
    }
}