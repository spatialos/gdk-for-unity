using System.Collections.Generic;
using System.Linq;

namespace Improbable.Gdk.QueryBasedInterest
{
    /// <summary>
    ///     Utility class to help construct ComponentInterest.Query objects.
    /// </summary>
    public class InterestQuery
    {
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
                    ResultComponentId = new List<uint>()
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
        ///     Defines what components to return in the query results.
        /// </summary>
        /// <param name="resultComponentId">
        ///     First ID of a component to return from the query results.
        /// </param>
        /// <param name="resultComponentIds">
        ///     Further IDs of components to return from the query results.
        /// </param>
        /// <remarks>
        ///     At least one component ID must be provided.
        /// </remarks>
        /// <returns>
        ///     A ComponentInterest.Query object.
        /// </returns>
        public ComponentInterest.Query FilterResults(uint resultComponentId, params uint[] resultComponentIds)
        {
            var resultIds = new List<uint>(resultComponentIds.Length + 1) { resultComponentId };
            resultIds.AddRange(resultComponentIds);

            query.FullSnapshotResult = null;
            query.ResultComponentId = resultIds;

            return query;
        }

        public static implicit operator ComponentInterest.Query(InterestQuery interestQuery)
        {
            return interestQuery.query;
        }
    }
}
