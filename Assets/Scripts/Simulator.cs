using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Simulator : MonoBehaviour
{
    public GameObject UI;
    public GameObject Static;
    public GameObject Template => gameObject.GetComponentInParent<GameObject>();
    
    public bool Playing;

    void Play() {}
    
    void Start() {}

    void Update() {}
}
