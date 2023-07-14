#nullable enable
namespace Wires
{
    public class Wire : AEComponent, IConductive
    {
        public override Types ComponentType => Types.Conductor;
    }
}