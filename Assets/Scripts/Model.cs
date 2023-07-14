﻿#nullable enable
using System;
using EComp;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Serialization;
using Wires;
using ContactPoint = Wires.ContactPoint;

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
    [FormerlySerializedAs("PhaseShift")] [FormerlySerializedAs("Phi")]
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
        throw new NotImplementedException();
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

    protected virtual void Awake() => Simulator = GetComponentInParent<Simulator>(true);

    public enum Types { Conductor, Switch, Consumer, Source }
}

public abstract class EComponent : Conductive
{
    public ContactPoint? Input;
    public ContactPoint? Output;
    public byte State;
    public PotentialInfo? InputPotential;
    public PotentialInfo? OutputPotential;
    
    protected virtual void LateUpdate() => InputPotential = OutputPotential = null;

    public PotentialInfo GetInputPotential() => InputPotential ??= ((WireMesh?)Input)?.FindPotential() ?? PotentialInfo.Zero;
    public PotentialInfo GetOutputPotential() => OutputPotential ??= ComputeOutputPotential();
    protected abstract PotentialInfo ComputeOutputPotential();
}
