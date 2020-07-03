using System;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.QueryBasedInterest;
using NUnit.Framework;

namespace Improbable.Gdk.EditmodeTests.Utility
{
    [TestFixture]
    public class ConstraintTests
    {
        private const double Distance = 10;

        private static Constraint BasicConstraint => Constraint.Component<Position.Component>();

        [Test]
        public void All_constraint_throws_if_enumerable_empty()
        {
            Assert.Throws<ArgumentException>(() => Constraint.All(new List<Constraint>()));
        }

        [Test]
        public void Any_constraint_throws_if_enumerable_empty()
        {
            Assert.Throws<ArgumentException>(() => Constraint.Any(new List<Constraint>()));
        }

        [Test]
        public void All_constraint_param_sets_OrConstraint_to_empty_list()
        {
            var constraint = Constraint.All(BasicConstraint);
            Assert.AreEqual(constraint.AsQueryConstraint().AndConstraint.Count, 1);
            Assert.AreEqual(0, constraint.AsQueryConstraint().OrConstraint.Count);
        }

        [Test]
        public void All_constraint_params_sets_OrConstraint_to_empty_list()
        {
            var constraint = Constraint.All(BasicConstraint, BasicConstraint, BasicConstraint);
            Assert.AreEqual(constraint.AsQueryConstraint().AndConstraint.Count, 3);
            Assert.AreEqual(0, constraint.AsQueryConstraint().OrConstraint.Count);
        }

        [Test]
        public void All_constraint_enumerable_sets_OrConstraint_to_empty_list()
        {
            var constraint = Constraint.All(new List<Constraint> { BasicConstraint });
            Assert.AreEqual(constraint.AsQueryConstraint().AndConstraint.Count, 1);
            Assert.AreEqual(0, constraint.AsQueryConstraint().OrConstraint.Count);
        }

        [Test]
        public void Any_constraint_param_sets_AndConstraint_to_empty_list()
        {
            var constraint = Constraint.Any(BasicConstraint);
            Assert.AreEqual(0, constraint.AsQueryConstraint().AndConstraint.Count);
            Assert.AreEqual(constraint.AsQueryConstraint().OrConstraint.Count, 1);
        }

        [Test]
        public void Any_constraint_params_sets_AndConstraint_to_empty_list()
        {
            var constraint = Constraint.Any(BasicConstraint, BasicConstraint, BasicConstraint);
            Assert.AreEqual(0, constraint.AsQueryConstraint().AndConstraint.Count);
            Assert.AreEqual(constraint.AsQueryConstraint().OrConstraint.Count, 3);
        }

        [Test]
        public void Any_constraint_enumerable_sets_AndConstraint_to_empty_list()
        {
            var constraint = Constraint.Any(new List<Constraint> { BasicConstraint });
            Assert.AreEqual(0, constraint.AsQueryConstraint().AndConstraint.Count);
            Assert.AreEqual(constraint.AsQueryConstraint().OrConstraint.Count, 1);
        }

        [Test]
        public void Sphere_constraint_sets_AndOr_constraints_to_empty_list()
        {
            var constraint = Constraint.Sphere(Distance, Coordinates.Zero);
            Assert.AreEqual(0, constraint.AsQueryConstraint().AndConstraint.Count);
            Assert.AreEqual(0, constraint.AsQueryConstraint().OrConstraint.Count);
        }

        [Test]
        public void Sphere_constraint_xyz_sets_AndOr_constraints_to_empty_list()
        {
            var constraint = Constraint.Sphere(Distance, 0, 0, 0);
            Assert.AreEqual(0, constraint.AsQueryConstraint().AndConstraint.Count);
            Assert.AreEqual(0, constraint.AsQueryConstraint().OrConstraint.Count);
        }

        [Test]
        public void Cylinder_constraint_sets_AndOr_constraints_to_empty_list()
        {
            var constraint = Constraint.Cylinder(Distance, Coordinates.Zero);
            Assert.AreEqual(0, constraint.AsQueryConstraint().AndConstraint.Count);
            Assert.AreEqual(0, constraint.AsQueryConstraint().OrConstraint.Count);
        }

        [Test]
        public void Cylinder_constraint_xyz_sets_AndOr_constraints_to_empty_list()
        {
            var constraint = Constraint.Cylinder(Distance, 0, 0, 0);
            Assert.AreEqual(0, constraint.AsQueryConstraint().AndConstraint.Count);
            Assert.AreEqual(0, constraint.AsQueryConstraint().OrConstraint.Count);
        }

        [Test]
        public void Box_constraint_sets_AndOr_constraints_to_empty_list()
        {
            var constraint = Constraint.Box(Distance, Distance, Distance, Coordinates.Zero);
            Assert.AreEqual(0, constraint.AsQueryConstraint().AndConstraint.Count);
            Assert.AreEqual(0, constraint.AsQueryConstraint().OrConstraint.Count);
        }

        [Test]
        public void Box_constraint__xyz_sets_AndOr_constraints_to_empty_list()
        {
            var constraint = Constraint.Box(Distance, Distance, Distance, 0, 0, 0);
            Assert.AreEqual(0, constraint.AsQueryConstraint().AndConstraint.Count);
            Assert.AreEqual(0, constraint.AsQueryConstraint().OrConstraint.Count);
        }

        [Test]
        public void RelativeSphere_constraint_sets_AndOr_constraints_to_empty_list()
        {
            var constraint = Constraint.RelativeSphere(Distance);
            Assert.AreEqual(0, constraint.AsQueryConstraint().AndConstraint.Count);
            Assert.AreEqual(0, constraint.AsQueryConstraint().OrConstraint.Count);
        }

        [Test]
        public void RelativeCylinder_constraint_sets_AndOr_constraints_to_empty_list()
        {
            var constraint = Constraint.RelativeCylinder(Distance);
            Assert.AreEqual(0, constraint.AsQueryConstraint().AndConstraint.Count);
            Assert.AreEqual(0, constraint.AsQueryConstraint().OrConstraint.Count);
        }

        [Test]
        public void RelativeBox_constraint_sets_AndOr_constraints_to_empty_list()
        {
            var constraint = Constraint.RelativeBox(Distance, Distance, Distance);
            Assert.AreEqual(0, constraint.AsQueryConstraint().AndConstraint.Count);
            Assert.AreEqual(0, constraint.AsQueryConstraint().OrConstraint.Count);
        }

        [Test]
        public void EntityId_constraint_sets_AndOr_constraints_to_empty_list()
        {
            var constraint = Constraint.EntityId(new EntityId(10));
            Assert.AreEqual(0, constraint.AsQueryConstraint().AndConstraint.Count);
            Assert.AreEqual(0, constraint.AsQueryConstraint().OrConstraint.Count);
        }

        [Test]
        public void Component_constraint_sets_AndOr_constraints_to_empty_list()
        {
            var constraint = Constraint.Component(Position.ComponentId);
            Assert.AreEqual(0, constraint.AsQueryConstraint().AndConstraint.Count);
            Assert.AreEqual(0, constraint.AsQueryConstraint().OrConstraint.Count);
        }

        [Test]
        public void Component_T_constraint_sets_AndOr_constraints_to_empty_list()
        {
            var constraint = Constraint.Component<Position.Component>();
            Assert.AreEqual(0, constraint.AsQueryConstraint().AndConstraint.Count);
            Assert.AreEqual(0, constraint.AsQueryConstraint().OrConstraint.Count);
        }
    }
}
