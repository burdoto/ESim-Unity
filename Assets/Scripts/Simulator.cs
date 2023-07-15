using UnityEngine;

public class Simulator : MonoBehaviour
{
    public static Simulator Instance { get; private set; }
    public bool Playing;

    private void Awake() => Instance = this;
    private void Play() {}
}
