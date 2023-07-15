using System;

namespace Devices
{
    public class Switch : EComponent
    {
        public Types SwitchType;
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

        protected override PotentialInfo ComputeOutputPotential() => (SwitchType, (States)State) switch
        {
            (Types.NormallyOpen, States.Toggled)
                or (Types.NormallyClosed, States.Normal)
                or (Types.TwoWay, States.Toggled)
                => GetInputPotential(),
            (Types.TwoWay, States.Normal)
                => throw new NotImplementedException("Two-Way switch does not work right"),
            _ => PotentialInfo.Zero
        };

        public enum States : byte { Normal = default, Toggled }
        public new enum Types : byte { NormallyOpen, NormallyClosed, TwoWay }
    }
}
