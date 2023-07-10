using System;

namespace EComp
{
    public class Potential : EComponent
    {
        public bool Neutral;
        public bool Ground;
        public double Voltage = 230;
        public double MaxAmperage = 500;
        public short PhaseShiftAngle;

        public override Types ComponentType => Types.Source;
        public Wire Wire => gameObject.GetComponent<Wire>();
        
        private void Awake()
        {
            Input = Output = Wire;
        }
    }
}
