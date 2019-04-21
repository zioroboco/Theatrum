using System;
using System.Diagnostics;
using Orbital;
using Unity.Entities;
using Unity.Mathematics;

namespace Keplerian {

  [DebuggerTypeProxy(typeof(Elements.DebuggerProxy))]
  [Serializable]
  public struct Elements : IComponentData, IEquatable<Elements> {

    public enum Direction {
      clockwise = -1,
      anticlockwise = 1
    }

    public readonly double a;
    public readonly double e;
    public readonly angle omega;
    public readonly angle theta;
    public readonly Direction direction;

    public Elements(double a, double e, angle omega, angle theta, Direction direction) {
      this.a = a;
      this.e = e;
      this.omega = omega;
      this.theta = theta;
      this.direction = direction;
    }

    public Elements(Newtonian.Vectors state) {
      throw new NotImplementedException();
    }

    public bool Equals(Elements rhs) {
      return this.a == rhs.a &&
        this.e == rhs.e &&
        this.omega == rhs.omega &&
        this.theta == rhs.theta &&
        this.direction == rhs.direction;
    }

    public override bool Equals(object o) {
      return o != null && Equals((Elements) o);
    }

    public static double sign(Direction direction) {
      return direction == Direction.anticlockwise ? 1d : -1d;
    }

    public override int GetHashCode() {
      return (int) math.hash(
        sign(this.direction) * math.double4(a, e, omega.radians, theta.radians)
      );
    }

    public static bool operator ==(Elements lhs, Elements rhs) {
      return lhs.Equals(rhs);
    }

    public static bool operator !=(Elements lhs, Elements rhs) {
      return !lhs.Equals(rhs);
    }

    public override string ToString() {
      return string.Format(
        "elements(a: {0}, e: {1}, omega: {2}, theta: {3}, direction: {4})",
        this.a,
        this.e,
        this.omega,
        this.theta,
        this.direction
      );
    }

    internal sealed class DebuggerProxy {
      public float a;
      public float e;
      public float omega;
      public float theta;
      public Direction direction;
      public DebuggerProxy(Elements el) {
        this.a = (float) el.a;
        this.e = (float) el.e;
        this.omega = (float) el.omega.radians;
        this.theta = (float) el.theta.radians;
        this.direction = el.direction;
      }
    }
  }
}
