using System;
using System.Reflection;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[RequiresEntityConversion]
public class Particle : MonoBehaviour, IConvertGameObjectToEntity {

    private enum Regime {
        Newtonian
    }

    [SerializeField]
    private Regime regime = Regime.Newtonian;

    [SerializeField]
    private Vector2 velocity = new Vector2(0f, 0f);

    private Entity entity;
    private EntityManager manager;

    public void Convert(
        Entity entity,
        EntityManager manager,
        GameObjectConversionSystem conversionSystem
    ) {
        this.entity = entity;
        this.manager = World.Active.GetOrCreateManager<EntityManager>();

        var transform = GetComponent<Transform>();
        var position = new Vector2(transform.position.x, transform.position.y);

        var vectors = new Newtonian.Vectors {
            Position = new double2(position.x, position.y),
            Velocity = new double2(velocity.x, velocity.y)
        };

        if (this.regime == Regime.Newtonian) {

            logInitialState(gameObject.name, vectors);
            manager.AddComponentData(entity, vectors);

        } else {
            throw new NotImplementedException();
        }
    }

    public void Update() {
        var r = manager.GetComponentData<Translation>(this.entity).Value;
        transform.position = new Vector3(r.x, r.y, 0f);
    }

    static void logInitialState<T>(string name, T data) {
        FieldInfo[] fieldsInfo = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);
        string[] fieldLines = new String[fieldsInfo.Length];
        int i = 0;
        foreach (FieldInfo field in fieldsInfo) {
            fieldLines[i] = string.Format("{0} = {1}", field.Name, field.GetValue(data));
            i++;
        }
        Debug.Log(string.Format(
            "Initialising {0} with {1}: {2}", name, typeof(T), string.Join("; ", fieldLines)
        ));
    }
}
