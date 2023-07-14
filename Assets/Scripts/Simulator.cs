using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Simulator : MonoBehaviour
{
    public GameObject UI;
    public GameObject Static;
    public bool Playing;
    
    public GameObject Template => gameObject;
    public IEnumerable<AEComponent> EComponents => Static.GetComponentsInChildren<AEComponent>()
        .Concat(Template.GetComponentsInChildren<AEComponent>());

    void Play()
    {
        //if (UI.GetComponentByName<>(""))
    }
    
    void Start() {}

    void Update() {}
}
