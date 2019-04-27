using NUnit.Framework;
using Orbital;
using Unity.Mathematics;

namespace Angle {

    public class Type {

        [Test]
        public void Equality() {
            var a = new angle(0d);
            var b = new angle(0d);
            Assert.True(a.Equals(b));
        }

        [Test]
        public void Inequality() {
            var a = new angle(0d);
            var b = new angle(1d);
            Assert.False(a.Equals(b));
        }

        [Test]
        public void InfixEquality() {
            var a = new angle(0d);
            var b = new angle(0d);
            Assert.True(a == b);
        }

        [Test]
        public void InfixInequality() {
            var a = new angle(0d);
            var b = new angle(1d);
            Assert.True(a != b);
        }

        [Test]
        public void ObjectEquality() {
            var a = new angle(0d);
            var b = new angle(1d);
            Assert.True(a.Equals(a));
            Assert.False(a.Equals(b));
        }

        [Test]
        public void Hashing() {
            var a_1 = new angle(0d);
            var a_2 = new angle(0d);
            var b = new angle(1d);
            Assert.True(a_1.GetHashCode() == a_2.GetHashCode());
            Assert.True(a_1.GetHashCode() != b.GetHashCode());
        }

        [Test]
        public void Formatting() {
            const double rad = 1d;
            var a = new angle(rad);
            Assert.AreEqual(
                $"{rad} radians",
                a.ToString()
            );
        }

    }

    public class Constructor {

        private const double tolerance = 1E-15d;
        private static readonly double2 a = new double2(2d, 0d);
        private static readonly double2 b = new double2(0d, 2d);

        [Test]
        public void FromVectors_Zero() {
            var actual = new angle(a, a);
            var expected = new angle(0d);
            Assert.AreEqual(expected.radians, actual.radians, tolerance);
        }

        [Test]
        public void FromVectors_PiOnTwo() {
            var actual = new angle(a, b);
            var expected = new angle(math.PI_DBL / 2d);
            Assert.AreEqual(expected.radians, actual.radians, tolerance);
        }

        [Test]
        public void FromVectors_PiOnTwo_Signed() {
            var actual = new angle(b, a);
            var expected = new angle(-math.PI_DBL / 2d);
            Assert.AreEqual(expected.radians, actual.radians, tolerance);
        }

        [Test]
        public void FromVectors_Pi() {
            var actual = new angle(a, -a);
            var expected = new angle(math.PI_DBL);
            Assert.AreEqual(expected.radians, actual.radians, tolerance);
        }

        [Test]
        public void FromVectors_Negative_PiOnTwo() {
            var actual = new angle(a, -b);
            var expected = new angle(-math.PI_DBL / 2d);
            Assert.AreEqual(expected.radians, actual.radians, tolerance);
        }


        [Test]
        public void FromDouble_Negative_SevenPiOnTwo() {
            var actual = new angle(-7d * math.PI_DBL / 2d);
            var expected = new angle(math.PI_DBL / 2d);
            Assert.AreEqual(expected.radians, actual.radians, tolerance);
        }

        [Test]
        public void FromSingleVector() {
            var actual = new angle(b);
            var expected = new angle(math.PI_DBL / 2d);
            Assert.AreEqual(expected.radians, actual.radians, tolerance);
        }

        [Test]
        public void FromSingleVector_Signed() {
            var actual = new angle(-b);
            var expected = new angle(-math.PI_DBL / 2d);
            Assert.AreEqual(expected.radians, actual.radians, tolerance);
        }

    }

    public class Wrap {

        private const double tolerance = 1E-15d;

        [Test]
        public void Wrap_Zero() {
            var actual = angle.wrap(0d);
            const double expected = 0d;
            Assert.AreEqual(expected, actual, tolerance);
        }

        [Test]
        public void Wrap_PiOnTwo() {
            var actual = angle.wrap(math.PI_DBL / 2d);
            var expected = math.PI_DBL / 2d;
            Assert.AreEqual(expected, actual, tolerance);
        }

        [Test]
        public void Wrap_Pi() {
            var actual = angle.wrap(math.PI_DBL);
            var expected = -math.PI_DBL;
            Assert.AreEqual(expected, actual, tolerance);
        }

        [Test]
        public void Wrap_ThreePiOnTwo() {
            var actual = angle.wrap(3d * math.PI_DBL / 2d);
            var expected = -math.PI_DBL / 2d;
            Assert.AreEqual(expected, actual, tolerance);
        }

        [Test]
        public void Wrap_Negative_ThreePiOnTwo() {
            var actual = angle.wrap(-3d * math.PI_DBL / 2d);
            var expected = math.PI_DBL / 2d;
            Assert.AreEqual(expected, actual, tolerance);
        }

        [Test]
        public void Wrap_FivePiOnTwo() {
            var actual = angle.wrap(5d * math.PI_DBL / 2d);
            var expected = math.PI_DBL / 2d;
            Assert.AreEqual(expected, actual, tolerance);
        }

        [Test]
        public void Wrap_Negative_FivePiOnTwo() {
            var actual = angle.wrap(-5d * math.PI_DBL / 2d);
            var expected = -math.PI_DBL / 2d;
            Assert.AreEqual(expected, actual, tolerance);
        }

        [Test]
        public void Wrap_Negative_SevenPiOnTwo() {
            var actual = angle.wrap(-7d * math.PI_DBL / 2d);
            var expected = math.PI_DBL / 2d;
            Assert.AreEqual(expected, actual, tolerance);
        }
    }

    public class UnitVector {

        private const double tolerance = 1E-15d;
        private static readonly double2 xHat = new double2(1d, 0d);
        private static readonly double2 yHat = new double2(0d, 1d);

        [Test]
        public void Radial_North() {
            var actual = new angle(math.PI_DBL / 2d).Radial();
            var expected = yHat;
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void Radial_South() {
            var actual = new angle(-math.PI_DBL / 2d).Radial();
            var expected = -yHat;
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void Radial_East() {
            var actual = new angle(0d).Radial();
            var expected = xHat;
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void Radial_West() {
            var actual = new angle(math.PI_DBL).Radial();
            var expected = -xHat;
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void Perpendicular_North() {
            var actual = new angle(math.PI_DBL / 2d).Perpendicular();
            var expected = -xHat;
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void Perpendicular_South() {
            var actual = new angle(-math.PI_DBL / 2d).Perpendicular();
            var expected = xHat;
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void Perpendicular_East() {
            var actual = new angle(0d).Perpendicular();
            var expected = yHat;
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void Perpendicular_West() {
            var actual = new angle(math.PI_DBL).Perpendicular();
            var expected = -yHat;
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }
    }
}
