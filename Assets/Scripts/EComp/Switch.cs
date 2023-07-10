using System;

namespace EComp
{
    public class Switch : EComponent
    {
        public Types Type;
        public bool Resting;

        public override EComponent.Types ComponentType => EComponent.Types.Switch;

        private void OnMouseDown()
        {
            State = (byte)(Resting && State == (byte)States.Toggled
                ? States.Normal
                : States.Toggled);
        }

        private void OnMouseUp()
        {
            if (!Resting && State == (byte)States.Toggled)
                State = (byte)States.Normal;
        }

        public override void ComputeValues()
        {
            if (State == (byte)States.Normal) goto end;
            
            var input = Input;
            if (input.IsNull()) goto end;

            input!.ComputeVoltage();
            SimState = input.SimState;

            end:
            base.ComputeValues();
        }

        public enum States : byte { Normal = default, Toggled }
        public new enum Types : byte { NormallyOpen, NormallyClosed, TwoWay, Cross }
    }
}
