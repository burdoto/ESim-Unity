using UnityEngine;

namespace Devices
{
    public class Lamp : EComponent
    {
        public double Voltage = 230;
        public double Wattage = 10;
        public Renderer Renderer;

        public override Types ComponentType => Types.Consumer;

        protected override void OnEnable()
        {
            base.OnEnable();
            Renderer = GetComponent<Renderer>();
        }

        protected override PotentialInfo ComputeOutputPotential()
        {
            var input = GetInputPotential().Next();
            
            /* TODO: state handling needs to be somewhere else
            var voltage = input % GetOutputPotential();
            if (voltage > Voltage * 1.05)
                State = StateBroken;
            else if (voltage < Voltage * 0.95)
                State = StateInsufficient;
            else State = StateNormal;
            */
            
            return State switch
            {
                StateNormal => input.Push(Wattage),
                StateInsufficient => input,
                _ => PotentialInfo.Zero
            };
        }
    }
}
