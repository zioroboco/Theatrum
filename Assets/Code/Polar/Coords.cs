using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Mathematics;

namespace Polar {

  [DebuggerTypeProxy(typeof(coords.DebuggerProxy))]
  [System.Serializable]
  public struct coords : System.IEquatable<coords>, IFormattable {

    public double r;
    public double theta;

    public static readonly coords zero;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public coords(double r, double theta) {
      this.r = r;
      this.theta = theta;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public coords(float r, double theta) {
      this.r = (double) r;
      this.theta = theta;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public coords(double r, float theta) {
      this.r = r;
      this.theta = (double) theta;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public coords(float r, float theta) {
      this.r = (double) r;
      this.theta = (double) theta;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool Equals(coords rhs) {
      return this.r == rhs.r && this.theta == rhs.theta;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override bool Equals(object o) {
      return Equals((coords) o);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override int GetHashCode() {
      return (int) math.hash(math.double2(this.r, this.theta));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(coords lhs, coords rhs) {
      return lhs.r.Equals(rhs.r) && lhs.theta.Equals(rhs.theta);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(coords lhs, coords rhs) {
      return !lhs.r.Equals(rhs.r) || !lhs.theta.Equals(rhs.theta);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override string ToString() {
      return string.Format("coords(r: {0}, theta: {1})", r, theta);
    }

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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double2 RHat() {
      return new double2(math.cos(this.theta), math.sin(this.theta));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double2 ThetaHat() {
      var rHat2 = this.RHat();
      var rHat3 = new double3(rHat2.x, rHat2.y, 0d);
      var zHat3 = new double3(0d, 0d, 1d);
      var thetaHat3 = math.cross(zHat3, rHat3);
      return new double2(thetaHat3.x, thetaHat3.y);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double2 ToWorldVector() {
      return this.r * this.RHat();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double2 ToWorldVector(double2 polar) {
      return this.WorldTransform(polar) + this.ToWorldVector();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double2x2 WorldTransform() {
      return new double2x2(this.ThetaHat(), this.RHat());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double2 WorldTransform(double2 polar) {
      return math.mul(this.WorldTransform(), polar);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double2x2 PolarTransform() {
      return math.inverse(this.WorldTransform());
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public double2 PolarTransform(double2 world) {
      return math.mul(this.PolarTransform(), world);
    }

  }
}
