using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Improbable.Gdk.QueryBasedInterest
{
    /// <summary>
    ///     Utility class to help construct ComponentInterest.Query objects.
    /// </summary>
    public class InterestQuery
    {
        private Constraint queryConstraint;
        private ComponentInterest.Query query;

        // Hides the default constructor
        private InterestQuery()
        {
        }

        /// <summary>
        ///     Creates an InterestQuery.
        /// </summary>
        /// <param name="constraint">
        ///     A Constraint object defining the constraints of the query.
        /// </param>
        /// <remarks>
        ///     Returns the full snapshot result by default.
        /// </remarks>
        /// <returns>
        ///     An InterestQuery object.
        /// </returns>
        public static InterestQuery Query(Constraint constraint)
        {
            var interest = new InterestQuery
            {
                queryConstraint = constraint,
                query =
                {
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
        ///     An updated InterestQuery object.
        /// </returns>
        public InterestQuery FilterResults(uint resultComponentId, params uint[] resultComponentIds)
        {
            var resultIds = new List<uint>(resultComponentIds.Length + 1) { resultComponentId };
            resultIds.AddRange(resultComponentIds);

            return FilterResults(resultIds);
        }

        /// <summary>
        ///     Defines what components to return in the query results.
        /// </summary>
        /// <param name="resultComponentIds">
        ///     Set of IDs of components to return from the query results.
        /// </param>
        /// <remarks>
        ///     At least one component ID must be provided. Query results are not filtered
        ///     if resultComponentIds is null or contains 0 elements.
        /// </remarks>
        /// <returns>
        ///     An updated InterestQuery object.
        /// </returns>
        public InterestQuery FilterResults(IEnumerable<uint> resultComponentIds)
        {
            if (IsEnumerableNullOrEmpty(resultComponentIds))
            {
                Debug.LogWarning("At least one InterestQuery must be provided to filter a queries results.");
                return this;
            }

            query.FullSnapshotResult = null;
            query.ResultComponentId.AddRange(resultComponentIds);
            return this;
        }

        /// <summary>
        ///     Returns the underlying ComponentInterest.Query object from the InterestQuery class.
        /// </summary>
        public ComponentInterest.Query AsComponentInterestQuery()
        {
            query.Constraint = queryConstraint.AsQueryConstraint();
            return query;
        }

        private bool IsEnumerableNullOrEmpty<T>(IEnumerable<T> data)
        {
            return data == null || !data.Any();
        }
    }
}
