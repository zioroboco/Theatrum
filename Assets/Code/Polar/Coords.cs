using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Mathematics;

namespace Polar {

  /// <summary>A set of polar coordinates.</summary>
  [DebuggerTypeProxy(typeof(coords.DebuggerProxy))]
  [System.Serializable]
  public struct coords : System.IEquatable<coords>, IFormattable {

    /// <summary>Scalar distance from the origin.</summary>
    public double r;

    /// <summary>Angle in radians from the direction of positive x.</summary>
    public double theta;

    /// <summary>The origin as polar coordinates (with theta = 0).</summary>
    public static readonly coords zero;

    public static readonly double2 xHat = new double2(1d, 0d);

    /// <summary>
    /// Construct a new set of coordinates from a world-space vector.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public coords(double2 v) {
      this.r = math.length(v);
      this.theta = math.acos(math.dot(math.normalizesafe(v), xHat));
    }

    /// <summary>
    /// Construct a new set of coordinates by applying a polar-space vector to
    /// another set of coordinates.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public coords(double2 polar, coords prev) {
      // TODO optimise me!
      var next = new coords(prev.ToWorldVector(polar));
      this.r = next.r;
      this.theta = next.theta;
    }

    /// <summary>Construct a new set of coordinates.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public coords(double r, double theta) {
      this.r = r;
      this.theta = theta;
    }

    /// <summary>Construct a new set of coordinates.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public coords(float r, double theta) {
      this.r = (double) r;
      this.theta = theta;
    }

    /// <summary>Construct a new set of coordinates.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public coords(double r, float theta) {
      this.r = r;
      this.theta = (double) theta;
    }

    /// <summary>Construct a new set of coordinates.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public coords(float r, float theta) {
      this.r = (double) r;
      this.theta = (double) theta;
    }

    /// <summary>Test equality against another set of coordinates.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(coords rhs) {
      return this.r == rhs.r && this.theta == rhs.theta;
    }

    /// <summary>Test equality against another set of coordinates.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object o) {
      return Equals((coords) o);
    }

    /// <summary>A hash summary of the coordinates.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() {
      return (int) math.hash(math.double2(this.r, this.theta));
    }

    /// <summary>Test equality against another set of coordinates.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(coords lhs, coords rhs) {
      return lhs.r.Equals(rhs.r) && lhs.theta.Equals(rhs.theta);
    }

    /// <summary>Test inequality against another set of coordinates.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(coords lhs, coords rhs) {
      return !lhs.r.Equals(rhs.r) || !lhs.theta.Equals(rhs.theta);
    }

    /// <summary>A string representation of the coordinates.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() {
      return string.Format("coords(r: {0}, theta: {1})", r, theta);
    }

    /// <summary>A localised string representation of the coordinates.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public string ToString(string format, IFormatProvider formatProvider) {
      return string.Format(
        "coords(r: {0}, theta: {1})",
        this.r.ToString(format, formatProvider),
        this.theta.ToString(format, formatProvider)
      );
    }

    internal sealed class DebuggerProxy {
      public float r;
      public float theta;
      public DebuggerProxy(coords c) {
        r = (float) c.r;
        theta = (float) c.theta;
      }
    }

    /// <summary>Unit vector in the direction of increasing `r`.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double2 RHat() {
      return new double2(math.cos(this.theta), math.sin(this.theta));
    }

    /// <summary>Unit vector in the direction of increasing `theta`.</summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double2 ToWorldVector() {
      return this.r * this.RHat();
    }

    /// <summary>
    /// The passed polar vector as a vector in world-space, relative to the
    /// origin of the polar coordinate system.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double2 ToWorldVector(double2 polar) {
      return this.WorldTransform(polar) + this.ToWorldVector();
    }

    /// <summary>
    /// The transform representing a change of basis from polar- to world-space.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double2x2 WorldTransform() {
      return new double2x2(this.ThetaHat(), this.RHat());
    }

    /// <summary>
    /// The passed polar vector as a vector in world-space, relative to the
    /// position in polar coordinates.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double2 WorldTransform(double2 polar) {
      return math.mul(this.WorldTransform(), polar);
    }

    /// <summary>
    /// The transform representing a change of basis from world- to polar-space.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double2x2 PolarTransform() {
      return math.inverse(this.WorldTransform());
    }

    /// <summary>
    /// The passed world vector as a vector in polar-space, relative to the
    /// position in polar coordinates.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double2 PolarTransform(double2 world) {
      return math.mul(this.PolarTransform(), world);
    }

  }
}
