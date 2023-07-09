using JetBrains.Annotations;
using UnityEngine;

public abstract class EComponent : MonoBehaviour, IEComponent
{
    [CanBeNull] public Wire Input;
    [CanBeNull] public Wire Output;
    public byte State;
    
    public abstract Types ComponentType { get; }
    
    public enum Types { Conductor, Switch, Consumer, Source }
}
