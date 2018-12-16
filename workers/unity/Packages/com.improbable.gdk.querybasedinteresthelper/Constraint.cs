using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;
using UnityEngine.Assertions;

namespace Improbable.Gdk.QueryBasedInterest
{
    /// <summary>
    ///     Utility class to help construct QueryConstraint objects.
    /// </summary>
    public static class Constraint
    {
        private static readonly List<ComponentInterest.QueryConstraint> EmptyList
            = new List<ComponentInterest.QueryConstraint>();

        private static ComponentInterest.QueryConstraint Default()
        {
            return new ComponentInterest.QueryConstraint
            {
                AndConstraint = EmptyList,
                OrConstraint = EmptyList
            };
        }

        /// <summary>
        ///     Creates a QueryConstraint object with a Sphere constraint.
        /// </summary>
        /// <param name="radius">
        ///     Radius of the Sphere constraint.
        /// </param>
        /// <param name="center">
        ///     Center of the Sphere constraint.
        /// </param>
        /// <returns>
        ///     A QueryConstraint object.
        /// </returns>
        public static ComponentInterest.QueryConstraint Sphere(double radius, Vector3 center)
        {
            var constraint = Default();
            constraint.SphereConstraint = new ComponentInterest.SphereConstraint
            {
                Center = new Coordinates(center.x, center.y, center.z),
                Radius = radius
            };
            return constraint;
        }

        /// <summary>
        ///     Creates a QueryConstraint object with a Cylinder constraint.
        /// </summary>
        /// <param name="radius">
        ///     Radius of the Cylinder constraint.
        /// </param>
        /// <param name="center">
        ///     Center of the Cylinder constraint.
        /// </param>
        /// <returns>
        ///     A QueryConstraint object.
        /// </returns>
        public static ComponentInterest.QueryConstraint Cylinder(double radius, Vector3 center)
        {
            var constraint = Default();
            constraint.CylinderConstraint = new ComponentInterest.CylinderConstraint()
            {
                Center = new Coordinates(center.x, center.y, center.z),
                Radius = radius
            };
            return constraint;
        }

        /// <summary>
        ///     Creates a QueryConstraint object with a Box constraint.
        /// </summary>
        /// <param name="xWidth">
        ///     Width of Box constraint in the X-axis.
        /// </param>
        /// <param name="yHeight">
        ///     Height of Box constraint in the Y-axis.
        /// </param>
        /// <param name="zDepth">
        ///     Depth of Box constraint in the Z-axis.
        /// </param>
        /// <param name="center">
        ///     Center of the Box constraint.
        /// </param>
        /// <returns>
        ///     A QueryConstraint object.
        /// </returns>
        public static ComponentInterest.QueryConstraint Box(double xWidth, double yHeight, double zDepth, Vector3 center)
        {
            var constraint = Default();
            constraint.BoxConstraint = new ComponentInterest.BoxConstraint
            {
                Center = new Coordinates(center.x, center.y, center.z),
                EdgeLength = new EdgeLength(xWidth, yHeight, zDepth)
            };
            return constraint;
        }

        /// <summary>
        ///     Creates a QueryConstraint object with a RelativeSphere constraint.
        /// </summary>
        /// <param name="radius">
        ///     Radius of the RelativeSphere constraint.
        /// </param>
        /// <remarks>
        ///     This constraint defines a sphere relative to the position of the entity.
        /// </remarks>
        /// <returns>
        ///     A QueryConstraint object.
        /// </returns>
        public static ComponentInterest.QueryConstraint RelativeSphere(double radius)
        {
            var constraint = Default();
            constraint.RelativeSphereConstraint = new ComponentInterest.RelativeSphereConstraint
            {
                Radius = radius
            };
            return constraint;
        }

        /// <summary>
        ///     Creates a QueryConstraint object with a RelativeCylinder constraint.
        /// </summary>
        /// <param name="radius">
        ///     Radius of the cylinder constraint.
        /// </param>
        /// <remarks>
        ///     This constraint defines a cylinder relative to the position of the entity.
        /// </remarks>
        /// <returns>
        ///     A QueryConstraint object.
        /// </returns>
        public static ComponentInterest.QueryConstraint RelativeCylinder(double radius)
        {
            var constraint = Default();
            constraint.RelativeCylinderConstraint = new ComponentInterest.RelativeCylinderConstraint
            {
                Radius = radius
            };
            return constraint;
        }

