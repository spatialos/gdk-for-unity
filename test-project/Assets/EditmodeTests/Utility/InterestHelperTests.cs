using System.Collections.Generic;
using Improbable.Gdk.QueryBasedInterest;
using Improbable.Worker.CInterop;
using NUnit.Framework;
using Unity.Entities;

namespace Improbable.Gdk.EditmodeTests.Utility
{
    [TestFixture]
    public class InterestHelperTests
    {
        private static ComponentInterest.Query BasicQuery
            => InterestQuery.Query(Constraint.RelativeSphere(10));

        private static ComponentInterest.Query DifferentBasicQuery
            => InterestQuery.Query(Constraint.RelativeSphere(20));

        private static InterestHelper BasicInterest
            => InterestHelper.Begin();

        private static InterestHelper FromInterest
            => InterestHelper.From(EmptyDictionary);

        private static InterestHelper MutateInterest
            => InterestHelper.Mutate(EmptyDictionary);

        private static Dictionary<uint, ComponentInterest> EmptyDictionary
            => new Dictionary<uint, ComponentInterest>();

        [Test]
        public void AddQueries_can_be_called_multiple_times_on_same_component()
        {
            Assert.DoesNotThrow(() => BasicInterest
                .AddQueries<Position.Component>(BasicQuery, BasicQuery)
                .AddQueries<Position.Component>(BasicQuery, BasicQuery));
        }

        [Test]
        public void AddQueries_can_be_called_after_AddQueries()
        {
            Assert.DoesNotThrow(() => BasicInterest
                .AddQueries<Position.Component>(BasicQuery, BasicQuery)
                .AddQueries<Metadata.Component>(BasicQuery, BasicQuery));
        }

        [Test]
        public void AddQueries_can_be_called_with_a_single_query()
        {
            Assert.DoesNotThrow(() => BasicInterest
                .AddQueries<Position.Component>(BasicQuery));
        }

        [Test]
        public void ReplaceQueries_clears_previous_query()
        {
            var initialQuery = BasicQuery;
            var initialQueryRadius = initialQuery.Constraint.RelativeSphereConstraint.Value.Radius;

            var differentBasicQuery = DifferentBasicQuery;
            var differentQueryRadius = DifferentBasicQuery.Constraint.RelativeSphereConstraint.Value.Radius;

            var interest = BasicInterest
                .AddQueries<Position.Component>(initialQuery)
                .ReplaceQueries<Position.Component>(differentBasicQuery);

            ComponentInterest replacedQuery;
            var queryExists = interest.ToComponentInterest().TryGetValue(Position.ComponentId, out replacedQuery);
            var replacedQueryRadius = replacedQuery.Queries[0].Constraint.RelativeSphereConstraint.Value.Radius;

            Assert.True(queryExists);
            Assert.AreEqual(1, replacedQuery.Queries.Count);
            Assert.AreNotEqual(initialQueryRadius, replacedQueryRadius);
            Assert.AreEqual(differentQueryRadius, replacedQueryRadius);
        }

        [Test]
        public void ReplaceQueries_clears_previous_queries()
        {
            var interest = BasicInterest
                .AddQueries<Position.Component>(BasicQuery, BasicQuery, BasicQuery)
                .ReplaceQueries<Position.Component>(DifferentBasicQuery);

            ComponentInterest replacedQuery;
            var queryExists = interest.ToComponentInterest().TryGetValue(Position.ComponentId, out replacedQuery);

            Assert.True(queryExists);
            Assert.AreEqual(1, replacedQuery.Queries.Count);
        }

        [Test]
        public void ClearQueries_clears_a_single_list_of_queries()
        {
            var interest = BasicInterest
                .AddQueries<Metadata.Component>(BasicQuery)
                .AddQueries<Persistence.Component>(BasicQuery)
                .AddQueries<Position.Component>(BasicQuery)
                .ClearQueries<Metadata.Component>();

            Assert.AreEqual(2, interest.ToComponentInterest().Keys.Count);
        }

        [Test]
        public void ClearQueries_can_be_used_with_empty_lists()
        {
            Assert.DoesNotThrow(() => BasicInterest
                .ClearQueries<Metadata.Component>());
        }

        [Test]
        public void ClearAllQueries_clears_entire_dictionary()
        {
            var interest = BasicInterest
                .AddQueries<Metadata.Component>(BasicQuery)
                .AddQueries<Persistence.Component>(BasicQuery)
                .AddQueries<Position.Component>(BasicQuery)
                .ClearAllQueries();

            Assert.AreEqual(0, interest.ToComponentInterest().Keys.Count);
        }

        [Test]
        public void ClearAllQueries_can_be_used_with_empty_dictionary()
        {
            Assert.DoesNotThrow(() => BasicInterest
                .ClearAllQueries());
        }

        [Test]
        public void GetInterest_should_not_return_null_with_new_interest()
        {
            var interest = BasicInterest.ToComponentInterest();
            Assert.IsNotNull(interest);
        }

        [Test]
        public void GetInterest_should_not_return_null_with_from_interest()
        {
            var interest = FromInterest.ToComponentInterest();
            Assert.IsNotNull(interest);
        }

        [Test]
        public void GetInterest_should_not_return_null_with_mutated_interest()
        {
            var interest = MutateInterest.ToComponentInterest();
            Assert.IsNotNull(interest);
        }

        [Test]
        public void From_dictionary_modifies_a_new_dictionary()
        {
            var originalInterest = BasicInterest
                .AddQueries<Position.Component>(BasicQuery)
                .AddQueries<Metadata.Component>(BasicQuery)
                .ToComponentInterest();

            var modifiedInterest = InterestHelper
                .From(originalInterest)
                .ClearQueries<Position.Component>()
                .ToComponentInterest();

            Assert.AreEqual(2, originalInterest.Keys.Count);
            Assert.AreEqual(1, modifiedInterest.Keys.Count);
        }

        [Test]
        public void Mutate_dictionary_changes_given_dictionary()
        {
            var originalInterest = BasicInterest
                .AddQueries<Position.Component>(BasicQuery)
                .AddQueries<Metadata.Component>(BasicQuery)
                .ToComponentInterest();

            var modifiedInterest = InterestHelper
                .Mutate(originalInterest)
                .ClearQueries<Position.Component>()
                .ToComponentInterest();

            Assert.AreEqual(1, originalInterest.Keys.Count);
            Assert.AreEqual(1, modifiedInterest.Keys.Count);
        }

        [Test]
        public void Snapshot_should_not_contain_null_component_interest()
        {
            var interest = BasicInterest.ToSnapshot();
            Assert.IsNotNull(interest.ComponentInterest);
        }
    }
}
