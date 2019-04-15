using System;
using System.Reflection;
using Space;
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

    private Entity _entity;
    private EntityManager _manager;

    public void Convert(
        Entity entity,
        EntityManager manager,
        GameObjectConversionSystem conversionSystem
    ) {
        this._entity = entity;
        this._manager = World.Active.GetOrCreateManager<EntityManager>();

        var position3 = GetComponent<Transform>().position;
        var position2 = new Vector2(position3.x, position3.y);

        var polarPosition = new coordinates(new double2(position2));
        var polarVelocity = polarPosition.PolarTransform(new double2(velocity));

        var vectors = new Newtonian.Vectors {
            Position = polarPosition,
            Velocity = polarVelocity
        };

        if (this.regime == Regime.Newtonian) {

            logInitialState(gameObject.name, vectors);
            manager.AddComponentData(entity, vectors);

        } else {
            throw new NotImplementedException();
        }
    }

    public void Update() {
        var r = _manager.GetComponentData<Translation>(this._entity).Value;
        transform.position = new Vector3(r.x, r.y, 0f);
    }

    private static void logInitialState<T>(string name, T data) {
        var fieldsInfo = typeof(T).GetFields(BindingFlags.Public | BindingFlags.Instance);
        var fieldLines = new String[fieldsInfo.Length];
        var i = 0;
        foreach (var field in fieldsInfo) {
            fieldLines[i] = $"{field.Name} = {field.GetValue(data)}";
            i++;
        }
        Debug.Log(
            $"Initialising {name} with {typeof(T)}: {string.Join("; ", fieldLines)}"
        );
    }
}
