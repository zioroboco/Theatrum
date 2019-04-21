using System;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEngine;

namespace Orbital {

  [DebuggerTypeProxy(typeof(angle.DebuggerProxy))]
  [System.Serializable]
  public struct angle : System.IEquatable<angle>, IFormattable {

    public readonly double radians;
    public angle(double radians) {
      this.radians = wrap(radians);
    }

    public angle(double2 vector) {
      this.radians = wrap(signed(xHat, vector));
    }

    public angle(double2 a, double2 b) {
      this.radians = wrap(signed(a, b));
    }

    private static readonly double2 xHat = new double2(1d, 0d);
    private static readonly double3 zHat = new double3(0d, 0d, 1d);

    public static double signed(double2 a, double2 b) {
      var unsigned = math.acos(math.dot(math.normalizesafe(a), math.normalizesafe(b)));
      var cross = math.cross(new double3(a.x, a.y, 0d), new double3(b.x, b.y, 0d));
      var sign = math.dot(zHat, cross) < 0;
      return (sign ? -1d : 1d)  * unsigned;
    }

    private static double wrapMax(double theta, double max) {
      return math.fmod(max + math.fmod(theta, max), max);
    }

    private static double wrapMinMax(double radians, double min, double max) {
      return min + wrapMax( radians - min, max - min);
    }

    public static double wrap(double radians) {
      return wrapMinMax(radians, -math.PI, math.PI);
    }

    public bool Equals(angle rhs) {
      return this.radians == rhs.radians;
    }

    public override bool Equals(object o) {
      return o != null && Equals((angle) o);
    }

    public override int GetHashCode() {
      return (int) this.radians;
    }

    public static bool operator ==(angle lhs, angle rhs) {
      return lhs.radians.Equals(rhs.radians);
    }

    public static bool operator !=(angle lhs, angle rhs) {
      return !lhs.radians.Equals(rhs.radians);
    }

    public override string ToString() {
      return $"{radians} radians";
    }

    public string ToString(string format, IFormatProvider formatProvider) {
      return $"{this.radians.ToString(format, formatProvider)} radians";
    }

    internal sealed class DebuggerProxy {
      public float radians;
      public DebuggerProxy(angle a) {
        radians = (float) a.radians;
      }
    }

    public double2 Radial() {
      return Radial(this);
    }

    public static double2 Radial(angle theta) {
      return Radial(theta.radians);
    }

    public static double2 Radial(double radians) {
      return new double2(math.cos(radians), math.sin(radians));
    }

    public double2 Perpendicular() {
      return Perpendicular(this.Radial());
    }

    private static readonly double3 z3 = new double3(0d, 0d, 1d);

    public static double2 Perpendicular(double2 radial) {
      var radial3 = new double3(radial.x, radial.y, 0d);
      var perpendicular3 = math.cross(z3, radial3);
      return new double2(perpendicular3.x, perpendicular3.y);
    }

  }
}
