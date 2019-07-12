using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;
using UnityEngine;

namespace Improbable.Gdk.QueryBasedInterest
{
    /// <summary>
    ///     Utility class to help construct Interest component snapshots.
    /// </summary>
    public class InterestTemplate
    {
        private readonly Dictionary<uint, ComponentInterest> interest;

        private InterestTemplate(Dictionary<uint, ComponentInterest> interest = null)
        {
            this.interest = interest ?? new Dictionary<uint, ComponentInterest>();
        }

        /// <summary>
        ///     Creates a new <see cref="InterestTemplate"/> object.
        /// </summary>
        /// <returns>
        ///     A new <see cref="InterestTemplate"/> object.
        /// </returns>
        public static InterestTemplate Create()
        {
            return new InterestTemplate();
        }

        /// <summary>
        ///     Creates a new <see cref="InterestTemplate"/> object given an existing <see cref="InterestTemplate"/>.
        /// </summary>
        /// <param name="interestTemplate">
        ///     An existing <see cref="InterestTemplate"/>.
        /// </param>
        /// <remarks>
        ///     The underlying data is deep copied.
        /// </remarks>
        /// <returns>
        ///     An <see cref="InterestTemplate"/> object.
        /// </returns>
        public static InterestTemplate Create(InterestTemplate interestTemplate)
        {
            return Create(interestTemplate.AsComponentInterest());
        }

        /// <summary>
        ///     Creates a new <see cref="InterestTemplate"/> object from the content of an existing Interest component.
        /// </summary>
        /// <param name="interest">
        ///     The underlying dictionary of an Interest component.
        /// </param>
        /// <remarks>
        ///     The underlying data is deep copied.
        /// </remarks>
        /// <returns>
        ///     An <see cref="InterestTemplate"/> object.
        /// </returns>
        public static InterestTemplate Create(Dictionary<uint, ComponentInterest> interest)
        {
            return new InterestTemplate(DeepCopy(interest));
        }

        private static Dictionary<uint, ComponentInterest> DeepCopy(Dictionary<uint, ComponentInterest> interest)
        {
            var clone = new Dictionary<uint, ComponentInterest>(interest.Count);
            foreach (var keyval in interest)
            {
                var componentInterestQueries = keyval.Value.Queries;
                var componentInterestClone = new List<ComponentInterest.Query>(componentInterestQueries.Count);
                foreach (var query in componentInterestQueries)
                {
                    var queryClone = query;
                    queryClone.ResultComponentId = new List<uint>(query.ResultComponentId);
                    componentInterestClone.Add(queryClone);
                }

                clone.Add(keyval.Key, new ComponentInterest { Queries = componentInterestClone });
            }

            return clone;
        }

        /// <summary>
        ///     Add InterestQueries to the Interest component.
        /// </summary>
        /// <param name="interestQuery">
        ///     First <see cref="InterestQuery"/> to add for a given authoritative component.
        /// </param>
        /// <param name="interestQueries">
        ///     Further InterestQueries to add for a given authoritative component.
        /// </param>
        /// <typeparam name="T">
        ///     Type of the authoritative component to add the InterestQueries to.
        /// </typeparam>
        /// <remarks>
        ///     At least one <see cref="InterestQuery"/> must be provided to update the Interest component.
        /// </remarks>
        /// <returns>
        ///     An <see cref="InterestTemplate"/> object.
        /// </returns>
        public InterestTemplate AddQueries<T>(InterestQuery interestQuery,
            params InterestQuery[] interestQueries)
            where T : ISpatialComponentData
        {
            return AddQueries(Dynamic.GetComponentId<T>(), interestQuery, interestQueries);
        }

        /// <summary>
        ///     Add InterestQueries to the Interest component.
        /// </summary>
        /// <param name="componentId">
        ///     Component ID of the authoritative component to add the InterestQueries to.
        /// </param>
        /// <param name="interestQuery">
        ///     First <see cref="InterestQuery"/> to add for a given authoritative component.
        /// </param>
        /// <param name="interestQueries">
        ///     Further InterestQueries to add for a given authoritative component.
        /// </param>
        /// <remarks>
        ///     At least one <see cref="InterestQuery"/> must be provided to update the Interest component.
        /// </remarks>
        /// <returns>
        ///     An <see cref="InterestTemplate"/> object.
        /// </returns>
        public InterestTemplate AddQueries(uint componentId,
            InterestQuery interestQuery,
            params InterestQuery[] interestQueries)
        {
            return AddQueries(componentId, InterestQueryParamsToQueries(interestQuery, interestQueries));
        }

