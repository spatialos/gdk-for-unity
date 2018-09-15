using Generated.Improbable.Gdk.Tests.ComponentsWithNoFields;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;

namespace Improbable.Gdk.GameObjectRepresentation.EditModeTests.MonoBehaviourActivationManagerTests
{
    [TestFixture]
    public class CommandHandlerBehaviourActivationTests :
        TestForBehaviourThatNeedsAuthority<CommandHandlerBehaviourActivationTests.TestBehaviourWithCommandHandlers>
    {
        protected override uint ComponentId => new ComponentWithNoFieldsWithCommands.Component().ComponentId;

        public class TestBehaviourWithCommandHandlers : MonoBehaviour
        {
            [Require] private ComponentWithNoFieldsWithCommands.Requirable.CommandRequestHandler commandRequestHandler;
        }
    }
}
