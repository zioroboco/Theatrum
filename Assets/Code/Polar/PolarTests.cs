using System;
using NUnit.Framework;
using Unity;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.TestTools;

public class PolarTests {

    private static double tolerance = 1E-15d;

    public class Position {

        [Test]
        public void PolarPositionFromVector_InDirectionOfY() {
            var actual = Polar.toPolarPosition(new double2(0d, 1d));
            var expected = new Polar.Position {
                r = 1d,
                theta = math.PI / 2d
            };
            Assert.AreEqual(expected.r, actual.r, tolerance);
            Assert.AreEqual(expected.theta, actual.theta, tolerance);
        }

        [Test]
        public void PolarPositionFromVector_InDirectionOfX() {
            var actual = Polar.toPolarPosition(new double2(1d, 0d));
            var expected = new Polar.Position {
                r = 1d,
                theta = 0d
            };
            Assert.AreEqual(expected.r, actual.r, tolerance);
            Assert.AreEqual(expected.theta, actual.theta, tolerance);
        }

        [Test]
        public void PolarPositionFromVector_InDirectionOfX_AndLong() {
            var actual = Polar.toPolarPosition(new double2(2d, 0d));
            var expected = new Polar.Position {
                r = 2d,
                theta = 0d
            };
            Assert.AreEqual(expected.r, actual.r, tolerance);
            Assert.AreEqual(expected.theta, actual.theta, tolerance);
        }

        [Test]
        public void PolarPositionFromVector_AtAngle() {
            var actual = Polar.toPolarPosition(new double2(1d, 1d));
            var expected = new Polar.Position {
                r = math.sqrt(2d),
                theta = math.PI / 4d
            };
            Assert.AreEqual(expected.r, actual.r, tolerance);
            Assert.AreEqual(expected.theta, actual.theta, tolerance);
        }
    }

    public class Vector {

        [Test]
        public void FromZeroVectorAtOrigin() {
            var v = new double2(0d, 0d);
            var p = new Polar.Position { r = 0d, theta = 0d };
            var actual = Polar.toPolarVector(v, p);
            var expected = new double2(0d, 0d);
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void FromVectorAtOriginWithRadialComponentOnly() {
            var v = new double2(1d, 0d);
            var p = new Polar.Position { r = 0d, theta = 0d };
            var actual = Polar.toPolarVector(v, p);
            var expected = new double2(0d, 1d);
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void FromVector_East() {
            var v = new double2(1d, 1d);
            var p = new Polar.Position { r = 1d, theta = 0d };
            var actual = Polar.toPolarVector(v, p);
            var expected = new double2(-1d, 1d);
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void FromVector_North() {
            var v = new double2(1d, 1d);
            var p = new Polar.Position { r = 1d, theta = math.PI / 2d };
            var actual = Polar.toPolarVector(v, p);
            var expected = new double2(1d, 1d);
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void FromVector_West() {
            var v = new double2(1d, 1d);
            var p = new Polar.Position { r = 1d, theta = math.PI };
            var actual = Polar.toPolarVector(v, p);
            var expected = new double2(1d, -1d);
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void FromVector_South() {
            var v = new double2(1d, 1d);
            var p = new Polar.Position { r = 1d, theta = (3d / 2d) * math.PI };
            var actual = Polar.toPolarVector(v, p);
            var expected = new double2(-1d, -1d);
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }
    }

    public class ToVector {

        [Test]
        public void ToWorldVector_WithoutMagnitudeOrDirection() {
            var actual = Polar.toVector(new Polar.Position { r = 0d, theta = 0d });
            var expected = new double2(0d, 0d);
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void ToWorldVector_WithOnlyMagnitude() {
            var actual = Polar.toVector(new Polar.Position { r = 2d, theta = 0d });
            var expected = new double2(2d, 0d);
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void ToWorldVector_WithMagnitudeAndDirection() {
            var actual = Polar.toVector(new Polar.Position { r = 2d, theta = math.PI / 2d });
            var expected = new double2(0d, 2d);
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void ToWorldVector_WithMagnitudeAndNegativeDirection() {
            var actual = Polar.toVector(new Polar.Position { r = 2d, theta = math.PI });
            var expected = new double2(-2d, 0d);
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }
    }

    public class RHat {

        [Test]
        public void Zero() {
            var expected = new double2(1d, 0d);
            var actual = Polar.rHat(0d);
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void PiOnTwo() {
            var expected = new double2(0d, 1d);
            var actual = Polar.rHat(math.PI / 2d);
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void Pi() {
            var expected = new double2(-1d, 0d);
            var actual = Polar.rHat(math.PI);
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void ThreePiOnTwo() {
            var expected = new double2(0d, -1d);
            var actual = Polar.rHat((3d / 2d) * math.PI);
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }
    }

    public class ThetaHat {

        [Test]
        public void Zero() {
            var expected = new double2(0d, -1d);
            var actual = Polar.thetaHat(0d);
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void PiOnTwo() {
            var expected = new double2(1d, 0d);
            var actual = Polar.thetaHat(math.PI / 2d);
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void Pi() {
            var expected = new double2(0d, 1d);
            var actual = Polar.thetaHat(math.PI);
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }

        [Test]
        public void ThreePiOnTwo() {
            var expected = new double2(-1d, 0d);
            var actual = Polar.thetaHat((3d / 2d) * math.PI);
            Assert.AreEqual(expected.x, actual.x, tolerance);
            Assert.AreEqual(expected.y, actual.y, tolerance);
        }
    }
}
