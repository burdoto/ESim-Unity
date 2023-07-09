namespace EComp
{
    public class Switch : EComponent
    {
        public Types Type;

        public override EComponent.Types ComponentType => EComponent.Types.Switch;
    
        public enum States : byte { Normal, Toggled }
        public new enum Types : byte { NormallyOpen, NormallyClosed, TwoWay, Cross }
    }
}