        /// <summary>
        ///     Add InterestQueries to the Interest component.
        /// </summary>
        /// <param name="interestQueries">
        ///     Set of InterestQueries to add for a given authoritative component.
        /// </param>
        /// <typeparam name="T">
        ///     Type of the authoritative component to add the InterestQueries to.
        /// </typeparam>
        /// <remarks>
        ///     At least one <see cref="InterestQuery"/> must be provided to update the Interest component.
        /// </remarks>
        /// <returns>
        ///     An <see cref="InterestTemplate"/> object.
        /// </returns>
        public InterestTemplate AddQueries<T>(IEnumerable<InterestQuery> interestQueries)
            where T : ISpatialComponentData
        {
            return AddQueries(Dynamic.GetComponentId<T>(), interestQueries);
        }

        /// <summary>
        ///     Add InterestQueries to the Interest component.
        /// </summary>
        /// <param name="componentId">
        ///     Component ID of the authoritative component to add the InterestQueries to.
        /// </param>
        /// <param name="interestQueries">
        ///     Set of InterestQueries to add for a given authoritative component.
        /// </param>
        /// <remarks>
        ///     At least one <see cref="InterestQuery"/> must be provided to update the Interest component. No
        ///     queries are added if interestQueries is empty.
        /// </remarks>
        /// <returns>
        ///     An <see cref="InterestTemplate"/> object.
        /// </returns>
        public InterestTemplate AddQueries(uint componentId, IEnumerable<InterestQuery> interestQueries)
        {
            if (!interestQueries.Any())
            {
                Debug.LogWarning("At least one InterestQuery must be provided to add to a component's interest.");
                return this;
            }

            return AddQueries(componentId, InterestQueryEnumerableToQueries(interestQueries));
        }

        private InterestTemplate AddQueries(uint componentId, List<ComponentInterest.Query> componentInterestQueries)
        {
            if (!interest.TryGetValue(componentId, out var componentInterest))
            {
                interest.Add(componentId, new ComponentInterest
                {
                    Queries = componentInterestQueries
                });
                return this;
            }

            componentInterest.Queries.AddRange(componentInterestQueries);
            return this;
        }

        /// <summary>
        ///     Replaces a component's InterestQueries in the Interest component.
        /// </summary>
        /// <param name="interestQuery">
        ///     First <see cref="InterestQuery"/> to add for a given authoritative component.
        /// </param>
        /// <param name="interestQueries">
        ///     Further InterestQueries to add for a given authoritative component.
        /// </param>
        /// <typeparam name="T">
        ///     Type of the authoritative component to replace InterestQueries of.
        /// </typeparam>
        /// <remarks>
        ///     At least one <see cref="InterestQuery"/> must be provided to replace a component's interest.
        /// </remarks>
        /// <returns>
        ///     An <see cref="InterestTemplate"/> object.
        /// </returns>
        public InterestTemplate ReplaceQueries<T>(InterestQuery interestQuery,
            params InterestQuery[] interestQueries)
            where T : ISpatialComponentData
        {
            return ReplaceQueries(Dynamic.GetComponentId<T>(), interestQuery, interestQueries);
        }

        /// <summary>
        ///     Replaces a component's InterestQueries in the Interest component.
        /// </summary>
        /// <param name="componentId">
        ///     Component ID of the authoritative component to replace InterestQueries of.
        /// </param>
        /// <param name="interestQuery">
        ///     First <see cref="InterestQuery"/> to add for a given authoritative component.
        /// </param>
        /// <param name="interestQueries">
        ///     Further InterestQueries to add for a given authoritative component.
        /// </param>
        /// <remarks>
        ///     At least one <see cref="InterestQuery"/> must be provided to replace a component's interest.
        /// </remarks>
        /// <returns>
        ///     An <see cref="InterestTemplate"/> object.
        /// </returns>
        public InterestTemplate ReplaceQueries(uint componentId,
            InterestQuery interestQuery,
            params InterestQuery[] interestQueries)
        {
            return ReplaceQueries(componentId, InterestQueryParamsToQueries(interestQuery, interestQueries));
        }

