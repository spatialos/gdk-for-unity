using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.QueryBasedInterest
{
    /// <summary>
    ///     Utility class to help construct Interest component snapshots.
    /// </summary>
    public class InterestHelper
    {
        private readonly Dictionary<uint, ComponentInterest> interest;

        private InterestHelper(Dictionary<uint, ComponentInterest> interest)
        {
            this.interest = interest ?? new Dictionary<uint, ComponentInterest>();
        }

        /// <summary>
        ///     Creates a new InterestHelper object.
        /// </summary>
        /// <returns>
        ///     A new InterestHelper object.
        /// </returns>
        public static InterestHelper Begin()
        {
            return new InterestHelper(null);
        }

        /// <summary>
        ///     Creates a new InterestHelper object given an existing Interest component.
        /// </summary>
        /// <param name="interest">
        ///     An existing Interest component.
        /// </param>
        /// <returns>
        ///     An InterestHelper object.
        /// </returns>
        public static InterestHelper From(Interest.Component interest)
        {
            return new InterestHelper(interest.ComponentInterest
                .ToDictionary(entry => entry.Key, entry => entry.Value));
        }

        /// <summary>
        ///     Creates a new InterestHelper object from the content of an existing Interest component.
        /// </summary>
        /// <param name="interest">
        ///     The underlying dictionary of an Interest component.
        /// </param>
        /// <returns>
        ///     An InterestHelper object.
        /// </returns>
        public static InterestHelper From(Dictionary<uint, ComponentInterest> interest)
        {
            return new InterestHelper(interest.ToDictionary(entry => entry.Key, entry => entry.Value));
        }

        /// <summary>
        ///     Mutates the given interest component.
        /// </summary>
        /// <param name="interest">
        ///     An existing Interest component.
        /// </param>
        /// <returns>
        ///     An InterestHelper object.
        /// </returns>
        public static InterestHelper Mutate(Interest.Component interest)
        {
            return new InterestHelper(interest.ComponentInterest);
        }

        /// <summary>
        ///     Mutates the underlying dictionary of an interest component.
        /// </summary>
        /// <param name="interest">
        ///     The underlying dictionary of an Interest component.
        /// </param>
        /// <returns>
        ///     An InterestHelper object.
        /// </returns>
        public static InterestHelper Mutate(Dictionary<uint, ComponentInterest> interest)
        {
            return new InterestHelper(interest);
        }

        /// <summary>
        ///     Add queries to the Interest component.
        /// </summary>
        /// <param name="query">
        ///     First query to add for a given authoritative component.
        /// </param>
        /// <param name="queries">
        ///     Further queries to add for a given authoritative component.
        /// </param>
        /// <typeparam name="T">
        ///     Type of the authoritative component to add the queries to.
        /// </typeparam>
        /// <remarks>
        ///     At least one query must be provided to update the Interest component.
        /// </remarks>
        /// <returns>
        ///     An InterestHelper object.
        /// </returns>
        public InterestHelper AddQueries<T>(ComponentInterest.Query query,
            params ComponentInterest.Query[] queries)
            where T : ISpatialComponentData
        {
            var interestQueries = new List<ComponentInterest.Query>(queries.Length + 1) { query };
            interestQueries.AddRange(queries);

            var componentId = Dynamic.GetComponentId<T>();
            if (!interest.ContainsKey(componentId))
            {
                interest.Add(componentId, new ComponentInterest
                {
                    Queries = new List<ComponentInterest.Query>(interestQueries)
                });
                return this;
            }

            interest[componentId].Queries.AddRange(interestQueries);
            return this;
        }

        /// <summary>
        ///     Replaces a component's queries in the Interest component.
        /// </summary>
        /// <param name="query">
        ///     First query to add for a given authoritative component.
        /// </param>
        /// <param name="queries">
        ///     Further queries to add for a given authoritative component.
        /// </param>
        /// <typeparam name="T">
        ///     Type of the authoritative component to replace queries of.
        /// </typeparam>
        /// <remarks>
        ///     At least one query must be provided to replace a component's interest.
        /// </remarks>
        /// <returns>
        ///     An InterestHelper object.
        /// </returns>
        public InterestHelper ReplaceQueries<T>(ComponentInterest.Query query,
            params ComponentInterest.Query[] queries)
            where T : ISpatialComponentData
        {
            var interestQueries = new List<ComponentInterest.Query>(queries.Length + 1) { query };
            interestQueries.AddRange(queries);

            var componentId = Dynamic.GetComponentId<T>();
            if (!interest.ContainsKey(componentId))
            {
                interest.Add(componentId, new ComponentInterest
                {
                    Queries = new List<ComponentInterest.Query>(interestQueries)
                });
                return this;
            }

            interest[componentId].Queries.Clear();
            interest[componentId].Queries.AddRange(interestQueries);
            return this;
        }

        /// <summary>
        ///     Add queries to the Interest component.
        /// </summary>
        /// <typeparam name="T">
        ///     Type of the authoritative component to clear queries from.
        /// </typeparam>
        /// <remarks>
        ///     At least one query must be provided to update the Interest component.
        /// </remarks>
        /// <returns>
        ///     An InterestHelper object.
        /// </returns>
        public InterestHelper ClearQueries<T>()
            where T : ISpatialComponentData
        {
            var componentId = Dynamic.GetComponentId<T>();
            interest.Remove(componentId);
            return this;
        }

        /// <summary>
        ///     Removes all queries.
        /// </summary>
        /// <returns>
        ///     An InterestHelper object.
        /// </returns>
        public InterestHelper ClearAllQueries()
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
        public Dictionary<uint, ComponentInterest> ToComponentInterest()
        {
            return interest;
        }
    }
}
