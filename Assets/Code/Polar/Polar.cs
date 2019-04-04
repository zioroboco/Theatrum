using Unity.Mathematics;

public class Polar {

  static double2 xHat2 = new double2(1d, 0d);
  static double2 yHat2 = new double2(0d, 1d);

  static double3 xHat3 = new double3(1d, 0d, 0d);
  static double3 yHat3 = new double3(0d, 1d, 0d);
  static double3 zHat3 = new double3(0d, 0d, 1d);

  public struct Position {
    public double r;
    public double theta;
  }

  /// <summary>
  /// Convert a world-space double2 to polar co-ordinates.
  /// </summary>
  public static Position toPolarPosition(double2 v) {
    var vHat = math.normalizesafe(v);
    var r = math.length(v);
    var theta = math.acos(math.dot(vHat, xHat2));
    return new Position { r = r, theta = theta };
  }

  /// <summary>
  /// Create a relative vector (from p) from the passed world-space double2.
  /// </summary>
  public static double2 toPolarVector(double2 v, Position p) {
    var vHat = math.normalizesafe(v);
    var vMag = math.length(v);
    var _rHat = rHat(p.theta);
    return vMag * new double2(
      math.dot(vHat, thetaHat(_rHat)),
      math.dot(vHat, _rHat)
    );
  }

  /// <summary>
  /// Convert polar co-ordinates to a world-space double2.
  /// </summary>
  public static double2 toVector(Position p) {
    return p.r * new double2(math.cos(p.theta), math.sin(p.theta));
  }

  /// <summary>
  /// Find world-space radial unit double2 for a given angle theta.
  /// </summary>
  public static double2 rHat(double theta) {
    return toVector(new Position { r = 1d, theta = theta });
  }

  /// <summary>
  /// Find world-space tangential unit double2 for a given radial unit double2.
  /// </summary>
  public static double2 thetaHat(double2 rHat) {
    var _thetaHat3 = math.normalizesafe(
      math.cross(-zHat3, new double3(rHat.x, rHat.y, 0d))
    );
    return new double2(_thetaHat3.x, _thetaHat3.y);
  }

  /// <summary>
  /// Find world-space tangential unit double2 for a given angle theta.
  /// </summary>
  public static double2 thetaHat(double theta) {
    return thetaHat(rHat(theta));
  }

}
