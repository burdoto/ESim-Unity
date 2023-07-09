namespace EComp
{
    public class Lamp : EComponent
    {
        public short Wattage = 10;

        public override Types ComponentType => Types.Consumer;
    }
}
