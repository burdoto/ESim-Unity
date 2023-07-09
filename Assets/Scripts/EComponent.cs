using JetBrains.Annotations;
using UnityEngine;

public abstract class EComponent : MonoBehaviour
{
    [CanBeNull] public Wire Input;
    [CanBeNull] public Wire Output;
    public byte State;
    
    public abstract Types ComponentType { get; }
    
    public enum Types { Switch, Consumer, Source }
}
