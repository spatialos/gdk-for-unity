using System.Collections.Generic;
using Improbable.Gdk.QueryBasedInterest;
using NUnit.Framework;

namespace Improbable.Gdk.EditmodeTests.Utility
{
    [TestFixture]
    public class InterestBuilderTests
    {
        private static ComponentInterest.Query BasicQuery
            => InterestQuery.Query(Constraint.RelativeSphere(10));

        private static ComponentInterest.Query DifferentBasicQuery
            => InterestQuery.Query(Constraint.RelativeSphere(20));

        private static InterestBuilder BasicInterest
            => InterestBuilder.Begin();

        private static InterestBuilder ModifyInterest
            => InterestBuilder.Modify(new Dictionary<uint, ComponentInterest>());

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
            var queryExists = interest.GetInterest().TryGetValue(Position.ComponentId, out replacedQuery);
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
            var queryExists = interest.GetInterest().TryGetValue(Position.ComponentId, out replacedQuery);

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

            Assert.AreEqual(2, interest.GetInterest().Keys.Count);
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

            Assert.AreEqual(0, interest.GetInterest().Keys.Count);
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
            var interest = BasicInterest.GetInterest();
            Assert.IsNotNull(interest);
        }

        [Test]
        public void GetInterest_should_not_return_null_with_modified_interest()
        {
            var interest = ModifyInterest.GetInterest();
            Assert.IsNotNull(interest);
        }

        [Test]
        public void Snapshot_should_not_contain_null_component_interest()
        {
            var interest = BasicInterest.ToSnapshot();
            Assert.IsNotNull(interest.ComponentInterest);
        }
    }
}
