using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Simulator : MonoBehaviour
{
    public GameObject UI;
    public GameObject Static;
    public GameObject Template => transform.parent.gameObject;
    public IEnumerable<IEComponent> EComponents => Static.GetComponentsInChildren<IEComponent>()
        .Concat(Template.GetComponentsInChildren<IEComponent>());

    public bool Playing;

    void Play()
    {
        //if (UI.GetComponentByName<>(""))
    }
    
    void Start() {}

    void Update() {}
}
