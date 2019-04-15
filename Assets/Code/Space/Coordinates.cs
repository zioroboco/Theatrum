using System;
using System.Diagnostics;
using Unity.Mathematics;

namespace Space {

  /// <summary>A set of polar coordinates.</summary>
  [DebuggerTypeProxy(typeof(coordinates.DebuggerProxy))]
  [System.Serializable]
  public struct coordinates : System.IEquatable<coordinates>, IFormattable {

    /// <summary>Scalar distance from the origin.</summary>
    public double r;

    /// <summary>Angle in radians from the direction of positive x.</summary>
    public double theta;

    public static readonly double2 xHat = new double2(1d, 0d);

    private static readonly double3 zHat = new double3(0d, 0d, 1d);

    public static readonly coordinates zero = new coordinates{ r = 0d, theta = 0d };

    /// <summary>
    /// Construct a new set of coordinates from a world-space vector.
    /// </summary>
    public coordinates(double2 v) {
      this.r = math.length(v);
      this.theta = angle(xHat, math.normalizesafe(v));
    }

    public static double angle(double2 a, double2 b) {
      var unsigned = math.acos(math.dot(math.normalizesafe(a), math.normalizesafe(b)));
      var cross = math.cross(new double3(a.x, a.y, 0d), new double3(b.x, b.y, 0d));
      var sign = math.dot(zHat, cross) < 0;
      return (sign ? -1d : 1d)  * unsigned;
    }

    private static double wrapMax(double theta, double max) {
      return math.fmod(max + math.fmod(theta, max), max);
    }

    private static double wrapMinMax(double theta, double min, double max) {
      return min + wrapMax(theta - min, max - min);
    }

    public static double normalizeAngle(double theta) {
      return wrapMinMax(theta, -math.PI, math.PI);
    }

    /// <summary>
    /// Construct a new set of coordinates by applying a polar-space vector to
    /// another set of coordinates.
    /// </summary>
    public coordinates(double2 polar, coordinates prev) {
      var next = new coordinates(prev.ToWorldVector(polar));
      this.r = next.r;
      this.theta = normalizeAngle(next.theta);
    }

    /// <summary>Construct a new set of coordinates.</summary>
    public coordinates(double r, double theta) {
      this.r = r;
      this.theta = normalizeAngle(theta);
    }

    /// <summary>Test equality against another set of coordinates.</summary>
    public bool Equals(coordinates rhs) {
      return this.r == rhs.r && this.theta == rhs.theta;
    }

    /// <summary>Test equality against another set of coordinates.</summary>
    public override bool Equals(object o) {
      return o != null && Equals((coordinates) o);
    }

    /// <summary>A hash summary of the coordinates.</summary>
    public override int GetHashCode() {
      return (int) math.hash(math.double2(this.r, this.theta));
    }

    /// <summary>Test equality against another set of coordinates.</summary>
    public static bool operator ==(coordinates lhs, coordinates rhs) {
      return lhs.r.Equals(rhs.r) && lhs.theta.Equals(rhs.theta);
    }

    /// <summary>Test inequality against another set of coordinates.</summary>
    public static bool operator !=(coordinates lhs, coordinates rhs) {
      return !lhs.r.Equals(rhs.r) || !lhs.theta.Equals(rhs.theta);
    }

    /// <summary>A string representation of the coordinates.</summary>
    public override string ToString() {
      return string.Format("coordinates(r: {0}, theta: {1})", r, theta);
    }

    /// <summary>A localised string representation of the coordinates.</summary>
    public string ToString(string format, IFormatProvider formatProvider) {
      return string.Format(
        "coordinates(r: {0}, theta: {1})",
        this.r.ToString(format, formatProvider),
        this.theta.ToString(format, formatProvider)
      );
    }

    internal sealed class DebuggerProxy {
      public float r;
      public float theta;
      public DebuggerProxy(coordinates c) {
        r = (float) c.r;
        theta = (float) c.theta;
      }
    }

    /// <summary>Unit vector in the direction of increasing `r`.</summary>
    public double2 RHat() {
      return new double2(math.cos(this.theta), math.sin(this.theta));
    }

    /// <summary>Unit vector in the direction of increasing `theta`.</summary>
    public double2 ThetaHat() {
      var rHat2 = this.RHat();
      var rHat3 = new double3(rHat2.x, rHat2.y, 0d);
      var zHat3 = new double3(0d, 0d, 1d);
      var thetaHat3 = math.cross(zHat3, rHat3);
      return new double2(thetaHat3.x, thetaHat3.y);
    }

    /// <summary>
    /// The position in polar coordinates as a vector in world-space, relative
    /// to the origin of the polar coordinate system.
    /// </summary>
    public double2 ToWorldVector() {
      return this.r * this.RHat();
    }

    /// <summary>
    /// The passed polar vector as a vector in world-space, relative to the
    /// origin of the polar coordinate system.
    /// </summary>
    public double2 ToWorldVector(double2 polar) {
      return this.WorldTransform(polar) + this.ToWorldVector();
    }

    /// <summary>
    /// The transform representing a change of basis from polar- to world-space.
    /// </summary>
    public double2x2 WorldTransform() {
      return new double2x2(this.ThetaHat(), this.RHat());
    }

    /// <summary>
    /// The passed polar vector as a vector in world-space, relative to the
    /// position in polar coordinates.
    /// </summary>
    public double2 WorldTransform(double2 polar) {
      return math.mul(this.WorldTransform(), polar);
    }

    /// <summary>
    /// The transform representing a change of basis from world- to polar-space.
    /// </summary>
    public double2x2 PolarTransform() {
      return math.inverse(this.WorldTransform());
    }

    /// <summary>
    /// The passed world vector as a vector in polar-space, relative to the
    /// position in polar coordinates.
    /// </summary>
    public double2 PolarTransform(double2 world) {
      return math.mul(this.PolarTransform(), world);
    }

    public coordinates Update(double2 polar) {
      return new coordinates(this.ToWorldVector(polar));
    }

  }
}
