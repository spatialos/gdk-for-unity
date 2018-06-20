using System.Linq;
using Generated.Improbable.PlayerLifecycle;
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.TestUtils;
using NUnit.Framework;
using Unity.Entities;
using UnityEngine;

[TestFixture]
public class HandleCreatePlayerRequestSystemTests : HybridGdkSystemTestBase
{
    private UnityTestWorker worker;

    [SetUp]
    public void Setup()
    {
        worker = new UnityTestWorker("worker-id", new Vector3());
    }

    [TearDown]
    public void TearDown()
    {
        worker.Dispose();
    }

    [Test]
    public void
        HandleCreatePlayerRequestSystem_should_call_CreatePlayerEntityTemplate_when_it_handles_a_CreatePlayer_request()
    {
        var world = worker.World;

        var entityManager = world.GetOrCreateManager<EntityManager>();

        var handleCreatePlayerRequestSystem = world.GetOrCreateManager<HandleCreatePlayerRequestSystem>();

        CreateEntityWithCreatePlayerRequest(entityManager);

        var createPlayerEntityTemplateCalled = false;

        GetPlayerEntityTemplateDelegate previousCreatePlayerEntityTemplate =
            PlayerLifecycleConfig.CreatePlayerEntityTemplate;

        try
        {
            PlayerLifecycleConfig.CreatePlayerEntityTemplate = (set, position) =>
            {
                createPlayerEntityTemplateCalled = true;
                return new Improbable.Worker.Entity();
            };

            Assert.IsFalse(createPlayerEntityTemplateCalled);

            handleCreatePlayerRequestSystem.Update();

            Assert.IsTrue(createPlayerEntityTemplateCalled);
        }
        finally
        {
            PlayerLifecycleConfig.CreatePlayerEntityTemplate = previousCreatePlayerEntityTemplate;
        }
    }

    private void CreateEntityWithCreatePlayerRequest(EntityManager entityManager)
    {
        var entity = entityManager.CreateEntity();

        worker.View.TranslationUnits.Values.First(translation => translation is WorldCommandsTranslation)
            .AddCommandRequestSender(entity, 1);

        var commandRequests = new CommandRequests<PlayerCreator.CreatePlayer.Request>();
        commandRequests.Buffer.Add(new PlayerCreator.CreatePlayer.Request());

        worker.View.SetComponentObject(entity, commandRequests);
    }
}
