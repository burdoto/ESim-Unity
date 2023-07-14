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
    public IEnumerable<Conductive> EComponents => Static.GetComponentsInChildren<Conductive>()
        .Concat(Template.GetComponentsInChildren<Conductive>());

    void Play()
    {
        //if (UI.GetComponentByName<>(""))
    }
    
    void Start() {}

    void Update() {}
}
