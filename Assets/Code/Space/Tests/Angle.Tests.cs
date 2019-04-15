using NUnit.Framework;
using Space;
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
            var expected = new angle(math.PI / 2d);
            Assert.AreEqual(expected.radians, actual.radians, tolerance);
        }

        [Test]
        public void FromVectors_PiOnTwo_Signed() {
            var actual = new angle(b, a);
            var expected = new angle(-math.PI / 2d);
            Assert.AreEqual(expected.radians, actual.radians, tolerance);
        }

        [Test]
        public void FromVectors_Pi() {
            var actual = new angle(a, -a);
            var expected = new angle(math.PI);
            Assert.AreEqual(expected.radians, actual.radians, tolerance);
        }

        [Test]
        public void FromVectors_Negative_PiOnTwo() {
            var actual = new angle(a, -b);
            var expected = new angle(-math.PI / 2d);
            Assert.AreEqual(expected.radians, actual.radians, tolerance);
        }


        [Test]
        public void FromDouble_Negative_SevenPiOnTwo() {
            var actual = new angle(-7d * math.PI / 2d);
            var expected = new angle(math.PI / 2d);
            Assert.AreEqual(expected.radians, actual.radians, tolerance);
        }

        [Test]
        public void FromSingleVector() {
            var actual = new angle(b);
            var expected = new angle(math.PI / 2d);
            Assert.AreEqual(expected.radians, actual.radians, tolerance);
        }

        [Test]
        public void FromSingleVector_Signed() {
            var actual = new angle(-b);
            var expected = new angle(-math.PI / 2d);
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
            var actual = angle.wrap(math.PI / 2d);
            var expected = math.PI / 2d;
            Assert.AreEqual(expected, actual, tolerance);
        }

        [Test]
        public void Wrap_Pi() {
            var actual = angle.wrap(math.PI);
            var expected = -math.PI;
            Assert.AreEqual(expected, actual, tolerance);
        }

        [Test]
        public void Wrap_ThreePiOnTwo() {
            var actual = angle.wrap(3d * math.PI / 2d);
            var expected = -math.PI / 2d;
            Assert.AreEqual(expected, actual, tolerance);
        }

        [Test]
        public void Wrap_Negative_ThreePiOnTwo() {
            var actual = angle.wrap(-3d * math.PI / 2d);
            var expected = math.PI / 2d;
            Assert.AreEqual(expected, actual, tolerance);
        }

        [Test]
        public void Wrap_FivePiOnTwo() {
            var actual = angle.wrap(5d * math.PI / 2d);
            var expected = math.PI / 2d;
            Assert.AreEqual(expected, actual, tolerance);
        }

        [Test]
        public void Wrap_Negative_FivePiOnTwo() {
            var actual = angle.wrap(-5d * math.PI / 2d);
            var expected = -math.PI / 2d;
            Assert.AreEqual(expected, actual, tolerance);
        }

        [Test]
        public void Wrap_Negative_SevenPiOnTwo() {
            var actual = angle.wrap(-7d * math.PI / 2d);
            var expected = math.PI / 2d;
            Assert.AreEqual(expected, actual, tolerance);
        }
    }
}
