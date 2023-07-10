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

        private void Awake()
        {
            Renderer = GetComponent<Renderer>();
        }

        public override void ComputeValues()
        {
            // can input wire handle 10W?
            // get input wire
            var input = Input;
            if (input.IsNull()) goto end;
            
            // get input voltage
            var volts = input!.ComputeVoltage();
            SimState.Push(volts, Wattage);
            
            end:
            base.ComputeValues();
        }

        protected override void ComputeReaction()
        {
            // break lamp if U > U_max
            if (SimState.Volts > Voltage * 1.1)
                State = StateBroken;
            
            base.ComputeReaction();
        }

        protected override void ComputeOutput()
        {
            //Renderer.material = Materials.LampBroken;
            base.ComputeOutput();
        }
    }
}
