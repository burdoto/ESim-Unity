using System;
using UnityEngine;

namespace EComp
{
    public class Lamp : EComponent
    {
        public double Voltage = 230;
        public double Wattage = 10;
        public Renderer Renderer;

        public override Types ComponentType => Types.Consumer;

        protected override void Awake()
        {
            base.Awake();
            Renderer = GetComponent<Renderer>();
        }

        private void Update()
        {
            throw new NotImplementedException();
        }

        protected override PotentialInfo ComputeOutputPotential()
        {
            var input = GetInputPotential().Next();
            var output = GetOutputPotential();
            var local = input % output;
                
            if (input.Volts > Voltage * 1.05)
                State = StateBroken;
            else if (input.Volts < Voltage * 0.95)
                State = StateInsufficient;
            else State = StateNormal;
            return State switch
            {
                StateNormal => input.Push(Wattage),
                StateInsufficient => input,
                _ => PotentialInfo.Zero
            };
        }
    }
}
