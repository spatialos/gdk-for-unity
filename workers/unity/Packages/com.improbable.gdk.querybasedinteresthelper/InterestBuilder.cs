using System.Collections.Generic;
using Improbable.Gdk.Core;

namespace Improbable.Gdk.QueryBasedInterest
{

    /// <summary>
    ///     Utility class to help construct Interest component snapshots.
    /// </summary>
    public class InterestBuilder
    {
        private readonly Dictionary<uint, ComponentInterest> interest;

        private InterestBuilder(Dictionary<uint, ComponentInterest> interest)
        {
            this.interest = interest ?? new Dictionary<uint, ComponentInterest>();
        }

        /// <summary>
        ///     Creates a new InterestBuilder object.
        /// </summary>
        /// <returns>
        ///     A new InterestBuilder object.
        /// </returns>
        public static InterestBuilder Begin()
        {
            return new InterestBuilder(null);
        }

        /// <summary>
        ///     Creates a new InterestBuilder object from the data of an existing Interest component.
        /// </summary>
        /// <param name="interest">
        ///     The data of an existing Interest component.
        /// </param>
        /// <returns>
        ///     A new InterestBuilder object.
        /// </returns>
        public static InterestBuilder Begin(Dictionary<uint, ComponentInterest> interest)
        {
            return new InterestBuilder(interest);
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
        ///     An InterestBuilder object.
        /// </returns>
        public InterestBuilder AddQueries<T>(ComponentInterest.Query query,
            params ComponentInterest.Query[] queries)
            where T : ISpatialComponentData
        {
            var interestQueries = new List<ComponentInterest.Query>(queries.Length + 1) {query};
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
        ///     An InterestBuilder object.
        /// </returns>
        public InterestBuilder ReplaceQueries<T>(ComponentInterest.Query query,
            params ComponentInterest.Query[] queries)
            where T : ISpatialComponentData
        {
            var interestQueries = new List<ComponentInterest.Query>(queries.Length + 1) {query};
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
        ///     An InterestBuilder object.
        /// </returns>
        public InterestBuilder ClearQueries<T>()
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
        ///     An InterestBuilder object.
        /// </returns>
        public InterestBuilder ClearAllQueries()
        {
            interest.Clear();
            return this;
        }

        /// <summary>
        ///     Returns the underlying data of an Interest component.
        /// </summary>
        /// <returns>
        ///     A Dictionary mapping a component ID to its component interest.
        /// </returns>
        public Dictionary<uint, ComponentInterest> GetInterest()
        {
            return interest;
        }

        /// <summary>
        ///     Builds the Interest snapshot.
        /// </summary>
        /// <returns>
        ///     A Interest.Snapshot object.
        /// </returns>
        public Interest.Snapshot Snapshot()
        {
            return new Interest.Snapshot { ComponentInterest = interest };
        }
    }
}
