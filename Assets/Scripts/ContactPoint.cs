using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class ContactPoint : IEComponent
{
    public Wire Parent => gameObject.GetComponentInParent<Wire>();
    public List<Wire> Targets;
    public IEnumerable<Wire> Wires => new[] { Parent }.Concat(Targets);
    public ContactTypes ContactType;

    public override Types ComponentType => Types.Conductor;
    
    void Start() {}

    void Update() {}
    
    public enum ContactTypes { Joint = default, Input, Output }
}
