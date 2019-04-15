using System;
using JetBrains.Annotations;
using NUnit.Framework;
using Space;
using Unity.Mathematics;

[UsedImplicitly]
internal class Coordinates {

    private const double tolerance = 1E-15d;

    public class Type {

        [Test]
        public void Zero() {
            Assert.AreEqual(new coordinates(0d, 0d), coordinates.zero);
        }

        [Test]
        public void Equality() {
            var a = new coordinates(2d, 3d);
            var b = new coordinates(2d, 3d);
            Assert.AreEqual(a, b);
        }

        [Test]
        public void Inequality() {
            var a = new coordinates(2d, 3d);
            var b = new coordinates(3d, 2d);
            Assert.AreNotEqual(a, b);
        }

        [Test]
        public void InfixEquality() {
            var a = new coordinates(2d, 3d);
            var b = new coordinates(2d, 3d);
            Assert.True(a == b);
        }

        [Test]
        public void InfixInequality() {
            var a = new coordinates(2d, 3d);
            var b = new coordinates(3d, 2d);
            Assert.True(a != b);
        }

        [Test]
        public void ObjectEquality() {
            var a = new coordinates(2d, 3d);
            var b = new coordinates(3d, 2d);
            Assert.True(a.Equals(a));
            Assert.False(a.Equals(b));
        }

        [Test]
        public void Hashing() {
            var a = new coordinates(2d, 3d);
            var b = new coordinates(3d, 2d);
            Assert.True(a.GetHashCode() == a.GetHashCode());
            Assert.True(a.GetHashCode() != b.GetHashCode());
        }

        [Test]
        public void Formatting() {
            var a_r = 2d;
            var a_theta = 3d;
            var a = new coordinates(a_r, a_theta);
            Assert.AreEqual(
                String.Format("coordinates(r: {0}, theta: {1})", a_r, a_theta),
                a.ToString()
            );
        }

    }

    public class Constructor {

        [Test]
        public void From_WorldVector_North() {
            var expected = new coordinates(2d, math.PI / 2d);
            var actual = new coordinates(new double2(0d, 2d));
            Assert.AreEqual(expected.r, actual.r, tolerance);
            Assert.AreEqual(expected.theta, actual.theta, tolerance);
        }

        [Test]
        public void From_WorldVector_South() {
            var expected = new coordinates(2d, 3d * math.PI / 2d);
            var actual = new coordinates(new double2(0d, -2d));
            Assert.AreEqual(expected.r, actual.r, tolerance);
            Assert.AreEqual(expected.theta, actual.theta, tolerance);
        }

        [Test]
        public void From_WorldVector_SouthNegative() {
            var expected = new coordinates(2d, -math.PI / 2d);
            var actual = new coordinates(new double2(0d, -2d));
            Assert.AreEqual(expected.r, actual.r, tolerance);
            Assert.AreEqual(expected.theta, actual.theta, tolerance);
        }

        [Test]
        public void From_PolarVector() {
            var v = new double2(1d, -1d);
            var prev = new coordinates(1d, math.PI / 2d);
            var expected = new coordinates(1d, math.PI);
            var actual = new coordinates(v, prev);
            Assert.AreEqual(expected.r, actual.r, tolerance);
            Assert.AreEqual(expected.theta, actual.theta, tolerance);
        }

    }

    public class StaticUtils {

        static double2 x = new double2(2d, 0d);
        static double2 y = new double2(0d, 2d);

        [Test]
        public void Angle_Zero() {
            double actual = coordinates.angle(x, x);
            double expected = coordinates.normalizeAngle(0d);
            Assert.AreEqual(expected, actual, tolerance);
        }

        [Test]
        public void Angle_PiOnTwo() {
            double actual = coordinates.angle(x, y);
            double expected = coordinates.normalizeAngle(math.PI / 2d);
            Assert.AreEqual(expected, actual, tolerance);
        }

        [Test]
        public void Angle_Pi() {
            double actual = coordinates.angle(x, -x);
            double expected = math.PI;
            Assert.AreEqual(expected, actual, tolerance);
        }

        [Test]
        public void Angle_NegPiOnTwo() {
            double actual = coordinates.angle(x, -y);
            double expected = -math.PI / 2d;
            Assert.AreEqual(expected, actual, tolerance);
        }

        [Test]
        public void NormalizeAngle_Zero() {
            double actual = coordinates.normalizeAngle(0d);
            double expected = 0d;
            Assert.AreEqual(expected, actual, tolerance);
        }

