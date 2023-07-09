namespace EComp
{
    public class Fuse : EComponent
    {
        public short TripAmperage = 16;
        public short RatedAmperage = 250;
    
        public enum States : byte { Normal, Tripped }
    }
}
