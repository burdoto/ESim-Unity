#nullable enable
using System.Collections.Generic;
using System.Linq;
using EComp;

namespace Wires
{
    public class WireMesh : AEComponent, IConductive
    {
        public List<Wire> Wires;
        public List<ContactPoint> Contacts;
        public Potential? Potential;
        public IEnumerable<IConductive> Parts => Wires.Cast<IConductive>().Concat(Contacts);
        public override Types ComponentType => Types.Conductor;

        private void Awake()
        {
            Potential = GetComponent<Potential>();
        }

        public override void ComputeValues()
        {
            if (!Potential.IsNull())
            {
                PotentialInfo.From(Potential!);
            }
            else
                foreach (var contact in Contacts.Where(it => it.ContactType == ContactPoint.ContactTypes.Output))
                {
                    PotentialInfo output = new();//contact.ComputePotential();
                    PotentialInfo.Attach(output);
                }

            base.ComputeValues();
        }

        public void Add(Wire wire) => Wires.Add(wire);
        public void Add(ContactPoint contact) => Contacts.Add(contact);
        public void Merge(WireMesh other)
        {
            other.Wires.ForEach(Add);
            other.Contacts.ForEach(Add);
        }

        public static implicit operator WireMesh(Wire wire) => For(wire);
        public static implicit operator WireMesh(ContactPoint contact) => For(contact);

        public static WireMesh For(Wire wire)
        {
            var yield = wire.Simulator.GetComponentsInChildren<WireMesh>()
                .FirstOrDefault(mesh => mesh.Wires.Contains(wire));
            if (!yield.IsNull())
                return yield!;
            yield = wire.Simulator.gameObject.AddComponent<WireMesh>();
            yield.Add(wire);
            return yield;
        }
        public static WireMesh For(ContactPoint point)
        {
            var yield = point.Simulator.GetComponentsInChildren<WireMesh>()
                .FirstOrDefault(mesh => mesh.Contacts.Contains(point));
            if (!yield.IsNull())
                return yield!;
            yield = point.Simulator.gameObject.AddComponent<WireMesh>();
            yield.Add(point);
            return yield;
        }
    }
}