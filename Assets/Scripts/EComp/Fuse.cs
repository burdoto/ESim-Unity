namespace EComp
{
    public class Fuse : EComponent
    {
        public short TripAmperage = 16;
        public short RatedAmperage = 250;

        public override Types ComponentType => Types.Switch;

        protected override void ComputeReaction()
        {
            if (SimState.Amps > RatedAmperage)
                State = StateBroken;
            else if (SimState.Amps > TripAmperage)
                State = (byte)States.Tripped;

            base.ComputeReaction();
        }

        public enum States : byte { Normal, Tripped }
    }
}
