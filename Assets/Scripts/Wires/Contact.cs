#nullable enable
namespace Wires
{
    public class Contact : Conductive
    {
        public EComponent? Device;
        public Types Type;
        public override Conductive.Types ComponentType => Conductive.Types.Conductor;

        protected void Start()
        {
            if (Device == null && Type is Types.Input or Types.Output)
                Device = transform.parent.GetComponent<EComponent>();
        }

        public new enum Types { Joint = default, Input, Output }
    }
}
