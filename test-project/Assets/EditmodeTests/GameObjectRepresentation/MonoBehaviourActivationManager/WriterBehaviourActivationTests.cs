using Generated.Improbable;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.GameObjectRepresentation.EditModeTests.MonoBehaviourActivationManagerTests
{
    [TestFixture]
    public class WriterBehaviourActivationTests :
        AuthorityRequiredBehaviourActivationTestsBase<WriterBehaviourActivationTests.TestBehaviourWithWriter>
    {
        protected override uint ComponentId => new Position.Component().ComponentId;

        public class TestBehaviourWithWriter : MonoBehaviour
        {
            [Require] private Position.Requirable.Writer positionWriter;
        }
    }
}
