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
        private ComponentInterest.Query query;

        private InterestQuery()
        {
        }

        /// <summary>
        ///     Creates an <see cref="InterestQuery"/>.
        /// </summary>
        /// <param name="constraint">
        ///     A <see cref="Constraint"/> object defining the constraints of the query.
        /// </param>
        /// <remarks>
        ///     Returns the full snapshot result by default.
        /// </remarks>
        /// <returns>
        ///     An <see cref="InterestQuery"/> object.
        /// </returns>
        public static InterestQuery Query(Constraint constraint)
        {
            var interest = new InterestQuery
            {
                query =
                {
                    Constraint = constraint.AsQueryConstraint(),
                    FullSnapshotResult = true,
                    ResultComponentId = new List<uint>()
                }
            };
            return interest;
        }

        /// <summary>
        ///     Sets the maximum frequency (Hz) of the query.
        /// </summary>
        /// <param name="frequencyHz">
        ///     The maximum frequency (Hz) to return query results.
        /// </param>
        /// <remarks>
        ///     Although the `Frequency` of a ComponentInterest.Query can be set, the runtime will ignore this field
        ///     for now as the feature has not yet been rolled out. Until then, this method will remain private.
        /// </remarks>
        /// <returns>
        ///     An updated <see cref="InterestQuery"/> object.
        /// </returns>
        private InterestQuery WithMaxFrequencyHz(float frequencyHz)
        {
            query.Frequency = frequencyHz;
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
        ///     An updated <see cref="InterestQuery"/> object.
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
        ///     if resultComponentIds is empty.
        /// </remarks>
        /// <returns>
        ///     An updated <see cref="InterestQuery"/> object.
        /// </returns>
        public InterestQuery FilterResults(IEnumerable<uint> resultComponentIds)
        {
            if (!resultComponentIds.Any())
            {
                Debug.LogWarning("At least one InterestQuery must be provided to filter a query's results.");
                return this;
            }

            query.FullSnapshotResult = null;
            query.ResultComponentId.AddRange(resultComponentIds);
            return this;
        }

        /// <summary>
        ///     Returns the underlying ComponentInterest.Query object from the <see cref="InterestQuery"/> class.
        /// </summary>
        public ComponentInterest.Query AsComponentInterestQuery()
        {
            return query;
        }
    }
}
