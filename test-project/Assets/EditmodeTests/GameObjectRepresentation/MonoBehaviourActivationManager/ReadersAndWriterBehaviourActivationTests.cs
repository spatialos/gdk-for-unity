using Improbable.Gdk.GameObjectRepresentation;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.EditmodeTests.GameObjectRepresentation.MonoBehaviourActivationManagerTests
{
    [TestFixture]
    public class ReadersAndWriterBehaviourActivationTests :
        AuthorityRequiredBehaviourActivationTestsBase<
            ReadersAndWriterBehaviourActivationTests.TestBehaviourWithReaderAndWriter>
    {
        protected override uint ComponentId => new Position.Component().ComponentId;

        public class TestBehaviourWithReaderAndWriter : MonoBehaviour
        {
            [Require] public Position.Requirable.Reader PositionReader;
            [Require] public Position.Requirable.Writer PositionWriter;
        }

        protected override void ValidateRequirablesNotNull()
        {
            var testBehaviour = TestGameObject.GetComponent<TestBehaviourWithReaderAndWriter>();
            Assert.IsNotNull(testBehaviour.PositionReader);
            Assert.IsNotNull(testBehaviour.PositionReader);
        }
    }
}
