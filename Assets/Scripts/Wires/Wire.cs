#nullable enable
using System.Linq;
using UnityEngine;

namespace Wires
{
    [RequireComponent(typeof(LineRenderer))]
    public class Wire : Conductive
    {
        public override Types ComponentType => Types.Conductor;
        
        public LineRenderer LineRenderer = null!;
        public EdgeCollider2D LineCollider = null!;

        private void Awake()
        {
            LineRenderer = gameObject.GetComponent<LineRenderer>();
            LineCollider = gameObject.AddComponent<EdgeCollider2D>();
        }

        private void Update()
        {
            var linePositions = new Vector3[LineRenderer.positionCount];
            LineRenderer.GetPositions(linePositions);
            LineCollider.points = linePositions.Select(x => (Vector2)x).ToArray();
        }
    }
}