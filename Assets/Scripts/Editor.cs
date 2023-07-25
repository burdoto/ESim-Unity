#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.Assertions;
using Wires;

public class Editor : MonoBehaviour
{
    public MonoBehaviour? Item;
    public MonoBehaviour? Origin;
    public MonoBehaviour? Relation;

    private static readonly Dictionary<string, GameObject> prefabs = new();
    private static readonly Dictionary<Type, string> prefabPaths = new()
    {
        {typeof(Contact),"Prefabs/Wires/Contact"},
        {typeof(Wire),"Prefabs/Wires/Wire"}
    };

    void Update()
    {
        T Make<T>(MonoBehaviour? parent = null) where T : Conductive
        {
            var type = typeof(T);
            string ErrMsg(string detail) => $"Cannot instantiate {type.Name}; {detail}";

            // find prefab in cache
            var path = prefabPaths[type] ?? throw new Exception(ErrMsg("prefab type unknown"));
            var prefab = (prefabs[path] ??= Resources.Load<GameObject>(path))
                         ?? throw new Exception(ErrMsg("could not load prefab"));

            // create gameobject
            var obj = Instantiate(prefab, parent?.transform ?? transform)
                  ?? throw new Exception(ErrMsg("instantiation failed"));
            // get core component (T)
            var core = obj.GetComponent<T>() ?? throw new Exception(ErrMsg("core component was not found"));

            // special initialization
            HandleMesh(core, parent);

            return core;
        }

        WireMesh HandleMesh(Conductive core, MonoBehaviour? arg)
        {
            var mesh = WireMesh.Find(core);
            switch (core, arg)
            {
                case (not null, Wire other):
                    return mesh.Merge(other);
                case (Wire w, Contact c):
                    var wTransform = w.transform;
                    wTransform.parent = transform;
                    c.transform.parent = wTransform;
                    mesh.Add(c);
                    break;
            }
            return mesh;
        }

        void ModWirePoints(Wire wire, Vector3? arg = null, bool append = true)
        {
            var lRen = wire.LineRenderer;
            var count = lRen.positionCount;
            var arr = new Vector3[count];
            Assert.AreEqual(count, lRen.GetPositions(arr));
            if (append)
                arr = (arg == null ? arr.SkipLast(1) : arr.Append(arg.Value)).ToArray();
            else arr[^1] = arg!.Value;
            lRen.SetPositions(arr);
        }

        var cam = Camera.current!;
        var cast = Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition), out var hit);
        var point = (cast ? hit.point : cam.ScreenToWorldPoint(Input.mousePosition) - cam.transform.position);
        var hover = cast ? hit.collider.gameObject : null;
        var lmb = Input.GetMouseButtonDown(0);
        var rmb = Input.GetMouseButtonDown(1);

        // editor interaction logic
        // switch statements in c# are absolutely superior
        switch (hover?.GetComponent<Conductive>(), Item, Origin, Relation, lmb, rmb)
        {
            case (_, _, Wire wire, _, _, true): // on RMB with wire placement
                // remove last wire point
                ModWirePoints(wire);
                goto stopHover;
            case (_, _, _, _, _, true): // on RMB
            case (null, _, _, _, _, _): // on no hover
                // destroy hover components
                stopHover:
                Item.Use(ComponentHolderProtocol.GameObject).Use(Destroy);
                Origin.Use(ComponentHolderProtocol.GameObject).Use(Destroy);
                Relation.Use(ComponentHolderProtocol.GameObject).Use(Destroy);
                break;
            case (Wire wire, null, null, null, _, _): // on hover wire
                // create hover preview of new contact
                Item = Make<Contact>();
                Origin = wire;
                break;
            case (_, Contact contact, Wire origin, _, true, _): // on LMB with preview contact
                // do place contact and switch to wire placement
                HandleMesh(origin, contact);
                Item = Make<Wire>(contact);
                break;
            case (_, Wire wire, _, _, true, _): // on LMB with wire placement
                // add new wire point
                ModWirePoints(wire, Vector3.zero);
                break;
            case (not Contact, Wire, _, _, _, _): // on wire placement
                goto doPosWire;
            case (Contact contact, Wire, _, _, _, _):// on hover contact with wire placement
                // snap wire to contact
                point = contact.transform.position;
                
                // reposition last point in wire to hover pos
                doPosWire:
                ModWirePoints((Item as Wire)!, point, false);
                break;
            case (_, not Wire, _, _, _, _): // on preview object
                // reposition preview item that is not a wire
                if (!Item.IsUnityNull())
                    Item!.transform.position = point;
                break;
        }
    }
}
