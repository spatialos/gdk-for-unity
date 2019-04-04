using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.QueryBasedInterest
{
    /// <summary>
    ///     Utility class to help define QueryConstraint objects for Interest queries.
    /// </summary>
    public class Constraint
    {
        private ComponentInterest.QueryConstraint queryConstraint;

        private Constraint(ComponentInterest.QueryConstraint queryConstraint)
        {
            this.queryConstraint = queryConstraint;
        }

        private static ComponentInterest.QueryConstraint Default()
        {
            return new ComponentInterest.QueryConstraint
            {
                AndConstraint = new List<ComponentInterest.QueryConstraint>(),
                OrConstraint = new List<ComponentInterest.QueryConstraint>()
            };
        }

        /// <summary>
        ///     Creates a <see cref="Constraint"/> object with a Sphere QueryConstraint.
        /// </summary>
        /// <param name="radius">
        ///     Radius of the Sphere QueryConstraint.
        /// </param>
        /// <param name="center">
        ///     Center of the Sphere QueryConstraint.
        /// </param>
        /// <returns>
        ///     A <see cref="Constraint"/> object.
        /// </returns>
        public static Constraint Sphere(double radius, Coordinates center)
        {
            var constraint = Default();
            constraint.SphereConstraint = new ComponentInterest.SphereConstraint
            {
                Center = center,
                Radius = radius
            };
            return new Constraint(constraint);
        }

        /// <summary>
        ///     Creates a <see cref="Constraint"/> object with a Sphere QueryConstraint.
        /// </summary>
        /// <param name="radius">
        ///     Radius of the Sphere QueryConstraint.
        /// </param>
        /// <param name="centerX">
        ///     X coordinate of the center of the Sphere QueryConstraint.
        /// </param>
        /// <param name="centerY">
        ///     Y coordinate of the center of the Sphere QueryConstraint.
        /// </param>
        /// <param name="centerZ">
        ///     Z coordinate of the center of the Sphere QueryConstraint.
        /// </param>
        /// <returns>
        ///     A <see cref="Constraint"/> object.
        /// </returns>
        public static Constraint Sphere(
            double radius,
            double centerX,
            double centerY,
            double centerZ)
        {
            return Sphere(radius, new Coordinates(centerX, centerY, centerZ));
        }

        /// <summary>
        ///     Creates a <see cref="Constraint"/> object with a Cylinder QueryConstraint.
        /// </summary>
        /// <param name="radius">
        ///     Radius of the Cylinder QueryConstraint.
        /// </param>
        /// <param name="center">
        ///     Center of the Cylinder QueryConstraint.
        /// </param>
        /// <returns>
        ///     A <see cref="Constraint"/> object.
        /// </returns>
        public static Constraint Cylinder(double radius, Coordinates center)
        {
            var constraint = Default();
            constraint.CylinderConstraint = new ComponentInterest.CylinderConstraint
            {
                Center = center,
                Radius = radius
            };
            return new Constraint(constraint);
        }

        /// <summary>
        ///     Creates a <see cref="Constraint"/> object with a Cylinder QueryConstraint.
        /// </summary>
        /// <param name="radius">
        ///     Radius of the Cylinder QueryConstraint.
        /// </param>
        /// <param name="centerX">
        ///     X coordinate of the center of the Cylinder QueryConstraint.
        /// </param>
        /// <param name="centerY">
        ///     Y coordinate of the center of the Cylinder QueryConstraint.
        /// </param>
        /// <param name="centerZ">
        ///     Z coordinate of the center of the Cylinder QueryConstraint.
        /// </param>
        /// <returns>
        ///     A <see cref="Constraint"/> object.
        /// </returns>
        public static Constraint Cylinder(
            double radius,
            double centerX,
            double centerY,
            double centerZ)
        {
            return Cylinder(radius, new Coordinates(centerX, centerY, centerZ));
        }

        /// <summary>
        ///     Creates a <see cref="Constraint"/> object with a Box queryConstraint.
        /// </summary>
        /// <param name="xWidth">
        ///     Width of Box QueryConstraint in the X-axis.
        /// </param>
        /// <param name="yHeight">
        ///     Height of Box QueryConstraint in the Y-axis.
        /// </param>
        /// <param name="zDepth">
        ///     Depth of Box QueryConstraint in the Z-axis.
        /// </param>
        /// <param name="center">
        ///     Center of the Box QueryConstraint.
        /// </param>
        /// <returns>
        ///     A <see cref="Constraint"/> object.
        /// </returns>
        public static Constraint Box(
            double xWidth,
            double yHeight,
            double zDepth,
            Coordinates center)
        {
            var constraint = Default();
            constraint.BoxConstraint = new ComponentInterest.BoxConstraint
            {
                Center = center,
                EdgeLength = new EdgeLength(xWidth, yHeight, zDepth)
            };
            return new Constraint(constraint);
        }

        /// <summary>
        ///     Creates a <see cref="Constraint"/> object with a Box QueryConstraint.
        /// </summary>
        /// <param name="xWidth">
        ///     Width of Box QueryConstraint in the X-axis.
        /// </param>
        /// <param name="yHeight">
        ///     Height of Box QueryConstraint in the Y-axis.
        /// </param>
        /// <param name="zDepth">
        ///     Depth of Box QueryConstraint in the Z-axis.
        /// </param>
        /// <param name="centerX">
        ///     X coordinate of the center of the Box QueryConstraint.
        /// </param>
        /// <param name="centerY">
        ///     Y coordinate of the center of the Box QueryConstraint.
        /// </param>
        /// <param name="centerZ">
        ///     Z coordinate of the center of the Box QueryConstraint.
        /// </param>
        /// <returns>
        ///     A <see cref="Constraint"/> object.
        /// </returns>
        public static Constraint Box(
            double xWidth,
            double yHeight,
            double zDepth,
            double centerX,
            double centerY,
            double centerZ)
        {
            return Box(xWidth, yHeight, zDepth, new Coordinates(centerX, centerY, centerZ));
        }

        /// <summary>
        ///     Creates a <see cref="Constraint"/> object with a RelativeSphere QueryConstraint.
        /// </summary>
        /// <param name="radius">
        ///     Radius of the RelativeSphere QueryConstraint.
        /// </param>
        /// <remarks>
        ///     This <see cref="Constraint"/> defines a sphere relative to the position of the entity.
        /// </remarks>
        /// <returns>
        ///     A <see cref="Constraint"/> object.
        /// </returns>
        public static Constraint RelativeSphere(double radius)
        {
            var constraint = Default();
            constraint.RelativeSphereConstraint = new ComponentInterest.RelativeSphereConstraint
            {
                Radius = radius
            };
            return new Constraint(constraint);
        }

        /// <summary>
        ///     Creates a <see cref="Constraint"/> object with a RelativeCylinder QueryConstraint.
        /// </summary>
        /// <param name="radius">
        ///     Radius of the cylinder QueryConstraint.
        /// </param>
        /// <remarks>
        ///     This <see cref="Constraint"/> defines a cylinder relative to the position of the entity.
        /// </remarks>
        /// <returns>
        ///     A <see cref="Constraint"/> object.
        /// </returns>
        public static Constraint RelativeCylinder(double radius)
        {
            var constraint = Default();
            constraint.RelativeCylinderConstraint = new ComponentInterest.RelativeCylinderConstraint
            {
                Radius = radius
            };
            return new Constraint(constraint);
        }

        /// <summary>
        ///     Creates a <see cref="Constraint"/> object with a RelativeBox QueryConstraint.
        /// </summary>
        /// <param name="xWidth">
        ///     Width of box QueryConstraint in the X-axis.
        /// </param>
        /// <param name="yHeight">
        ///     Height of box QueryConstraint in the Y-axis.
        /// </param>
        /// <param name="zDepth">
        ///     Depth of box QueryConstraint in the Z-axis.
        /// </param>
        /// <remarks>
        ///     This <see cref="Constraint"/> defines a box relative to the position of the entity.
        /// </remarks>
        /// <returns>
        ///     A <see cref="Constraint"/> object.
        /// </returns>
        public static Constraint RelativeBox(double xWidth, double yHeight, double zDepth)
        {
            var constraint = Default();
            constraint.RelativeBoxConstraint = new ComponentInterest.RelativeBoxConstraint
            {
                EdgeLength = new EdgeLength(xWidth, yHeight, zDepth)
            };
            return new Constraint(constraint);
        }

        /// <summary>
        ///     Creates a <see cref="Constraint"/> object with an EntityId QueryConstraint.
        /// </summary>
        /// <param name="entityId">
        ///     EntityId of an entity to interested in.
        /// </param>
        /// <returns>
        ///     A <see cref="Constraint"/> object.
        /// </returns>
        public static Constraint EntityId(EntityId entityId)
        {
            var constraint = Default();
            constraint.EntityIdConstraint = entityId.Id;
            return new Constraint(constraint);
        }

        /// <summary>
        ///     Creates a <see cref="Constraint"/> object with an Component QueryConstraint.
        /// </summary>
        /// <typeparam name="T">
        ///     Type of the component to constrain.
        /// </typeparam>
        /// <returns>
        ///     A <see cref="Constraint"/> object.
        /// </returns>
        public static Constraint Component<T>() where T : ISpatialComponentData
        {
            var constraint = Default();
            constraint.ComponentConstraint = Dynamic.GetComponentId<T>();
            return new Constraint(constraint);
        }

        /// <summary>
        ///     Creates a <see cref="Constraint"/> object with a Component QueryConstraint.
        /// </summary>
        /// <param name="componentId">
        ///     Component ID of the component to constrain.
        /// </param>
        /// <returns>
        ///     A <see cref="Constraint"/> object.
        /// </returns>
        public static Constraint Component(uint componentId)
        {
            var constraint = Default();
            constraint.ComponentConstraint = componentId;
            return new Constraint(constraint);
        }

        /// <summary>
        ///     Creates a <see cref="Constraint"/> object with an And QueryConstraint.
        /// </summary>
        /// <param name="constraint">
        ///     First <see cref="Constraint"/> in the list of conjunctions.
        /// </param>
        /// <param name="constraints">
        ///     Further Constraints for the list of conjunctions.
        /// </param>
        /// <remarks>
        ///     At least one <see cref="Constraint"/> must be provided to create a valid "All" QueryConstraint.
        /// </remarks>
        /// <returns>
        ///     A <see cref="Constraint"/> object.
        /// </returns>
        public static Constraint All(Constraint constraint, params Constraint[] constraints)
        {
            return new Constraint(new ComponentInterest.QueryConstraint
            {
                AndConstraint = ConstraintParamsToQueryConstraints(constraint, constraints),
                OrConstraint = new List<ComponentInterest.QueryConstraint>()
            });
        }

        /// <summary>
        ///     Creates a <see cref="Constraint"/> object with an And QueryConstraint.
        /// </summary>
        /// <param name="constraints">
        ///     Constraints for the list of conjunctions.
        /// </param>
        /// <remarks>
        ///     At least one <see cref="Constraint"/> must be provided to create a valid "All" QueryConstraint.
        /// </remarks>
        /// <returns>
        ///     A <see cref="Constraint"/> object.
        /// </returns>
        public static Constraint All(IEnumerable<Constraint> constraints)
        {
            if (!constraints.Any())
            {
                throw new ArgumentException("At least one Constraint must be provided.");
            }

            return new Constraint(new ComponentInterest.QueryConstraint
            {
                AndConstraint = ConstraintEnumerableToQueryConstraints(constraints),
                OrConstraint = new List<ComponentInterest.QueryConstraint>()
            });
        }

        /// <summary>
        ///     Creates a <see cref="Constraint"/> object with an Or QueryConstraint.
        /// </summary>
        /// <param name="constraint">
        ///     First <see cref="Constraint"/> in the list of disjunctions.
        /// </param>
        /// <param name="constraints">
        ///     Further Constraints for the list of disjunctions.
        /// </param>
        /// <remarks>
        ///     At least one <see cref="Constraint"/> must be provided to create a valid "Any" QueryConstraint.
        /// </remarks>
        /// <returns>
        ///     A <see cref="Constraint"/> object.
        /// </returns>
        public static Constraint Any(Constraint constraint, params Constraint[] constraints)
        {
            return new Constraint(new ComponentInterest.QueryConstraint
            {
                AndConstraint = new List<ComponentInterest.QueryConstraint>(),
                OrConstraint = ConstraintParamsToQueryConstraints(constraint, constraints)
            });
        }

        /// <summary>
        ///     Creates a <see cref="Constraint"/> object with an Or queryConstraint.
        /// </summary>
        /// <param name="constraints">
        ///     Set of Constraints for the list of disjunctions.
        /// </param>
        /// <remarks>
        ///     At least one <see cref="Constraint"/> must be provided to create a valid "Any" QueryConstraint.
        /// </remarks>
        /// <returns>
        ///     A <see cref="Constraint"/> object.
        /// </returns>
        public static Constraint Any(IEnumerable<Constraint> constraints)
        {
            if (!constraints.Any())
            {
                throw new ArgumentException("At least one Constraint must be provided.");
            }

            return new Constraint(new ComponentInterest.QueryConstraint
            {
                AndConstraint = new List<ComponentInterest.QueryConstraint>(),
                OrConstraint = ConstraintEnumerableToQueryConstraints(constraints)
            });
        }

        /// <summary>
        ///     Returns a QueryConstraint object from a <see cref="Constraint"/>.
        /// </summary>
        /// <returns>
        ///     A QueryConstraint object.
        /// </returns>
        public ComponentInterest.QueryConstraint AsQueryConstraint()
        {
            return queryConstraint;
        }

        private static List<ComponentInterest.QueryConstraint> ConstraintParamsToQueryConstraints(
            Constraint constraint,
            params Constraint[] constraints)
        {
            var output = new List<ComponentInterest.QueryConstraint>(constraints.Length + 1)
            {
                constraint.queryConstraint
            };

            foreach (var constraintParam in constraints)
            {
                output.Add(constraintParam.queryConstraint);
            }

            return output;
        }

        private static List<ComponentInterest.QueryConstraint> ConstraintEnumerableToQueryConstraints(
            IEnumerable<Constraint> constraints)
        {
            return constraints.Select(constraint => constraint.queryConstraint).ToList();
        }
    }
}