        [Test]
        public void NormalizeAngle_PiOnTwo() {
            double actual = coordinates.normalizeAngle(math.PI / 2d);
            double expected = math.PI / 2d;
            Assert.AreEqual(expected, actual, tolerance);
        }

        [Test]
        public void NormalizeAngle_Pi() {
            double actual = coordinates.normalizeAngle(math.PI);
            double expected = -math.PI;
            Assert.AreEqual(expected, actual, tolerance);
        }

        [Test]
        public void NormalizeAngle_ThreePiOnTwo() {
            double actual = coordinates.normalizeAngle(3d * math.PI / 2d);
            double expected = -math.PI / 2d;
            Assert.AreEqual(expected, actual, tolerance);
        }

        [Test]
        public void NormalizeAngle_Negative_ThreePiOnTwo() {
            double actual = coordinates.normalizeAngle(-3d * math.PI / 2d);
            double expected = math.PI / 2d;
            Assert.AreEqual(expected, actual, tolerance);
        }

        [Test]
        public void NormalizeAngle_FivePiOnTwo() {
            double actual = coordinates.normalizeAngle(5d * math.PI / 2d);
            double expected = math.PI / 2d;
            Assert.AreEqual(expected, actual, tolerance);
        }

        [Test]
        public void NormalizeAngle_Negative_FivePiOnTwo() {
            double actual = coordinates.normalizeAngle(-5d * math.PI / 2d);
            double expected = -math.PI / 2d;
            Assert.AreEqual(expected, actual, tolerance);
        }

        [Test]
        public void NormalizeAngle_Negative_SevenPiOnTwo() {
            double actual = coordinates.normalizeAngle(7d * math.PI / 2d);
            double expected = -math.PI / 2d;
            Assert.AreEqual(expected, actual, tolerance);
        }

    }

    public class Utils {

        [Test]
        public void RHat() {
            var c = new coordinates(2d, math.PI / 2d);
            var expected = new double2(0d, 1d);
            var actual = c.RHat();
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void ThetaHat() {
            var c = new coordinates(2d, math.PI / 2d);
            // Increasing theta is in the negative-x direction.
            var expected = new double2(-1d, 0d);
            var actual = c.ThetaHat();
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void WorldTransform() {
            var c = new coordinates(1d, math.PI / 2d);
            var v_polar = new double2(2d, 3d);
            var T_world = c.WorldTransform();
            var expected = new double2(-2d, 3d);
            var actual = math.mul(T_world, v_polar);
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void WorldTransform_Vector() {
            var c = new coordinates(1d, math.PI / 2d);
            var v_polar = new double2(2d, 3d);
            var expected = new double2(-2d, 3d);
            var actual = c.WorldTransform(v_polar);
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void PolarTransform() {
            var c = new coordinates(1d, math.PI / 2d);
            var v_world = new double2(-2d, 3d);
            var T_polar = c.PolarTransform();
            var expected = new double2(2d, 3d);
            var actual = math.mul(T_polar, v_world);
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void PolarTransform_Vector() {
            var c = new coordinates(1d, math.PI / 2d);
            var v_world = new double2(-2d, 3d);
            var expected = new double2(2d, 3d);
            var actual = c.PolarTransform(v_world);
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void ToWorldVector() {
            var c = new coordinates(2d, math.PI / 2d);
            var expected = new double2(0d, 2d);
            var actual = c.ToWorldVector();
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void ToWorldVector_PolarVector() {
            var c = new coordinates(1d, math.PI / 2d);
            var v_polar = new double2(2d, 3d);
            var expected = new double2(-2d, 4d);
            var actual = c.ToWorldVector(v_polar);
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void Update() {
            var pos = new coordinates(2d, -math.PI / 2d);
            var v_polar = new double2(-2d, -2d);
            var expected = new coordinates(2d, math.PI);
            var actual = pos.Update(v_polar);
            Assert.AreEqual(expected.r, actual.r, tolerance);
            Assert.AreEqual(
                coordinates.normalizeAngle(expected.theta),
                coordinates.normalizeAngle(actual.theta),
                tolerance
            );
        }

        [Test]
        public void RelativeToWorldVector() {
            var pos = new coordinates(2d, -math.PI / 2d);
            var v_polar = new double2(-2d, -2d);
            var expected = new coordinates(2d, math.PI);
            var actual = pos.Update(v_polar);
            Assert.AreEqual(expected.r, actual.r, tolerance);
            Assert.AreEqual(
                coordinates.normalizeAngle(expected.theta),
                coordinates.normalizeAngle(actual.theta),
                tolerance
            );
        }

    }
}
