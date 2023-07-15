#nullable enable
using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using Wires;

[Serializable]
public sealed class PotentialInfo
{
    public static readonly PotentialInfo Zero = new();
    public PotentialInfo? Parent;
    public bool Neutral;
    public bool Ground;
    public double Volts;
    public double Watts;
    public double Amps;
    public double PhaseShiftAngle;
    public bool IsZero => !Neutral && !Ground && Volts == 0 && Watts == 0 && Amps == 0 && PhaseShiftAngle == 0;

    public PotentialInfo Next() => new() { Parent = this };

    public static implicit operator PotentialInfo(Potential potential) => new()
    {
        Neutral = potential.Neutral,
        Ground = potential.Ground,
        Volts = potential.Voltage,
        PhaseShiftAngle = potential.PhaseShiftAngle
    };
    
    public static double operator %(PotentialInfo input, PotentialInfo output)
    {
        double? CheckCOM(Func<PotentialInfo,bool> flag) => (flag(input), flag(output)) switch
        {
            (false,true) => input.Volts,
            (true,false) => output.Volts,
            (true,true) => 0,
            _ => null
        };
        return CheckCOM(x=>x.Neutral)
               ?? CheckCOM(x=>x.Ground) 
               ?? input.Volts * Math.Sqrt(3 * (Math.Abs(output.PhaseShiftAngle - input.PhaseShiftAngle) / 120));
    }

    public PotentialInfo Push(double watts)
    {
        // I = P / U
        Amps = (Watts = watts) / Volts;
        return this;
    }

    public PotentialInfo Attach(PotentialInfo other)
    {
        Neutral = other.Neutral;
        Ground = other.Ground;
        if (IsZero)
        {
            Volts = other.Volts;
            Watts = other.Watts;
            Amps = other.Amps;
            PhaseShiftAngle = other.PhaseShiftAngle;
            return this;
        }
        throw new NotImplementedException();
    }
}

public abstract class Conductive : MonoBehaviour
{
    public const byte StateNormal = 0x00;
    public const byte StateInsufficient = 0xFE;
    public const byte StateBroken = 0xFF;
    public Simulator Simulator = null!;

    public abstract Types ComponentType { get; }

    protected virtual void OnEnable() => Simulator = GetComponentInParent<Simulator>(true);

    public enum Types { Conductor, Switch, Consumer, Source }
}

public abstract class EComponent : Conductive
{
    public Contact? Input;
    public Contact? Output;
    public byte State;
    public PotentialInfo? InputPotential;
    public PotentialInfo? OutputPotential;

    private void Update() => OutputPotential = GetOutputPotential();
    protected virtual void LateUpdate() => InputPotential = OutputPotential = null;

    public PotentialInfo GetInputPotential() => InputPotential ??= ((WireMesh?)Input)?.FindPotential() ?? PotentialInfo.Zero;
    public PotentialInfo GetOutputPotential() => OutputPotential ??= ComputeOutputPotential();
    protected abstract PotentialInfo ComputeOutputPotential();
}
