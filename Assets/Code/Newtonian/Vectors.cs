using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Newtonian {

    [Serializable]
    public struct Vectors : IComponentData {
        public double2 Position;
        public double2 Velocity;
    }

}
