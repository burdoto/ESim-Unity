﻿namespace Wires
{
    public class Potential : Conductive
    {
        public bool Neutral;
        public bool Ground;
        public double Voltage = 230;
        public double MaxAmperage = 500;
        public short PhaseShiftAngle;
        public WireMesh WireMesh;

        public override Types ComponentType => Types.Source;
        public Wire Wire => gameObject.GetComponent<Wire>();

        protected override void Awake()
        {
            base.Awake();
            WireMesh = GetComponent<WireMesh>() ?? gameObject.AddComponent<WireMesh>();
        }
    }
}