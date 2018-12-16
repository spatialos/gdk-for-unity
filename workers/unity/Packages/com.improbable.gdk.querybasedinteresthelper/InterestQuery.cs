using System.Collections.Generic;

namespace Improbable.Gdk.QueryBasedInterest
{
    /// <summary>
    ///     Utility class to help construct ComponentInterest.Query objects.
    /// </summary>
    public class InterestQuery
    {
        private static readonly List<uint> EmptyList = new List<uint>();

        private ComponentInterest.Query query;

        /// <summary>
        ///     Creates an InterestQuery.
        /// </summary>
        /// <param name="constraint">
        ///     A QueryConstraint object defining the constraints of the query.
        /// </param>
        /// <remarks>
        ///     Returns the full snapshot result by default.
        /// </remarks>
        /// <returns>
        ///     An InterestQuery object.
        /// </returns>
        public static InterestQuery Query(ComponentInterest.QueryConstraint constraint)
        {
            var interest = new InterestQuery
            {
                query =
                {
                    Constraint = constraint,
                    FullSnapshotResult = true,
                    ResultComponentId = EmptyList
                }
            };
            return interest;
        }

        /// <summary>
        ///     Sets the maximum frequency of the query.
        /// </summary>
        /// <param name="frequency">
        ///     The maximum frequency to return query results.
        /// </param>
        /// <returns>
        ///     An updated InterestQuery object.
        /// </returns>
        public InterestQuery WithMaxFrequencyHz(float frequency)
        {
            query.Frequency = frequency;
            return this;
        }

        /// <summary>
        ///     Sets the maximum frequency of the query.
        /// </summary>
        /// <param name="resultComponentIds">
        ///     IDs of components to return from the query results.
        /// </param>
        /// <remarks>
        ///     Does nothing if no component IDs are given.
        /// </remarks>
        /// <returns>
        ///     A ComponentInterest.Query object.
        /// </returns>
        public ComponentInterest.Query FilterResults(params uint[] resultComponentIds)
        {
            if (resultComponentIds.Length > 0)
            {
                query.FullSnapshotResult = null;
                query.ResultComponentId = new List<uint>(resultComponentIds);
            }

            return query;
        }

        public static implicit operator ComponentInterest.Query(InterestQuery interestQuery)
        {
            return interestQuery.query;
        }
    }
}
