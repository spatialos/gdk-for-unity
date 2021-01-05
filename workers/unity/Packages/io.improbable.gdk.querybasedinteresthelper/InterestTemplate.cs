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
        private readonly Dictionary<uint, ComponentSetInterest> interest;

        private InterestTemplate(Dictionary<uint, ComponentSetInterest> interest = null)
        {
            this.interest = interest ?? new Dictionary<uint, ComponentSetInterest>();
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
            return Create(interestTemplate.AsComponentSetInterest());
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
        public static InterestTemplate Create(Dictionary<uint, ComponentSetInterest> interest)
        {
            return new InterestTemplate(DeepCopy(interest));
        }

        private static Dictionary<uint, ComponentSetInterest> DeepCopy(Dictionary<uint, ComponentSetInterest> interest)
        {
            var clone = new Dictionary<uint, ComponentSetInterest>(interest.Count);
            foreach (var keyval in interest)
            {
                var componentInterestQueries = keyval.Value.Queries;
                var componentInterestClone = new List<ComponentSetInterest.Query>(componentInterestQueries.Count);
                foreach (var query in componentInterestQueries)
                {
                    var queryClone = query;
                    queryClone.ResultComponentId = new List<uint>(query.ResultComponentId);
                    componentInterestClone.Add(queryClone);
                }

                clone.Add(keyval.Key, new ComponentSetInterest { Queries = componentInterestClone });
            }

            return clone;
        }

        /// <summary>
        ///     Add InterestQueries to the Interest component.
        /// </summary>
        /// <param name="componentSet">
        ///     Component Set ID of the authoritative component set to add the InterestQueries to.
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
        public InterestTemplate AddQueries(ComponentSet componentSet,
            InterestQuery interestQuery,
            params InterestQuery[] interestQueries)
        {
            return AddQueries(componentSet, InterestQueryParamsToQueries(interestQuery, interestQueries));
        }


        /// <summary>
        ///     Add InterestQueries to the Interest component.
        /// </summary>
        /// <param name="componentSet">
        ///     Component Set ID of the authoritative component set to add the InterestQueries to.
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
        public InterestTemplate AddQueries(ComponentSet componentSet, IEnumerable<InterestQuery> interestQueries)
        {
            if (!interestQueries.Any())
            {
                UnityEngine.Debug.LogWarning("At least one InterestQuery must be provided to add to a component's interest.");
                return this;
            }

            return AddQueries(componentSet, InterestQueryEnumerableToQueries(interestQueries));
        }

        private InterestTemplate AddQueries(ComponentSet componentSet, List<ComponentSetInterest.Query> componentInterestQueries)
        {
            if (!interest.TryGetValue(componentSet.ComponentSetId, out var componentInterest))
            {
                interest.Add(componentSet.ComponentSetId, new ComponentSetInterest
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
        public InterestTemplate ReplaceQueries(ComponentSet componentSet,
            InterestQuery interestQuery,
            params InterestQuery[] interestQueries)
        {
            return ReplaceQueries(componentSet, InterestQueryParamsToQueries(interestQuery, interestQueries));
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
        public InterestTemplate ReplaceQueries(ComponentSet componentSet, IEnumerable<InterestQuery> interestQueries)
        {
            if (!interestQueries.Any())
            {
                UnityEngine.Debug.LogWarning("At least one InterestQuery must be provided to replace a component's interest.");
                return this;
            }

            return ReplaceQueries(componentSet, InterestQueryEnumerableToQueries(interestQueries));
        }

        private InterestTemplate ReplaceQueries(ComponentSet componentSet,
            List<ComponentSetInterest.Query> componentInterestQueries)
        {
            if (!interest.TryGetValue(componentSet.ComponentSetId, out var componentInterest))
            {
                interest.Add(componentSet.ComponentSetId, new ComponentSetInterest
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
        /// <param name="componentId">
        ///     Component ID of the authoritative component to clear InterestQueries from.
        /// </param>
        /// <returns>
        ///     An <see cref="InterestTemplate"/> object.
        /// </returns>
        public InterestTemplate ClearQueries(ComponentSet componentSet)
        {
            interest.Remove(componentSet.ComponentSetId);
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
            return new Interest.Snapshot { ComponentSetInterest = interest };
        }

        /// <summary>
        ///     Returns the underlying data of an Interest component.
        /// </summary>
        /// <returns>
        ///     A Dictionary<uint, ComponentSetInterest>.
        /// </returns>
        public Dictionary<uint, ComponentSetInterest> AsComponentSetInterest()
        {
            return interest;
        }

        private static List<ComponentSetInterest.Query> InterestQueryParamsToQueries(
            InterestQuery interestQuery,
            params InterestQuery[] interestQueries)
        {
            var output = new List<ComponentSetInterest.Query>(interestQueries.Length + 1)
            {
                interestQuery.AsComponentSetInterestQuery()
            };

            foreach (var constraintParam in interestQueries)
            {
                output.Add(constraintParam.AsComponentSetInterestQuery());
            }

            return output;
        }

        private static List<ComponentSetInterest.Query> InterestQueryEnumerableToQueries(
            IEnumerable<InterestQuery> interestQueries)
        {
            return interestQueries.Select(interestQuery => interestQuery.AsComponentSetInterestQuery()).ToList();
        }
    }
}
