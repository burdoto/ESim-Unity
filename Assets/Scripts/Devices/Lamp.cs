using UnityEngine;
using Wires;

namespace Devices
{
    public class Lamp : EComponent
    {
        public double Voltage = 230;
        public double Wattage = 10;

        public override Types ComponentType => Types.Consumer;

        protected override PotentialInfo ComputeOutputPotential()
        {
            var input = GetInputPotential();//.Next();
            
            var voltage = input % ((WireMesh)Output)!.FindPotential();
            if (voltage > Voltage * 1.05)
                State = StateBroken;
            else if (voltage < 0.001)
                State = StateNormal;
            else if (voltage < Voltage * 0.95)
                State = StateInsufficient;
            else State = StateActive;
            
            return State switch
            {
                StateActive => input.Push(Wattage),
                StateInsufficient => input,
                _ => PotentialInfo.Zero
            };
        }
    }
}
