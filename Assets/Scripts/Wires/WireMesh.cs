#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using EComp;

namespace Wires
{
    public class WireMesh : Conductive
    {
        public List<Wire> Wires = new();
        public List<ContactPoint> Contacts = new();
        public Potential? Potential;
        public IEnumerable<Conductive> Parts => Wires.Cast<Conductive>().Concat(Contacts);
        public override Types ComponentType => Types.Conductor;

        protected override void Awake()
        {
            base.Awake();
            Potential = GetComponent<Potential>();
            if (GetComponent<Wire>() is {} wire) Add(wire);
            if (GetComponent<ContactPoint>() is {} contact) Add(contact);
        }

        public PotentialInfo FindPotential()
        {
            PotentialInfo output = new();
            if (Potential != null)
                output = Potential;
            else
                foreach (var contact in Contacts.Where(it => it.ContactType == ContactPoint.ContactTypes.Output))
                {
                    var device = contact.Device;
                    if (device != null) continue;
                    output = device!.GetOutputPotential();
                    output.Attach(output);
                }
            return output;
        }

        public void Add(Wire wire)
        {
            if (wire != null && !Wires.Contains(wire))
                Wires.Add(wire);
        }
        public void Add(ContactPoint contact)
        {
            if (contact != null && !Contacts.Contains(contact))
                Contacts.Add(contact);
        }
        public void Merge(WireMesh other)
        {
            other.Wires.ForEach(Add);
            other.Contacts.ForEach(Add);
        }

        public static implicit operator WireMesh?(Wire? wire) => wire.Use(Find);
        public static implicit operator WireMesh?(ContactPoint? contact) => contact.Use(Find);

        public static WireMesh Find(Wire wire)
        {
            var yield = wire.Simulator.GetComponentsInChildren<WireMesh>()
                .FirstOrDefault(mesh => mesh.Wires.Contains(wire));
            if (yield != null)
                return yield;
            yield = wire.Simulator.gameObject.AddComponent<WireMesh>();
            yield.Add(wire);
            return yield;
        }
        public static WireMesh Find(ContactPoint point)
        {
            var yield = point.Simulator.GetComponentsInChildren<WireMesh>()
                .FirstOrDefault(mesh => mesh.Contacts.Contains(point));
            if (yield != null)
                return yield;
            yield = point.Simulator.gameObject.AddComponent<WireMesh>();
            yield.Add(point);
            return yield;
        }
    }
}