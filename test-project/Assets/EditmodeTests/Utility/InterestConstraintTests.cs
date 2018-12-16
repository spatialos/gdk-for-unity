using Improbable.Gdk.Core;
using Improbable.Gdk.QueryBasedInterest;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.Utility
{
    [TestFixture]
    public class InterestConstraintTests
    {
        private const double Distance = 10;

        private static ComponentInterest.QueryConstraint BasicConstraint => Constraint.Component<Position.Component>();

        [Test]
        public void All_constraint_sets_OrConstraint_to_empty_list()
        {
            var constraint = Constraint.All(BasicConstraint);
            Assert.Greater(constraint.AndConstraint.Count, 0);
            Assert.AreEqual(0, constraint.OrConstraint.Count);
        }

        [Test]
        public void Any_constraint_sets_AndConstraint_to_empty_list()
        {
            var constraint = Constraint.Any(BasicConstraint);
            Assert.AreEqual(0, constraint.AndConstraint.Count);
            Assert.Greater(constraint.OrConstraint.Count, 0);
        }

        [Test]
        public void Sphere_constraint_sets_AndOr_constraints_to_empty_list()
        {
            var constraint = Constraint.Sphere(Distance, Vector3.zero);
            Assert.AreEqual(0, constraint.AndConstraint.Count);
            Assert.AreEqual(0, constraint.OrConstraint.Count);
        }

        [Test]
        public void Cylinder_constraint_sets_AndOr_constraints_to_empty_list()
        {
            var constraint = Constraint.Cylinder(Distance, Vector3.zero);
            Assert.AreEqual(0, constraint.AndConstraint.Count);
            Assert.AreEqual(0, constraint.OrConstraint.Count);
        }

        [Test]
        public void Box_constraint_sets_AndOr_constraints_to_empty_list()
        {
            var constraint = Constraint.Box(Distance, Distance, Distance, Vector3.zero);
            Assert.AreEqual(0, constraint.AndConstraint.Count);
            Assert.AreEqual(0, constraint.OrConstraint.Count);
        }

        [Test]
        public void RelativeSphere_constraint_sets_AndOr_constraints_to_empty_list()
        {
            var constraint = Constraint.RelativeSphere(Distance);
            Assert.AreEqual(0, constraint.AndConstraint.Count);
            Assert.AreEqual(0, constraint.OrConstraint.Count);
        }

        [Test]
        public void RelativeCylinder_constraint_sets_AndOr_constraints_to_empty_list()
        {
            var constraint = Constraint.RelativeCylinder(Distance);
            Assert.AreEqual(0, constraint.AndConstraint.Count);
            Assert.AreEqual(0, constraint.OrConstraint.Count);
        }

        [Test]
        public void RelativeBox_constraint_sets_AndOr_constraints_to_empty_list()
        {
            var constraint = Constraint.RelativeBox(Distance, Distance, Distance);
            Assert.AreEqual(0, constraint.AndConstraint.Count);
            Assert.AreEqual(0, constraint.OrConstraint.Count);
        }

        [Test]
        public void EntityId_constraint_sets_AndOr_constraints_to_empty_list()
        {
            var constraint = Constraint.EntityId(new EntityId(10));
            Assert.AreEqual(0, constraint.AndConstraint.Count);
            Assert.AreEqual(0, constraint.OrConstraint.Count);
        }

        [Test]
        public void Component_constraint_sets_AndOr_constraints_to_empty_list()
        {
            var constraint = Constraint.Component(Position.ComponentId);
            Assert.AreEqual(0, constraint.AndConstraint.Count);
            Assert.AreEqual(0, constraint.OrConstraint.Count);
        }

        [Test]
        public void Component_T__constraint_sets_AndOr_constraints_to_empty_list()
        {
            var constraint = Constraint.Component<Position.Component>();
            Assert.AreEqual(0, constraint.AndConstraint.Count);
            Assert.AreEqual(0, constraint.OrConstraint.Count);
        }
    }
}
