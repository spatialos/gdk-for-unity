using System.Collections.Generic;
using Improbable.Gdk.QueryBasedInterest;
using NUnit.Framework;

namespace Improbable.Gdk.EditmodeTests.Utility
{
    [TestFixture]
    public class InterestTemplateTests
    {
        private static InterestQuery BasicQuery
            => InterestQuery.Query(Constraint.RelativeSphere(10));

        private static InterestQuery DifferentBasicQuery
            => InterestQuery.Query(Constraint.RelativeSphere(20));

        private static InterestTemplate EmptyInterest
            => InterestTemplate.Create();

        private static InterestTemplate EmptyInterestFromTemplate
            => InterestTemplate.Create(EmptyInterest);

        private static InterestTemplate EmptyInterestFromDictionary
            => InterestTemplate.Create(EmptyDictionary);

        private static Dictionary<uint, ComponentInterest> EmptyDictionary
            => new Dictionary<uint, ComponentInterest>();

        [Test]
        public void AddQueries_can_be_called_multiple_times_on_same_component()
        {
            Assert.DoesNotThrow(() => EmptyInterest
                .AddQueries<Position.Component>(BasicQuery, BasicQuery)
                .AddQueries<Position.Component>(BasicQuery, BasicQuery));
        }

        [Test]
        public void AddQueries_can_be_called_after_AddQueries()
        {
            Assert.DoesNotThrow(() => EmptyInterest
                .AddQueries<Position.Component>(BasicQuery, BasicQuery)
                .AddQueries<Metadata.Component>(BasicQuery, BasicQuery));
        }

        [Test]
        public void AddQueries_can_be_called_with_a_single_query()
        {
            Assert.DoesNotThrow(() => EmptyInterest
                .AddQueries<Position.Component>(BasicQuery));
        }

        [Test]
        public void ReplaceQueries_clears_previous_query()
        {
            var initialQuery = BasicQuery;
            var initialQueryRadius = initialQuery.AsComponentInterestQuery()
                .Constraint.RelativeSphereConstraint.Value.Radius;

            var differentBasicQuery = DifferentBasicQuery;
            var differentQueryRadius = differentBasicQuery.AsComponentInterestQuery()
                .Constraint.RelativeSphereConstraint.Value.Radius;

            var interest = EmptyInterest
                .AddQueries<Position.Component>(initialQuery)
                .ReplaceQueries<Position.Component>(differentBasicQuery);

            var queryExists = interest.AsComponentInterest().TryGetValue(Position.ComponentId, out var replacedQuery);
            var replacedQueryRadius = replacedQuery.Queries[0].Constraint.RelativeSphereConstraint.Value.Radius;

            Assert.True(queryExists);
            Assert.AreEqual(1, replacedQuery.Queries.Count);
            Assert.AreNotEqual(initialQueryRadius, replacedQueryRadius);
            Assert.AreEqual(differentQueryRadius, replacedQueryRadius);
        }

        [Test]
        public void ReplaceQueries_clears_previous_queries()
        {
            var interest = EmptyInterest
                .AddQueries<Position.Component>(BasicQuery, BasicQuery, BasicQuery)
                .ReplaceQueries<Position.Component>(DifferentBasicQuery);

            var queryExists = interest.AsComponentInterest().TryGetValue(Position.ComponentId, out var replacedQuery);

            Assert.True(queryExists);
            Assert.AreEqual(1, replacedQuery.Queries.Count);
        }

        [Test]
        public void ClearQueries_clears_a_single_list_of_queries()
        {
            var interest = EmptyInterest
                .AddQueries<Metadata.Component>(BasicQuery)
                .AddQueries<Persistence.Component>(BasicQuery)
                .AddQueries<Position.Component>(BasicQuery)
                .ClearQueries<Metadata.Component>();

            Assert.AreEqual(2, interest.AsComponentInterest().Keys.Count);
        }

        [Test]
        public void ClearQueries_can_be_used_with_empty_lists()
        {
            Assert.DoesNotThrow(() => EmptyInterest
                .ClearQueries<Metadata.Component>());
        }

        [Test]
        public void ClearAllQueries_clears_entire_dictionary()
        {
            var interest = EmptyInterest
                .AddQueries<Metadata.Component>(BasicQuery)
                .AddQueries<Persistence.Component>(BasicQuery)
                .AddQueries<Position.Component>(BasicQuery)
                .ClearAllQueries();

            Assert.AreEqual(0, interest.AsComponentInterest().Keys.Count);
        }

        [Test]
        public void ClearAllQueries_can_be_used_with_empty_dictionary()
        {
            Assert.DoesNotThrow(() => EmptyInterest
                .ClearAllQueries());
        }

        [Test]
        public void GetInterest_should_not_return_null_after_creation()
        {
            var interest = EmptyInterest.AsComponentInterest();
            Assert.IsNotNull(interest);
        }

        [Test]
        public void GetInterest_should_not_return_null_after_creation_from_template()
        {
            var interest = EmptyInterestFromTemplate.AsComponentInterest();
            Assert.IsNotNull(interest);
        }

        [Test]
        public void GetInterest_should_not_return_null_after_creation_from_dictionary()
        {
            var interest = EmptyInterestFromDictionary.AsComponentInterest();
            Assert.IsNotNull(interest);
        }

        [Test]
        public void Create_from_template_modifies_a_deep_copy()
        {
            var originalInterest = EmptyInterest
                .AddQueries<Position.Component>(BasicQuery)
                .AddQueries<Metadata.Component>(BasicQuery);

            var modifiedInterest = InterestTemplate
                .Create(originalInterest)
                .ClearQueries<Position.Component>()
                .AsComponentInterest();

            Assert.AreEqual(2, originalInterest.AsComponentInterest().Keys.Count);
            Assert.AreEqual(1, modifiedInterest.Keys.Count);
        }

        [Test]
        public void Create_from_dictionary_modifies_a_deep_copy()
        {
            var originalInterest = EmptyInterest
                .AddQueries<Position.Component>(BasicQuery)
                .AddQueries<Metadata.Component>(BasicQuery)
                .AsComponentInterest();

            var modifiedInterest = InterestTemplate
                .Create(originalInterest)
                .ClearQueries<Position.Component>()
                .AsComponentInterest();

            Assert.AreEqual(2, originalInterest.Keys.Count);
            Assert.AreEqual(1, modifiedInterest.Keys.Count);
        }

        [Test]
        public void Snapshot_should_not_contain_null_component_interest()
        {
            var interest = EmptyInterest.ToSnapshot();
            Assert.IsNotNull(interest.ComponentInterest);
        }
    }
}
