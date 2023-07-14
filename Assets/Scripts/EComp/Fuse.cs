﻿using System;

namespace EComp
{
    public class Fuse : EComponent
    {
        public short TripAmperage = 16;
        public short RatedAmperage = 250;

        public override Types ComponentType => Types.Switch;

        protected override PotentialInfo ComputeOutputPotential()
        {
            var output = GetOutputPotential();
            if (output.Amps > RatedAmperage)
                State = StateBroken;
            else if (output.Amps > TripAmperage)
                State = (byte)States.Tripped;
            return (States)State switch
            {
                States.Normal => GetInputPotential(),
                _ => PotentialInfo.Zero
            };
        }

        public enum States : byte { Normal, Tripped }
    }
}
