using System;
using System.Diagnostics;
using Unity.Mathematics;

namespace Space {

  [DebuggerTypeProxy(typeof(state.DebuggerProxy))]
  [System.Serializable]
  public struct state : System.IEquatable<state>, IFormattable {

    public readonly coordinates position;
    public readonly double2 velocity;

    public state(coordinates position, double2 polarVelocity) {
      this.position = position;
      this.velocity = polarVelocity;
    }

    public bool Equals(state rhs) {
      return math.all(this.ToDouble4() == rhs.ToDouble4());
    }

    public override bool Equals(object o) {
      return o != null && Equals((state) o);
    }

    public double4 ToDouble4() {
      return ToDouble4(this);
    }

    public double4 ToDouble4(state s) {
      return new double4(
        this.velocity.x,
        this.velocity.y,
        this.position.r,
        this.position.theta.radians
      );
    }

    public override int GetHashCode() {
      return this.ToDouble4().GetHashCode();
    }

    public static bool operator ==(state lhs, state rhs) {
      return lhs.Equals(rhs);
    }

    public static bool operator !=(state lhs, state rhs) {
      return !lhs.Equals(rhs);
    }

    public override string ToString() {
      return $"{this.ToDouble4()}";
    }

    public string ToString(string format, IFormatProvider formatProvider) {
      return $"{this.ToDouble4().ToString(format, formatProvider)}";
    }

    internal sealed class DebuggerProxy {
      public float x;
      public float y;
      public float z;
      public float w;
      public DebuggerProxy(state s) {
        x = (float) s.velocity.x;
        y = (float) s.velocity.y;
        z = (float) s.position.r;
        w = (float) s.position.theta.radians;
      }
    }

  }
}
