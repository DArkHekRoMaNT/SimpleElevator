namespace SimpleElevator
{
    public class Config
    {
        public ElevatorRangesConfig ElevatorRanges { get; set; } = new ElevatorRangesConfig();
        public float TemporalElevatorMultiplier { get; set; } = 2;

        public class ElevatorRangesConfig
        {
            public int Copper { get; set; } = 8;
            public int TinBronze { get; set; } = 16;
            public int BismuthBronze { get; set; } = 16;
            public int BlackBronze { get; set; } = 16;
            public int Gold { get; set; } = 32;
            public int Silver { get; set; } = 32;
            public int Iron { get; set; } = 64;
            public int MeteoricIron { get; set; } = 72;
            public int Steel { get; set; } = 96;
        }
    }
}