using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Gdk.QueryBasedInterest
{
    /// <summary>
    ///     Utility class to help construct ComponentSetInterest.Query objects.
    /// </summary>
    public class InterestQuery
    {
        private ComponentSetInterest.Query query;

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
                    ResultComponentId = new List<uint>(),
                    ResultComponentSetId = new List<uint>()
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
        ///     A frequency of 0 means there will be no rate limiting.
        /// </remarks>
        /// <returns>
        ///     An updated <see cref="InterestQuery"/> object.
        /// </returns>
        public InterestQuery WithMaxFrequencyHz(float frequencyHz)
        {
            if (frequencyHz < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(frequencyHz), frequencyHz, "The max frequency must be greater than or equal to zero.");
            }

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
                UnityEngine.Debug.LogWarning("At least one component ID must be provided to filter a query's results.");
                return this;
            }

            query.FullSnapshotResult = null;
            query.ResultComponentId.AddRange(resultComponentIds);
            return this;
        }

        /// <summary>
        ///     Defines what components to return in the query results.
        /// </summary>
        /// <param name="resultComponentSet">
        ///     First component set to return from the query results.
        /// </param>
        /// <param name="resultComponentSets">
        ///     Further component sets to return from the query results.
        /// </param>
        /// <returns>
        ///     An updated <see cref="InterestQuery"/> object.
        /// </returns>
        public InterestQuery FilterResults(ComponentSet resultComponentSet, params ComponentSet[] resultComponentSets)
        {
            var componentSetResultIds = new List<ComponentSet>(resultComponentSets.Length + 1) { resultComponentSet };
            componentSetResultIds.AddRange(resultComponentSets);

            return FilterResults(componentSetResultIds);
        }

        /// <summary>
        ///     Defines what components to return in the query results.
        /// </summary>
        /// <param name="resultComponentSets">
        ///     Set of component sets to return from the query results.
        /// </param>
        /// <remarks>
        ///     At least one component set must be provided. Query results are not filtered
        ///     if resultComponentSets is empty.
        /// </remarks>
        /// <returns>
        ///     An updated <see cref="InterestQuery"/> object.
        /// </returns>
        public InterestQuery FilterResults(IEnumerable<ComponentSet> resultComponentSets)
        {
            if (!resultComponentSets.Any())
            {
                UnityEngine.Debug.LogWarning("At least one component set must be provided to filter a query's results.");
                return this;
            }

            query.FullSnapshotResult = null;
            query.ResultComponentSetId.AddRange(resultComponentSets.Select(set => set.ComponentSetId));
            return this;
        }

        /// <summary>
        ///     Returns the underlying ComponentSetInterest.Query object from the <see cref="InterestQuery"/> class.
        /// </summary>
        public ComponentSetInterest.Query AsComponentSetInterestQuery()
        {
            return query;
        }
    }
}
