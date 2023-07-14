#nullable enable
namespace Wires
{
    public class ContactPoint : Conductive
    {
        public EComponent? Device;
        public ContactTypes ContactType;
        public override Types ComponentType => Types.Conductor;

        protected override void Awake()
        {
            base.Awake();
            if (Device != null) 
                Device = transform.parent.GetComponent<EComponent>();
        }
    
        public enum ContactTypes { Joint = default, Input, Output }
    }
}
