using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour, IEComponent
{
    public IEnumerable<SolderPoint> Solders => gameObject.GetComponentsInChildren<SolderPoint>();
    public EComponent.Types ComponentType => EComponent.Types.Conductor;
}
