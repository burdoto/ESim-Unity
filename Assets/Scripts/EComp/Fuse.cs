namespace EComp
{
    public class Fuse : EComponent
    {
        public short TripAmperage = 16;
        public short RatedAmperage = 250;

        public override Types ComponentType => Types.Switch;

        public enum States : byte { Normal, Tripped }
    }
}
