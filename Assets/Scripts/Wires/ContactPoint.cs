namespace Wires
{
    public class ContactPoint : AEComponent, IConductive
    {
        public ContactTypes ContactType;

        public override Types ComponentType => Types.Conductor;
    
        void Start() {}

        void Update() {}
    
        public enum ContactTypes { Joint = default, Input, Output }
    }
}
