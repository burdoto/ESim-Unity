using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

public class SolderPoint : MonoBehaviour
{
    [NotNull] public Wire Parent => gameObject.GetComponentInParent<Wire>();
    [NotNull] public List<Wire> Targets;
    [NotNull] public IEnumerable<Wire> Wires => Targets.Append(Parent);

    void Start() {}

    void Update() {}
}
