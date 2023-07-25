#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

namespace Wires
{
    public class WireMesh : Conductive
    {
        public bool Static;
        public List<Wire> Wires = new();
        public List<Contact> Contacts = new();
        public Potential? Potential;
        public IEnumerable<Conductive> Parts => Wires.Cast<Conductive>().Concat(Contacts);
        public override Types ComponentType => Types.Conductor;

        protected void Start()
        {
            Potential = GetComponent<Potential>();
            if (GetComponent<Wire>() is {} wire) Add(wire);
            if (GetComponent<Contact>() is {} contact) Add(contact);
        }

        public PotentialInfo FindPotential()
        {
            if (!Potential.IsUnityNull())
                return Potential!;
            PotentialInfo? output = null;
            foreach (var contact in Contacts.Where(it => it.Type == Contact.Types.Output))
            {
                var device = contact.Device;
                if (device.IsUnityNull()) continue;
                if (output == null)
                    output = device!.GetOutputPotential();
                else output.Attach(output);
            }
            return output ?? new();//?.Next() ?? new();
        }

        public void Add(Wire wire)
        {
            if (wire != null && !Wires.Contains(wire))
                Wires.Add(wire);
        }
        public void Add(Contact contact)
        {
            if (contact != null && !Contacts.Contains(contact))
                Contacts.Add(contact);
        }
        public WireMesh Merge(WireMesh other)
        {
            if (this == other)
                return this;
            if (other.Static)
                return other.Merge(this);
            other.Wires.ForEach(Add);
            other.Contacts.ForEach(Add);
            Destroy(other.gameObject);
            return this;
        }

        [ContractAnnotation("null => null")]
        public static implicit operator WireMesh(Wire? wire) => wire.Use(Find)!;
        [ContractAnnotation("null => null")]
        public static implicit operator WireMesh(Contact? contact) => contact.Use(Find)!;

        [ContractAnnotation("null => null")]
        public static WireMesh Find(Conductive it) => it switch
        {
            Wire w => w,
            Contact c => c,
            null => null!,
            _ => throw new ArgumentOutOfRangeException(nameof(it), it.GetType(), "Invalid type for WireMesh")
        };
        [ContractAnnotation("null => null")]
        public static WireMesh Find(Wire wire)
        {
            if (wire.IsUnityNull())
                return null!;
            var yield = wire.GetComponentInParent<Simulator>()
                .GetComponentsInChildren<WireMesh>()
                .FirstOrDefault(mesh => mesh.Wires.Contains(wire));
            if (yield != null)
                return yield;
            yield = wire.gameObject.AddComponent<WireMesh>();
            yield.Add(wire);
            return yield;
        }
        [ContractAnnotation("null => null")]
        public static WireMesh Find(Contact point)
        {
            if (point.IsUnityNull())
                return null!;
            var yield = point.GetComponentInParent<Simulator>()
                .GetComponentsInChildren<WireMesh>()
                .FirstOrDefault(mesh => mesh.Contacts.Contains(point));
            if (yield != null)
                return yield;
            yield = point.gameObject.AddComponent<WireMesh>();
            yield.Add(point);
            return yield;
        }
    }
}