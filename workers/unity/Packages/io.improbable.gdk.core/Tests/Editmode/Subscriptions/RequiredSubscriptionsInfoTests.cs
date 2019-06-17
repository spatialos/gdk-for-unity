using Improbable.Gdk.Subscriptions;
using NUnit.Framework;
using Unity.Entities;

namespace Improbable.Gdk.Core.EditmodeTests.Subscriptions
{
    /// <summary>
    ///     This tests the correctness of the reflection code that grabs information about Required fields on a class.
    /// </summary>
    [TestFixture]
    public class RequiredSubscriptionsInfoTests
    {
        private class RequiredFields
        {
#pragma warning disable 649
            [Require] public World World;
            [Require] protected EntityId EntityId;
            [Require] internal ILogDispatcher LogDispatcher;
            [Require] private Entity entity;
#pragma warning restore 649
        }

        [Test]
        public void RequiredSubscriptionsInfo_finds_all_public_and_private_fields()
        {
            var info = new RequiredSubscriptionsInfo(typeof(RequiredFields));
            Assert.AreEqual(4, info.RequiredTypes.Length);

            Assert.Contains(typeof(World), info.RequiredTypes);
            Assert.Contains(typeof(EntityId), info.RequiredTypes);
            Assert.Contains(typeof(ILogDispatcher), info.RequiredTypes);
            Assert.Contains(typeof(Entity), info.RequiredTypes);
        }

        private class RequiredStaticFields
        {
#pragma warning disable 649
            [Require] public static World World;
            [Require] private static Entity entity;
#pragma warning restore 649
        }

        [Test]
        public void RequiredSubscriptionsInfo_ignores_static_fields()
        {
            var info = new RequiredSubscriptionsInfo(typeof(RequiredStaticFields));
            Assert.AreEqual(0, info.RequiredTypes.Length);
        }
    }
}
