namespace Wires
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

        protected void Start()
        {
            WireMesh = GetComponent<WireMesh>() ?? Wire;
            WireMesh.Static = true;
        }
    }
}
