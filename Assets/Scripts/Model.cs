using JetBrains.Annotations;
using UnityEngine;

public abstract class EComponent : MonoBehaviour
{
    [CanBeNull] public Wire Input;
    [CanBeNull] public Wire Output;
    public byte State;
}
