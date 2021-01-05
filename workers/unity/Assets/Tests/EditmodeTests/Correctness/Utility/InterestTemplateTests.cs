using System.Collections.Generic;
using Improbable.Gdk.QueryBasedInterest;
using Improbable.Generated;
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

        private static Dictionary<uint, ComponentSetInterest> EmptyDictionary
            => new Dictionary<uint, ComponentSetInterest>();

        [Test]
        public void AddQueries_can_be_called_multiple_times_on_same_component()
        {
            Assert.DoesNotThrow(() => EmptyInterest
                .AddQueries(ComponentSets.AuthorityDelegationSet, BasicQuery, BasicQuery)
                .AddQueries(ComponentSets.AuthorityDelegationSet, BasicQuery, BasicQuery));
        }

        [Test]
        public void AddQueries_can_be_called_multiple_times_on_different_components()
        {
            Assert.DoesNotThrow(() => EmptyInterest
                .AddQueries(ComponentSets.AuthorityDelegationSet, BasicQuery, BasicQuery)
                .AddQueries(ComponentSets.WellKnownComponentSet, BasicQuery, BasicQuery));
        }

        [Test]
        public void AddQueries_can_be_called_with_a_single_query()
        {
            Assert.DoesNotThrow(() => EmptyInterest
                .AddQueries(ComponentSets.AuthorityDelegationSet, BasicQuery));
        }

        [Test]
        public void AddQueries_can_be_called_multiple_times_on_same_component_with_enumerable()
        {
            Assert.DoesNotThrow(() => EmptyInterest
                .AddQueries(ComponentSets.AuthorityDelegationSet, new List<InterestQuery> { BasicQuery, BasicQuery })
                .AddQueries(ComponentSets.AuthorityDelegationSet, new List<InterestQuery> { BasicQuery, BasicQuery }));
        }

        [Test]
        public void AddQueries_can_be_called_multiple_times_on_different_components_with_enumerable()
        {
            Assert.DoesNotThrow(() => EmptyInterest
                .AddQueries(ComponentSets.AuthorityDelegationSet, new List<InterestQuery> { BasicQuery, BasicQuery })
                .AddQueries(ComponentSets.WellKnownComponentSet, new List<InterestQuery> { BasicQuery, BasicQuery }));
        }

        [Test]
        public void AddQueries_can_be_called_with_a_single_query_with_enumerable()
        {
            Assert.DoesNotThrow(() => EmptyInterest
                .AddQueries(ComponentSets.AuthorityDelegationSet, new List<InterestQuery> { BasicQuery }));
        }

        [Test]
        public void AddQueries_does_nothing_with_empty_enumerable()
        {
            Assert.False(EmptyInterest
                .AddQueries(ComponentSets.AuthorityDelegationSet, new List<InterestQuery>())
                .AsComponentSetInterest()
                .ContainsKey(Position.ComponentId));
        }

        [Test]
        public void AddQueries_does_not_throw_exception_with_empty_enumerable()
        {
            Assert.DoesNotThrow(() => EmptyInterest
                .AddQueries(ComponentSets.AuthorityDelegationSet, new List<InterestQuery>()));
        }

        [Test]
        public void ReplaceQueries_clears_previous_query_for_a_component()
        {
            var initialQuery = BasicQuery;
            var initialQueryRadius = initialQuery.AsComponentSetInterestQuery()
                .Constraint.RelativeSphereConstraint.Value.Radius;

            var differentBasicQuery = DifferentBasicQuery;
            var differentQueryRadius = differentBasicQuery.AsComponentSetInterestQuery()
                .Constraint.RelativeSphereConstraint.Value.Radius;

            var interest = EmptyInterest
                .AddQueries(ComponentSets.AuthorityDelegationSet, initialQuery)
                .ReplaceQueries(ComponentSets.AuthorityDelegationSet, differentBasicQuery);

            var queryExists = interest.AsComponentSetInterest().TryGetValue(ComponentSets.AuthorityDelegationSet.ComponentSetId, out var replacedQuery);
            var replacedQueryRadius = replacedQuery.Queries[0].Constraint.RelativeSphereConstraint.Value.Radius;

            Assert.True(queryExists);
            Assert.AreEqual(1, replacedQuery.Queries.Count);
            Assert.AreNotEqual(initialQueryRadius, replacedQueryRadius);
            Assert.AreEqual(differentQueryRadius, replacedQueryRadius);
        }

        [Test]
        public void ReplaceQueries_clears_previous_queries_for_a_component()
        {
            var interest = EmptyInterest
                .AddQueries(ComponentSets.AuthorityDelegationSet, BasicQuery, BasicQuery, BasicQuery)
                .ReplaceQueries(ComponentSets.AuthorityDelegationSet, DifferentBasicQuery);

            var queryExists = interest.AsComponentSetInterest().TryGetValue(ComponentSets.AuthorityDelegationSet.ComponentSetId, out var replacedQuery);

            Assert.True(queryExists);
            Assert.AreEqual(1, replacedQuery.Queries.Count);
        }

        [Test]
        public void ReplaceQueries_clears_previous_query_for_a_component_with_enumerable()
        {
            var initialQuery = BasicQuery;
            var initialQueryRadius = initialQuery.AsComponentSetInterestQuery()
                .Constraint.RelativeSphereConstraint.Value.Radius;

            var differentBasicQuery = DifferentBasicQuery;
            var differentQueryRadius = differentBasicQuery.AsComponentSetInterestQuery()
                .Constraint.RelativeSphereConstraint.Value.Radius;

            var interest = EmptyInterest
                .AddQueries(ComponentSets.AuthorityDelegationSet, initialQuery)
                .ReplaceQueries(ComponentSets.AuthorityDelegationSet, new List<InterestQuery> { differentBasicQuery });

            var queryExists = interest.AsComponentSetInterest().TryGetValue(ComponentSets.AuthorityDelegationSet.ComponentSetId, out var replacedQuery);
            var replacedQueryRadius = replacedQuery.Queries[0].Constraint.RelativeSphereConstraint.Value.Radius;

            Assert.True(queryExists);
            Assert.AreEqual(1, replacedQuery.Queries.Count);
            Assert.AreNotEqual(initialQueryRadius, replacedQueryRadius);
            Assert.AreEqual(differentQueryRadius, replacedQueryRadius);
        }

        [Test]
        public void ReplaceQueries_clears_previous_queries_for_a_component_with_enumerable()
        {
            var interest = EmptyInterest
                .AddQueries(ComponentSets.AuthorityDelegationSet, BasicQuery, BasicQuery, BasicQuery)
                .ReplaceQueries(ComponentSets.AuthorityDelegationSet, new List<InterestQuery> { DifferentBasicQuery });

            var queryExists = interest.AsComponentSetInterest().TryGetValue(ComponentSets.AuthorityDelegationSet.ComponentSetId, out var replacedQuery);

            Assert.True(queryExists);
            Assert.AreEqual(1, replacedQuery.Queries.Count);
        }

        [Test]
        public void ReplaceQueries_does_nothing_with_empty_enumerable()
        {
            var basicQuery = BasicQuery;
            var interest = EmptyInterest
                .AddQueries(ComponentSets.AuthorityDelegationSet, basicQuery)
                .ReplaceQueries(ComponentSets.AuthorityDelegationSet, new List<InterestQuery>());

            Assert.True(interest.AsComponentSetInterest().TryGetValue(ComponentSets.AuthorityDelegationSet.ComponentSetId, out var queryValue));
            Assert.AreEqual(queryValue.Queries[0].Constraint.RelativeSphereConstraint.Value.Radius,
                basicQuery.AsComponentSetInterestQuery().Constraint.RelativeSphereConstraint.Value.Radius);
        }

        [Test]
        public void ReplaceQueries_does_not_throw_exception_with_empty_enumerable()
        {
            Assert.DoesNotThrow(() => EmptyInterest
                .AddQueries(ComponentSets.AuthorityDelegationSet, BasicQuery)
                .ReplaceQueries(ComponentSets.AuthorityDelegationSet, new List<InterestQuery>()));
        }

        [Test]
        public void ClearQueries_clears_queries_for_a_single_component()
        {
            var interest = EmptyInterest
                .AddQueries(ComponentSets.WellKnownComponentSet, BasicQuery)
                .AddQueries(ComponentSets.DependentComponentSet, BasicQuery)
                .AddQueries(ComponentSets.AuthorityDelegationSet, BasicQuery)
                .ClearQueries(ComponentSets.WellKnownComponentSet);

            Assert.AreEqual(2, interest.AsComponentSetInterest().Keys.Count);
            Assert.True(interest.AsComponentSetInterest().ContainsKey(ComponentSets.DependentComponentSet.ComponentSetId));
            Assert.True(interest.AsComponentSetInterest().ContainsKey(ComponentSets.AuthorityDelegationSet.ComponentSetId));
        }

        [Test]
        public void ClearQueries_can_be_used_with_empty_interest()
        {
            Assert.DoesNotThrow(() => EmptyInterest
                .ClearQueries(ComponentSets.WellKnownComponentSet));
        }

        [Test]
        public void ClearAllQueries_clears_queries_for_all_components()
        {
            var interest = EmptyInterest
                .AddQueries(ComponentSets.WellKnownComponentSet, BasicQuery)
                .AddQueries(ComponentSets.DependentComponentSet, BasicQuery)
                .AddQueries(ComponentSets.AuthorityDelegationSet, BasicQuery)
                .ClearAllQueries();

            Assert.AreEqual(0, interest.AsComponentSetInterest().Keys.Count);
        }

        [Test]
        public void ClearAllQueries_can_be_used_with_empty_interest()
        {
            Assert.DoesNotThrow(() => EmptyInterest
                .ClearAllQueries());
        }

        [Test]
        public void AsComponentSetInterest_should_not_return_null_after_creation()
        {
            var interest = EmptyInterest.AsComponentSetInterest();
            Assert.IsNotNull(interest);
        }

        [Test]
        public void AsComponentSetInterest_should_not_return_null_after_creation_from_template()
        {
            var interest = EmptyInterestFromTemplate.AsComponentSetInterest();
            Assert.IsNotNull(interest);
        }

        [Test]
        public void AsComponentSetInterest_should_not_return_null_after_creation_from_dictionary()
        {
            var interest = EmptyInterestFromDictionary.AsComponentSetInterest();
            Assert.IsNotNull(interest);
        }

        [Test]
        public void Create_from_template_modifies_a_deep_copy()
        {
            var originalInterest = EmptyInterest
                .AddQueries(ComponentSets.AuthorityDelegationSet, BasicQuery)
                .AddQueries(ComponentSets.WellKnownComponentSet, BasicQuery);

            var modifiedInterest = InterestTemplate
                .Create(originalInterest)
                .ClearQueries(ComponentSets.AuthorityDelegationSet)
                .AsComponentSetInterest();

            Assert.AreEqual(2, originalInterest.AsComponentSetInterest().Keys.Count);
            Assert.AreEqual(1, modifiedInterest.Keys.Count);
        }

        [Test]
        public void Create_from_dictionary_modifies_a_deep_copy()
        {
            var originalInterest = EmptyInterest
                .AddQueries(ComponentSets.AuthorityDelegationSet, BasicQuery)
                .AddQueries(ComponentSets.WellKnownComponentSet, BasicQuery)
                .AsComponentSetInterest();

            var modifiedInterest = InterestTemplate
                .Create(originalInterest)
                .ClearQueries(ComponentSets.AuthorityDelegationSet)
                .AsComponentSetInterest();

            Assert.AreEqual(2, originalInterest.Keys.Count);
            Assert.AreEqual(1, modifiedInterest.Keys.Count);
        }

        [Test]
        public void ToSnapshot_with_empty_interest_should_not_contain_null_component_interest()
        {
            var interest = EmptyInterest.ToSnapshot();
            Assert.IsNotNull(interest.ComponentSetInterest);
        }
    }
}
