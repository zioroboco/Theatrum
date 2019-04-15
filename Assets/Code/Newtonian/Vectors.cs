using System;
using Space;
using Unity.Entities;
using Unity.Mathematics;

namespace Newtonian {

    [Serializable]
    public struct Vectors : IComponentData {
        public coordinates Position;
        public double2 Velocity;
    }

}
