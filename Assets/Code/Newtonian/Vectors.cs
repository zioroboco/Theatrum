using System;
using Space;
using Unity.Entities;
using Unity.Mathematics;

namespace Newtonian {

    [Serializable]
    public struct Vectors : IComponentData {
        public coords Position;
        public double2 Velocity;
    }

}
