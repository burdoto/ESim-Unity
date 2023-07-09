using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    public IEnumerable<SolderPoint> Solders => gameObject.GetComponentsInChildren<SolderPoint>();
}
