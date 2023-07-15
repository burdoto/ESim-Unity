#nullable enable
using UnityEngine.Serialization;

namespace Wires
{
    public class Contact : Conductive
    {
        public EComponent? Device;
        public Types type;
        public override Conductive.Types ComponentType => Conductive.Types.Conductor;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (Device != null) 
                Device = transform.parent.GetComponent<EComponent>();
        }
    
        public new enum Types { Joint = default, Input, Output }
    }
}
