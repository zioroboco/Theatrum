using NUnit.Framework;
using Orbital;
using Unity.Mathematics;

namespace Coordinates {

    public class Type {

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
            var a_1 = new coordinates(2d, 3d);
            var a_2 = new coordinates(2d, 3d);
            var b = new coordinates(3d, 2d);
            Assert.True(a_1.GetHashCode() == a_2.GetHashCode());
            Assert.True(a_1.GetHashCode() != b.GetHashCode());
        }

        [Test]
        public void Formatting() {
            var a_r = 2d;
            var a_theta = new angle(3d);
            var a = new coordinates(a_r, a_theta.radians);
            Assert.AreEqual(
                $"coordinates(r: {a_r}, theta: {a_theta})",
                a.ToString()
            );
        }

    }

    public class Constructor {

        private const double tolerance = 1E-15d;

        [Test]
        public void From_WorldVector_North() {
            var expected = new coordinates(2d, math.PI / 2d);
            var actual = new coordinates(new double2(0d, 2d));
            Assert.AreEqual(expected.r, actual.r, tolerance);
            Assert.AreEqual(expected.theta.radians, actual.theta.radians, tolerance);
        }

        [Test]
        public void From_WorldVector_South() {
            var expected = new coordinates(2d, 3d * math.PI / 2d);
            var actual = new coordinates(new double2(0d, -2d));
            Assert.AreEqual(expected.r, actual.r, tolerance);
            Assert.AreEqual(expected.theta.radians, actual.theta.radians, tolerance);
        }

        [Test]
        public void From_WorldVector_SouthNegative() {
            var expected = new coordinates(2d, -math.PI / 2d);
            var actual = new coordinates(new double2(0d, -2d));
            Assert.AreEqual(expected.r, actual.r, tolerance);
            Assert.AreEqual(expected.theta.radians, actual.theta.radians, tolerance);
        }

        [Test]
        public void From_WorldVector_SouthNegative_WithAngle() {
            var theta = new angle(-math.PI / 2d);
            var expected = new coordinates(2d, theta);
            var actual = new coordinates(new double2(0d, -2d));
            Assert.AreEqual(expected.r, actual.r, tolerance);
            Assert.AreEqual(expected.theta.radians, actual.theta.radians, tolerance);
        }

    }

    public class Utils {

        private const double tolerance = 1E-15d;

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
                angle.wrap(expected.theta.radians),
                angle.wrap(actual.theta.radians),
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
                angle.wrap(expected.theta.radians),
                angle.wrap(actual.theta.radians),
                tolerance
            );
        }

    }
}
