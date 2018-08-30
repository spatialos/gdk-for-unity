using System.Collections.Generic;
using Generated.Improbable.Gdk.Tests.ComponentsWithNoFields;
using NUnit.Framework;
using Unity.Entities;

namespace Improbable.Gdk.Core.EditmodeTests.MonoBehaviours.CommandSenders
{
    [TestFixture]
    public class CommandRequestHandlerTests
    {
        [Test]
        public void SendCmdResponse_queues_responses()
        {
            using (var world = new World("test-world"))
            {
                var entityManager = world.GetOrCreateManager<EntityManager>();
                var entity = entityManager.CreateEntity();

                var commandResponder = new ComponentWithNoFieldsWithCommands.CommandResponders.Cmd();

                commandResponder.CommandListHandle = ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdResponderProvider.Allocate(world);
                commandResponder.ResponsesToSend = new List<ComponentWithNoFieldsWithCommands.Cmd.Response>();

                entityManager.AddComponentData(entity, commandResponder);

                var commandRequestHandler =
                    new ComponentWithNoFieldsWithCommands.Requirables.CommandRequestHandler(entity, entityManager,
                        new LoggingDispatcher());

                commandRequestHandler.SendCmdResponse(ComponentWithNoFieldsWithCommands
                    .Cmd
                    .CreateResponse(
                        new ComponentWithNoFieldsWithCommands.Cmd.ReceivedRequest(),
                        new Empty()));

                var componentData = entityManager.GetComponentData<ComponentWithNoFieldsWithCommands.CommandResponders.Cmd>(entity);
                Assert.IsNotEmpty(componentData.ResponsesToSend);

                ComponentWithNoFieldsWithCommands.ReferenceTypeProviders.CmdResponderProvider.Free(commandResponder.CommandListHandle);
            }
        }
    }
}
