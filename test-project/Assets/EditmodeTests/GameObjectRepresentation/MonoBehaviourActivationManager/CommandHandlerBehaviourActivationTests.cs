using Improbable.Gdk.GameObjectRepresentation;
using Improbable.Gdk.Tests.ComponentsWithNoFields;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.GameObjectRepresentation.MonoBehaviourActivationManagerTests
{
    [TestFixture]
    public class CommandHandlerBehaviourActivationTests :
        AuthorityRequiredBehaviourActivationTestsBase<
            CommandHandlerBehaviourActivationTests.TestBehaviourWithCommandHandlers>
    {
        protected override uint ComponentId => new ComponentWithNoFieldsWithCommands.Component().ComponentId;

        public class TestBehaviourWithCommandHandlers : MonoBehaviour
        {
            [Require] public ComponentWithNoFieldsWithCommands.Requirable.CommandRequestHandler CommandRequestHandler;
        }

        protected override void ValidateRequirablesNotNull()
        {
            Assert.IsNotNull(TestGameObject.GetComponent<TestBehaviourWithCommandHandlers>().CommandRequestHandler);
        }
    }
}
