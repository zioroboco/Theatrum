using System;
using System.Diagnostics;
using Unity.Mathematics;

namespace Orbital {

  [DebuggerTypeProxy(typeof(elements.DebuggerProxy))]
  [System.Serializable]
  public struct elements : System.IEquatable<elements> {

    public enum Direction {
      clockwise = -1,
      anticlockwise = 1
    }

    public readonly double a;
    public readonly double e;
    public readonly angle omega;
    public readonly angle theta;
    public readonly Direction direction;

    public elements(double a, double e, angle omega, angle theta, Direction direction) {
      this.a = a;
      this.e = e;
      this.omega = omega;
      this.theta = theta;
      this.direction = direction;
    }

    public bool Equals(elements rhs) {
      return this.a == rhs.a &&
        this.e == rhs.e &&
        this.omega == rhs.omega &&
        this.theta == rhs.theta &&
        this.direction == rhs.direction;
    }

    public override bool Equals(object o) {
      return o != null && Equals((elements) o);
    }

    public static double sign(Direction direction) {
      return direction == Direction.anticlockwise ? 1d : -1d;
    }

    public override int GetHashCode() {
      return (int) math.hash(
        sign(this.direction) * math.double4(a, e, omega.radians, theta.radians)
      );
    }

    public static bool operator ==(elements lhs, elements rhs) {
      return lhs.Equals(rhs);
    }

    public static bool operator !=(elements lhs, elements rhs) {
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
      public DebuggerProxy(elements el) {
        this.a = (float) el.a;
        this.e = (float) el.e;
        this.omega = (float) el.omega.radians;
        this.theta = (float) el.theta.radians;
        this.direction = el.direction;
      }
    }
  }
}