        /// <summary>
        ///     Replaces a component's InterestQueries in the Interest component.
        /// </summary>
        /// <param name="interestQueries">
        ///     Set of InterestQueries to add for a given authoritative component.
        /// </param>
        /// <typeparam name="T">
        ///     Type of the authoritative component to replace InterestQueries of.
        /// </typeparam>
        /// <remarks>
        ///     At least one <see cref="InterestQuery"/> must be provided to replace a component's interest.
        /// </remarks>
        /// <returns>
        ///     An <see cref="InterestTemplate"/> object.
        /// </returns>
        public InterestTemplate ReplaceQueries<T>(IEnumerable<InterestQuery> interestQueries)
            where T : ISpatialComponentData
        {
            return ReplaceQueries(Dynamic.GetComponentId<T>(), interestQueries);
        }

        /// <summary>
        ///     Replaces a component's InterestQueries in the Interest component.
        /// </summary>
        /// <param name="componentId">
        ///     Component ID of the authoritative component to replace InterestQueries of.
        /// </param>
        /// <param name="interestQueries">
        ///     Set of InterestQueries to add for a given authoritative component.
        /// </param>
        /// <remarks>
        ///     At least one <see cref="InterestQuery"/> must be provided to replace a component's interest. No
        ///     queries are replaced if interestQueries is empty.
        /// </remarks>
        /// <returns>
        ///     An <see cref="InterestTemplate"/> object.
        /// </returns>
        public InterestTemplate ReplaceQueries(uint componentId, IEnumerable<InterestQuery> interestQueries)
        {
            if (!interestQueries.Any())
            {
                Debug.LogWarning("At least one InterestQuery must be provided to replace a component's interest.");
                return this;
            }

            return ReplaceQueries(componentId, InterestQueryEnumerableToQueries(interestQueries));
        }

        private InterestTemplate ReplaceQueries(uint componentId,
            List<ComponentInterest.Query> componentInterestQueries)
        {
            if (!interest.TryGetValue(componentId, out var componentInterest))
            {
                interest.Add(componentId, new ComponentInterest
                {
                    Queries = componentInterestQueries
                });
                return this;
            }

            componentInterest.Queries.Clear();
            componentInterest.Queries.AddRange(componentInterestQueries);
            return this;
        }

        /// <summary>
        ///     Clears all InterestQueries for a given authoritative component.
        /// </summary>
        /// <typeparam name="T">
        ///     Type of the authoritative component to clear InterestQueries from.
        /// </typeparam>
        /// <returns>
        ///     An <see cref="InterestTemplate"/> object.
        /// </returns>
        public InterestTemplate ClearQueries<T>()
            where T : ISpatialComponentData
        {
            return ClearQueries(Dynamic.GetComponentId<T>());
        }

        /// <summary>
        ///     Clears all InterestQueries for a given authoritative component.
        /// </summary>
        /// <param name="componentId">
        ///     Component ID of the authoritative component to clear InterestQueries from.
        /// </param>
        /// <returns>
        ///     An <see cref="InterestTemplate"/> object.
        /// </returns>
        public InterestTemplate ClearQueries(uint componentId)
        {
            interest.Remove(componentId);
            return this;
        }

        /// <summary>
        ///     Clears all InterestQueries.
        /// </summary>
        /// <returns>
        ///     An <see cref="InterestTemplate"/> object.
        /// </returns>
        public InterestTemplate ClearAllQueries()
        {
            interest.Clear();
            return this;
        }

        /// <summary>
        ///     Builds the Interest snapshot.
        /// </summary>
        /// <returns>
        ///     A Interest.Snapshot object.
        /// </returns>
        public Interest.Snapshot ToSnapshot()
        {
            return new Interest.Snapshot { ComponentInterest = interest };
        }

        /// <summary>
        ///     Returns the underlying data of an Interest component.
        /// </summary>
        /// <returns>
        ///     A Dictionary<uint, ComponentInterest>.
        /// </returns>
        public Dictionary<uint, ComponentInterest> AsComponentInterest()
        {
            return interest;
        }

        private static List<ComponentInterest.Query> InterestQueryParamsToQueries(
            InterestQuery interestQuery,
            params InterestQuery[] interestQueries)
        {
            var output = new List<ComponentInterest.Query>(interestQueries.Length + 1)
            {
                interestQuery.AsComponentInterestQuery()
            };

            foreach (var constraintParam in interestQueries)
            {
                output.Add(constraintParam.AsComponentInterestQuery());
            }

            return output;
        }

        private static List<ComponentInterest.Query> InterestQueryEnumerableToQueries(
            IEnumerable<InterestQuery> interestQueries)
        {
            return interestQueries.Select(interestQuery => interestQuery.AsComponentInterestQuery()).ToList();
        }
    }
}
