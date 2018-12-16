using Improbable.Gdk.QueryBasedInterest;
using NUnit.Framework;

namespace Improbable.Gdk.EditmodeTests.Utility
{
    [TestFixture]
    public class InterestBuilderTests
    {
        private static ComponentInterest.Query BasicQuery => InterestQuery.Query(Constraint.RelativeSphere(10));

        private static InterestBuilder BasicInterest => InterestBuilder.Begin();

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

        //add tests
        //replacequeries

        //clearqueries

        //clearallqueries

        [Test]
        public void GetInterest_should_not_return_null_interest()
        {
            var interest = BasicInterest.GetInterest();
            Assert.IsNotNull(interest);
        }

        [Test]
        public void Snapshot_should_not_contain_null_component_interest()
        {
            var interest = BasicInterest.Snapshot();
            Assert.IsNotNull(interest.ComponentInterest);
        }
    }
}
