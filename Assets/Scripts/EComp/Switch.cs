namespace EComp
{
    public class Switch : EComponent
    {
        public Types Type;
    
        public enum States : byte { Normal, Toggled }
        public enum Types : byte { NormallyOpen, NormallyClosed, TwoWay, Cross }
    }
}
