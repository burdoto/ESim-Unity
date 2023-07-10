#nullable enable
using System;
using EComp;
using JetBrains.Annotations;
using UnityEngine;

public sealed class SimState
{
    public double Volts;
    public double Watts;
    public double Amps;
    public double Phi;

    public void From(Potential potential)
    {
        Volts = potential.Voltage;
        Phi = potential.PhaseShiftAngle;
    }
    
    public void Push(double volts, double watts)
    {
        // I = P / U
        Amps = (Watts = watts) / (Volts = volts);
    }
}

public abstract class IEComponent : MonoBehaviour
{
    public const byte StateBroken = 0xFF;
    public TickStates TickState;
    public SimState SimState = new();

    public abstract Types ComponentType { get; }

    private void FixedUpdate()
    {
        InitializeTick();
        if (TickState == TickStates.Init)
            ComputeValues();
        else if (TickState < TickStates.Values)
            Debug.unityLogger.LogError("IEComponent.FixedUpdate()",
                "Could not ComputeValues(); TickState was " + TickState);
        if (TickState == TickStates.Values)
            ComputeReaction();
        else if (TickState < TickStates.Reaction)
            Debug.unityLogger.LogError("IEComponent.FixedUpdate()",
                "Could not ComputeReaction(); TickState was " + TickState);
        if (TickState == TickStates.Reaction)
            ComputeOutput();
        else if (TickState < TickStates.Output)
            Debug.unityLogger.LogError("IEComponent.FixedUpdate()",
                "Could not ComputeOutput(); TickState was " + TickState);
    }
    
    public double ComputeVoltage()
    {
        if (TickState < TickStates.Init)
            InitializeTick();
        if (TickState < TickStates.Values)
            ComputeValues();
        return SimState.Volts;
    }

    protected void InitializeTick()
    {
        SimState = new SimState();
        TickState = TickStates.Init;
    }

    public virtual void ComputeValues()
    {
        // this call finalizes initial computations
        // we should collect all Current from Voltage and Wattage here
        TickState = TickStates.Values;
    }
    
    protected virtual void ComputeReaction()
    {
        // this call finalizes Current computations
        // if a fuse is installed and I_max < I, it should be tripped in this method
        TickState = TickStates.Reaction;
    }
    
    protected virtual void ComputeOutput()
    {
        // this call finalizes output computations
        // if this is a Lamp and no fuse tripped, it should light up
        TickState = TickStates.Output;
    }

    public enum TickStates
    {
        None = default,
        Init,     // After InitTick()
        Values,   // After ComputeCurrent()
        Reaction, // After ComputeReaction()
        Output    // After ComputeOutput()
    }
    
    public enum Types { Conductor, Switch, Consumer, Source }
}

public abstract class EComponent : IEComponent
{
    public Wire? Input;
    public Wire? Output;
    public byte State;
}
