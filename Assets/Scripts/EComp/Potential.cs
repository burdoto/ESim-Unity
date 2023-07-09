using System;

namespace EComp
{
    public class Potential : EComponent
    {
        public bool Neutral;
        public bool Ground;
        public short Voltage = 230;
        public short PhaseShiftAngle;
        public short MaxAmperage = 500;

        public override Types ComponentType => Types.Source;
        public Wire Wire => gameObject.GetComponent<Wire>();
        
        private void Awake()
        {
            Input = Output = Wire;
        }
    }
}
