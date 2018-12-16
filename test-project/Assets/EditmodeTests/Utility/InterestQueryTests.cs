using Improbable.Gdk.QueryBasedInterest;
using NUnit.Framework;

namespace Improbable.Gdk.EditmodeTests.Utility
{
    [TestFixture]
    public class InterestQueryTests
    {
        private static InterestQuery BasicQuery => InterestQuery.Query(Constraint.Component(Position.ComponentId));

        [Test]
        public void Query_sets_FullSnapshotResult_to_true()
        {
            ComponentInterest.Query query = BasicQuery;
            Assert.True(query.FullSnapshotResult);
        }

        [Test]
        public void Query_sets_ResultComponentId_to_empty_list()
        {
            ComponentInterest.Query query = BasicQuery;
            Assert.True(query.ResultComponentId.Count == 0);
        }

        [Test]
        public void Frequency_null_if_MaxFrequencyHz_never_called()
        {
            ComponentInterest.Query query = BasicQuery;
            Assert.IsNull(query.Frequency);
        }

        [Test]
        public void Filter_with_no_components_does_nothing()
        {
            var query = BasicQuery.FilterResults();
            Assert.True(query.ResultComponentId.Count == 0);
        }

        [Test]
        public void Filter_with_components_implies_FullSnapshotResult_is_null()
        {
            var query = BasicQuery.FilterResults(Position.ComponentId);
            Assert.IsNull(query.FullSnapshotResult);
        }
    }
}
