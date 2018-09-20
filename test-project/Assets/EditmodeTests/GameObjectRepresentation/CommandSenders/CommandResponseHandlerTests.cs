using Improbable.Gdk.Core;
using Improbable.Gdk.Tests.ComponentsWithNoFields;
using Improbable.Worker;
using Improbable.Worker.Core;
using NUnit.Framework;
using Unity.Entities;

namespace Improbable.Gdk.EditmodeTests.GameObjectRepresentation.CommandSenders
{
    [TestFixture]
    public class CommandResponseHandlerTests
    {
        [Test]
        public void OnCmdResponseInternal_calls_OnCmdResponse_delegates()
        {
            using (var world = new World("test-world"))
            {
                var entityManager = world.GetOrCreateManager<EntityManager>();
                var entity = entityManager.CreateEntity();

                var commandResponseHandler =
                    new ComponentWithNoFieldsWithCommands.Requirable.CommandResponseHandler(entity, entityManager,
                        new LoggingDispatcher());

                var responseCallbackCalled = false;

                commandResponseHandler.OnCmdResponse += response => { responseCallbackCalled = true; };

                commandResponseHandler.OnCmdResponseInternal(
                    new ComponentWithNoFieldsWithCommands.Cmd.ReceivedResponse(
                        new EntityId(0),
                        string.Empty,
                        StatusCode.Success,
                        new Empty(),
                        new Empty(),
                        null,
                        0
                    ));

                Assert.IsTrue(responseCallbackCalled);
            }
        }
    }
}
