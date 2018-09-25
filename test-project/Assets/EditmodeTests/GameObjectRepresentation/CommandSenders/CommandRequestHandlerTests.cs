using System.Collections.Generic;
using Improbable.Gdk.Tests.ComponentsWithNoFields;
using NUnit.Framework;
using Unity.Entities;

namespace Improbable.Gdk.EditmodeTests.GameObjectRepresentation.CommandSenders
{
    [TestFixture]
    public class CommandRequestHandlerTests
    {
        [Test]
        public void SendResponse_queues_responses()
        {
            using (var world = new World("test-world"))
            {
                var entityManager = world.GetOrCreateManager<EntityManager>();
                var entity = entityManager.CreateEntity();

                var commandResponder = new ComponentWithNoFieldsWithCommands.CommandResponders.Cmd();

                commandResponder.CommandListHandle =
                    ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdResponderProvider.Allocate(world);
                commandResponder.ResponsesToSend = new List<ComponentWithNoFieldsWithCommands.Cmd.Response>();

                entityManager.AddComponentData(entity, commandResponder);

                var receivedRequest = new ComponentWithNoFieldsWithCommands.Cmd.ReceivedRequest();

                var cmdRequestResponder =
                    new ComponentWithNoFieldsWithCommands.Cmd.RequestResponder(entityManager, entity, receivedRequest);

                cmdRequestResponder.SendResponse(new Empty());

                var componentData =
                    entityManager.GetComponentData<ComponentWithNoFieldsWithCommands.CommandResponders.Cmd>(entity);

                Assert.IsNotEmpty(componentData.ResponsesToSend);

                ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdResponderProvider.Free(commandResponder
                    .CommandListHandle);
            }
        }
    }
}
