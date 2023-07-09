using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Simulator : MonoBehaviour
{
    public GameObject UI;
    public GameObject Static;
    public long Tick;
    public GameObject Template => gameObject.GetComponentInParent<GameObject>();
    public IEnumerable<IEComponent> EComponents => Static.GetComponentsInChildren<IEComponent>()
        .Concat(Template.GetComponentsInChildren<IEComponent>());

    public bool Playing;

    void Play() {}
    
    void Start() {}

    void Update() {}
}
