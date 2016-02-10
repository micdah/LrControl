namespace micdah.LrControl
{
    internal struct Int14
    {
        public Int14(int msb, int lsb)
        {
            MSB = msb;
            LSB = lsb;
        }

        public Int14(int value)
        {
            MSB = value >> 7;
            LSB = value & 0x7F;
        }

        public int MSB { get; set; }
        public int LSB { get; set; }

        public int Value
        {
            get
            {
                return MSB << 7 | LSB;
            }
            set
            {
                MSB = value >> 7;
                LSB = value & 0x7F;
            }
        }
    }
}