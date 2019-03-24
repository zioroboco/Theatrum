using System;
using Unity.Entities;
using Unity.Mathematics;

[Serializable]
public struct Vectors : IComponentData {
    public double2 Position;
    public double2 Velocity;
}
