using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;

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
        ///     Creates a new InterestTemplate object.
        /// </summary>
        /// <returns>
        ///     A new InterestTemplate object.
        /// </returns>
        public static InterestTemplate New()
        {
            return new InterestTemplate();
        }

        /// <summary>
        ///     Creates a new InterestTemplate object given an existing Interest component.
        /// </summary>
        /// <param name="interest">
        ///     An existing Interest component.
        /// </param>
        /// <returns>
        ///     An InterestTemplate object.
        /// </returns>
        public static InterestTemplate From(Interest.Component interest)
        {
            return From(interest.ComponentInterest);
        }

        /// <summary>
        ///     Creates a new InterestTemplate object from the content of an existing Interest component.
        /// </summary>
        /// <param name="interest">
        ///     The underlying dictionary of an Interest component.
        /// </param>
        /// <returns>
        ///     An InterestTemplate object.
        /// </returns>
        public static InterestTemplate From(Dictionary<uint, ComponentInterest> interest)
        {
            return new InterestTemplate(DeepClone(interest));
        }

        private static Dictionary<uint, ComponentInterest> DeepClone(Dictionary<uint, ComponentInterest> interest)
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
        ///     Mutates the given interest component.
        /// </summary>
        /// <param name="interest">
        ///     An existing Interest component.
        /// </param>
        /// <returns>
        ///     An InterestTemplate object.
        /// </returns>
        public static InterestTemplate Mutate(Interest.Component interest)
        {
            return Mutate(interest.ComponentInterest);
        }

        /// <summary>
        ///     Mutates the underlying dictionary of an interest component.
        /// </summary>
        /// <param name="interest">
        ///     The underlying dictionary of an Interest component.
        /// </param>
        /// <returns>
        ///     An InterestTemplate object.
        /// </returns>
        public static InterestTemplate Mutate(Dictionary<uint, ComponentInterest> interest)
        {
            return new InterestTemplate(interest);
        }

        /// <summary>
        ///     Add interestQueries to the Interest component.
        /// </summary>
        /// <param name="query">
        ///     First interestQuery to add for a given authoritative component.
        /// </param>
        /// <param name="queries">
        ///     Further interestQueries to add for a given authoritative component.
        /// </param>
        /// <typeparam name="T">
        ///     Type of the authoritative component to add the interestQueries to.
        /// </typeparam>
        /// <remarks>
        ///     At least one interestQuery must be provided to update the Interest component.
        /// </remarks>
        /// <returns>
        ///     An InterestTemplate object.
        /// </returns>
        public InterestTemplate AddQueries<T>(InterestQuery query,
            params InterestQuery[] queries)
            where T : ISpatialComponentData
        {
            return AddQueries(Dynamic.GetComponentId<T>(), query, queries);
        }

        /// <summary>
        ///     Add interestQueries to the Interest component.
        /// </summary>
        /// <param name="componentId">
        ///     Component ID of the authoritative component to add the interestQueries to.
        /// </param>
        /// <param name="interestQuery">
        ///     First interestQuery to add for a given authoritative component.
        /// </param>
        /// <param name="interestQueries">
        ///     Further interestQueries to add for a given authoritative component.
        /// </param>
        /// <remarks>
        ///     At least one interestQuery must be provided to update the Interest component.
        /// </remarks>
        /// <returns>
        ///     An InterestTemplate object.
        /// </returns>
        public InterestTemplate AddQueries(uint componentId,
            InterestQuery interestQuery,
            params InterestQuery[] interestQueries)
        {
            var componentInterestQueries = QueryParamsToList(interestQuery, interestQueries);

            if (!interest.TryGetValue(componentId, out var componentInterest))
            {
                interest.Add(componentId, new ComponentInterest
                {
                    Queries = new List<ComponentInterest.Query>(componentInterestQueries)
                });
                return this;
            }

            componentInterest.Queries.AddRange(componentInterestQueries);
            return this;
        }

        /// <summary>
        ///     Replaces a component's interestQueries in the Interest component.
        /// </summary>
        /// <param name="query">
        ///     First interestQuery to add for a given authoritative component.
        /// </param>
        /// <param name="queries">
        ///     Further interestQueries to add for a given authoritative component.
        /// </param>
        /// <typeparam name="T">
        ///     Type of the authoritative component to replace interestQueries of.
        /// </typeparam>
        /// <remarks>
        ///     At least one interestQuery must be provided to replace a component's interest.
        /// </remarks>
        /// <returns>
        ///     An InterestTemplate object.
        /// </returns>
        public InterestTemplate ReplaceQueries<T>(InterestQuery query,
            params InterestQuery[] queries)
            where T : ISpatialComponentData
        {
            return ReplaceQueries(Dynamic.GetComponentId<T>(), query, queries);
        }

        /// <summary>
        ///     Replaces a component's interestQueries in the Interest component.
        /// </summary>
        /// <param name="componentId">
        ///     Component ID of the authoritative component to replace interestQueries of.
        /// </param>
        /// <param name="interestQuery">
        ///     First interestQuery to add for a given authoritative component.
        /// </param>
        /// <param name="queries">
        ///     Further interestQueries to add for a given authoritative component.
        /// </param>
        /// <remarks>
        ///     At least one interestQuery must be provided to replace a component's interest.
        /// </remarks>
        /// <returns>
        ///     An InterestTemplate object.
        /// </returns>
        public InterestTemplate ReplaceQueries(uint componentId,
            InterestQuery interestQuery,
            params InterestQuery[] interestQueries)
        {
            var componentInterestQueries = QueryParamsToList(interestQuery, interestQueries);

            if (!interest.TryGetValue(componentId, out var componentInterest))
            {
                interest.Add(componentId, new ComponentInterest
                {
                    Queries = new List<ComponentInterest.Query>(componentInterestQueries)
                });
                return this;
            }

            componentInterest.Queries.Clear();
            componentInterest.Queries.AddRange(componentInterestQueries);
            return this;
        }

        /// <summary>
        ///     Add interestQueries to the Interest component.
        /// </summary>
        /// <typeparam name="T">
        ///     Type of the authoritative component to clear interestQueries from.
        /// </typeparam>
        /// <returns>
        ///     An InterestTemplate object.
        /// </returns>
        public InterestTemplate ClearQueries<T>()
            where T : ISpatialComponentData
        {
            return ClearQueries(Dynamic.GetComponentId<T>());
        }

        /// <summary>
        ///     Add interestQueries to the Interest component.
        /// </summary>
        /// <param name="componentId">
        ///     Component ID of the authoritative component to clear interestQueries from.
        /// </param>
        /// <returns>
        ///     An InterestTemplate object.
        /// </returns>
        public InterestTemplate ClearQueries(uint componentId)
        {
            interest.Remove(componentId);
            return this;
        }

        /// <summary>
        ///     Removes all interestQueries.
        /// </summary>
        /// <returns>
        ///     An InterestTemplate object.
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
        ///     Returns a deep copy of the underlying data of an Interest component.
        /// </summary>
        /// <returns>
        ///     A Dictionary<uint, ComponentInterest>.
        /// </returns>
        public Dictionary<uint, ComponentInterest> ToComponentInterest()
        {
            return DeepClone(interest);
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

        private static List<ComponentInterest.Query> QueryParamsToList(InterestQuery interestQuery,
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
    }
}