        /// <summary>
        ///     Creates a QueryConstraint object with a RelativeBox constraint.
        /// </summary>
        /// <param name="xWidth">
        ///     Width of box constraint in the X-axis.
        /// </param>
        /// <param name="yHeight">
        ///     Height of box constraint in the Y-axis.
        /// </param>
        /// <param name="zDepth">
        ///     Depth of box constraint in the Z-axis.
        /// </param>
        /// <remarks>
        ///     This constraint defines a box relative to the position of the entity.
        /// </remarks>
        /// <returns>
        ///     A QueryConstraint object.
        /// </returns>
        public static ComponentInterest.QueryConstraint RelativeBox(double xWidth, double yHeight, double zDepth)
        {
            var constraint = Default();
            constraint.RelativeBoxConstraint = new ComponentInterest.RelativeBoxConstraint
            {
                EdgeLength = new EdgeLength(xWidth, yHeight, zDepth)
            };
            return constraint;
        }

        /// <summary>
        ///     Creates a QueryConstraint object with an EntityId constraint.
        /// </summary>
        /// <param name="entityId">
        ///     EntityId of the .
        /// </param>
        /// <returns>
        ///     A QueryConstraint object.
        /// </returns>
        public static ComponentInterest.QueryConstraint EntityId(EntityId entityId)
        {
            var constraint = Default();
            constraint.EntityIdConstraint = entityId.Id;
            return constraint;
        }

        /// <summary>
        ///     Creates a QueryConstraint object with an EntityId constraint.
        /// </summary>
        /// <typeparam name="T">
        ///     Type of the component to constrain.
        /// </typeparam>
        /// <returns>
        ///     A QueryConstraint object.
        /// </returns>
        public static ComponentInterest.QueryConstraint Component<T>() where T : ISpatialComponentData
        {
            var constraint = Default();
            constraint.ComponentConstraint = Dynamic.GetComponentId<T>();
            return constraint;
        }

        /// <summary>
        ///     Creates a QueryConstraint object with a Component constraint.
        /// </summary>
        /// <param name="componentId">
        ///     ID of the component to constrain.
        /// </param>
        /// <returns>
        ///     A QueryConstraint object.
        /// </returns>
        public static ComponentInterest.QueryConstraint Component(uint componentId)
        {
            var constraint = Default();
            constraint.ComponentConstraint = componentId;
            return constraint;
        }

        /// <summary>
        ///     Creates a QueryConstraint object with an And constraint.
        /// </summary>
        /// <param name="constraint">
        ///     First constraint in the list of conjunctions.
        /// </param>
        /// <param name="constraints">
        ///     Further constraints for the list of conjunctions.
        /// </param>
        /// <remarks>
        ///     At least one constraint must be provided to create an "All" QueryConstraint.
        /// </remarks>
        /// <returns>
        ///     A QueryConstraint object.
        /// </returns>
        public static ComponentInterest.QueryConstraint All(ComponentInterest.QueryConstraint constraint,
            params ComponentInterest.QueryConstraint[] constraints)
        {
            var andConstraints = new List<ComponentInterest.QueryConstraint>(constraints.Length + 1) {constraint};
            andConstraints.AddRange(constraints);

            return new ComponentInterest.QueryConstraint
            {
                AndConstraint = andConstraints,
                OrConstraint = EmptyList
            };
        }

        /// <summary>
        ///     Creates a QueryConstraint object with an Or constraint.
        /// </summary>
        /// <param name="constraint">
        ///     First constraint in the list of disjunctions.
        /// </param>
        /// <param name="constraints">
        ///     Further constraints for the list of disjunctions.
        /// </param>
        /// <remarks>
        ///     At least one constraint must be provided to create an "Any" QueryConstraint.
        /// </remarks>
        /// <returns>
        ///     A QueryConstraint object.
        /// </returns>
        public static ComponentInterest.QueryConstraint Any(ComponentInterest.QueryConstraint constraint,
            params ComponentInterest.QueryConstraint[] constraints)
        {
            var orConstraints = new List<ComponentInterest.QueryConstraint>(constraints.Length + 1) {constraint};
            orConstraints.AddRange(constraints);

            return new ComponentInterest.QueryConstraint
            {
                AndConstraint = EmptyList,
                OrConstraint = orConstraints
            };
        }
    }
}
