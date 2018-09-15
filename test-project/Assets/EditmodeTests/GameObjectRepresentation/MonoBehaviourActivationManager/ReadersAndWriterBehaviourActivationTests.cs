using Generated.Improbable;
using NUnit.Framework;
using UnityEngine;

namespace Improbable.Gdk.GameObjectRepresentation.EditModeTests.MonoBehaviourActivationManagerTests
{
    [TestFixture]
    public class ReadersAndWriterBehaviourActivationTests :
        AuthorityRequiredBehaviourActivationTestsBase<ReadersAndWriterBehaviourActivationTests.TestBehaviourWithReaderAndWriter>
    {
        protected override uint ComponentId => new Position.Component().ComponentId;

        public class TestBehaviourWithReaderAndWriter : MonoBehaviour
        {
            [Require] private Position.Requirable.Reader positionReader;
            [Require] private Position.Requirable.Writer positionWriter;
        }
    }
}
