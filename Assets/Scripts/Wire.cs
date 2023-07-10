#nullable enable
using System.Collections.Generic;
using System.Linq;
using EComp;

public class Wire : IEComponent
{
    public List<ContactPoint> Solders = new();
    public Potential? Potential;
    
    public override Types ComponentType => Types.Conductor;

    private void Awake()
    {
        Potential = GetComponent<Potential>();
    }

    public override void ComputeValues()
    {
        if (!Potential.IsNull())
        {
            SimState.From(Potential!);
            goto end;
        }

        foreach (var contactPoint in Solders)
            if (contactPoint.ContactType == ContactPoint.ContactTypes.Output)
            {
                var parent = contactPoint.transform.parent.GetComponent<IEComponent>();
                parent.ComputeVoltage();
                SimState = parent.SimState;
                goto end;
            }

        end:
        base.ComputeValues();
    }
}
