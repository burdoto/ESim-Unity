#nullable enable
using System;

namespace Wires
{
    public class Wire : Conductive
    {
        public override Types ComponentType => Types.Conductor;
    }
}