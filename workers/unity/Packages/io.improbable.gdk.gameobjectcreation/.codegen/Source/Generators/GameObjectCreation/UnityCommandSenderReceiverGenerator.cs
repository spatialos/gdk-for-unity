using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.CodeWriter.Scopes;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public static class UnityCommandSenderReceiverGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static string Generate(UnityComponentDetails details, string package)
        {
            var componentNamespace = $"global::{package}.{details.ComponentName}";

            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "System",
                    "System.Collections.Generic",
                    "Unity.Entities",
                    "Unity.Collections",
                    "Improbable.Gdk.Core",
                    "Improbable.Gdk.Subscriptions",
                    "Improbable.Worker.CInterop",
                    "Entity = Unity.Entities.Entity"
                );

                cgw.Namespace(package, ns =>
                {
                    ns.Type(GenerateCommandSenderSubscriptionManager(details, package));
                    ns.Type(GenerateCommandReceiverSubscriptionManager(details, package));
                    ns.Type(GenerateCommandSender(details, package, componentNamespace));
                    ns.Type(GenerateCommandReceiver(details, package, componentNamespace));
                });
            }).Format();
        }

        private static TypeBlock GenerateCommandSenderSubscriptionManager(UnityComponentDetails componentDetails, string qualifiedNamespace)
        {
            Logger.Trace($"Generating {qualifiedNamespace}.{componentDetails.ComponentName}CommandSenderSubscriptionManager class.");

            return Scope.AnnotatedType("AutoRegisterSubscriptionManager",
                $"public class {componentDetails.ComponentName}CommandSenderSubscriptionManager : SubscriptionManager<{componentDetails.ComponentName}CommandSender>",
                t =>
                {
                    t.Line($@"
private readonly World world;
private readonly WorkerSystem workerSystem;

private Dictionary<EntityId, HashSet<Subscription<{componentDetails.ComponentName}CommandSender>>>
    entityIdToSenderSubscriptions =
        new Dictionary<EntityId, HashSet<Subscription<{componentDetails.ComponentName}CommandSender>>>();

public {componentDetails.ComponentName}CommandSenderSubscriptionManager(World world)
{{
    this.world = world;

    // Check that these are there
    workerSystem = world.GetExistingSystem<WorkerSystem>();

    var constraintSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

    constraintSystem.RegisterEntityAddedCallback(entityId =>
    {{
        if (!entityIdToSenderSubscriptions.TryGetValue(entityId, out var subscriptions))
        {{
            return;
        }}

        workerSystem.TryGetEntity(entityId, out var entity);
        foreach (var subscription in subscriptions)
        {{
            if (!subscription.HasValue)
            {{
                subscription.SetAvailable(new {componentDetails.ComponentName}CommandSender(entity, world));
            }}
        }}
    }});

    constraintSystem.RegisterEntityRemovedCallback(entityId =>
    {{
        if (!entityIdToSenderSubscriptions.TryGetValue(entityId, out var subscriptions))
        {{
            return;
        }}

        foreach (var subscription in subscriptions)
        {{
            if (subscription.HasValue)
            {{
                ResetValue(subscription);
                subscription.SetUnavailable();
            }}
        }}
    }});
}}

public override Subscription<{componentDetails.ComponentName}CommandSender> Subscribe(EntityId entityId)
{{
    if (entityIdToSenderSubscriptions == null)
    {{
        entityIdToSenderSubscriptions = new Dictionary<EntityId, HashSet<Subscription<{componentDetails.ComponentName}CommandSender>>>();
    }}

    if (entityId.Id < 0)
    {{
        throw new ArgumentException(""EntityId can not be < 0"");
    }}

    var subscription = new Subscription<{componentDetails.ComponentName}CommandSender>(this, entityId);

    if (!entityIdToSenderSubscriptions.TryGetValue(entityId, out var subscriptions))
    {{
        subscriptions = new HashSet<Subscription<{componentDetails.ComponentName}CommandSender>>();
        entityIdToSenderSubscriptions.Add(entityId, subscriptions);
    }}

    if (workerSystem.TryGetEntity(entityId, out var entity))
    {{
        subscription.SetAvailable(new {componentDetails.ComponentName}CommandSender(entity, world));
    }}
    else if (entityId.Id == 0)
    {{
        subscription.SetAvailable(new {componentDetails.ComponentName}CommandSender(Entity.Null, world));
    }}

    subscriptions.Add(subscription);
    return subscription;
}}

public override void Cancel(ISubscription subscription)
{{
    var sub = ((Subscription<{componentDetails.ComponentName}CommandSender>) subscription);
    if (sub.HasValue)
    {{
        var sender = sub.Value;
        sender.IsValid = false;
    }}

    var subscriptions = entityIdToSenderSubscriptions[sub.EntityId];
    subscriptions.Remove(sub);
    if (subscriptions.Count == 0)
    {{
        entityIdToSenderSubscriptions.Remove(sub.EntityId);
    }}
}}

public override void ResetValue(ISubscription subscription)
{{
    var sub = ((Subscription<{componentDetails.ComponentName}CommandSender>) subscription);
    if (sub.HasValue)
    {{
        sub.Value.RemoveAllCallbacks();
    }}
}}
");
                });
        }

        private static TypeBlock GenerateCommandReceiverSubscriptionManager(UnityComponentDetails componentDetails, string qualifiedNamespace)
        {
            Logger.Trace($"Generating {qualifiedNamespace}.{componentDetails.ComponentName}CommandReceiverSubscriptionManager class.");

            return Scope.AnnotatedType("AutoRegisterSubscriptionManager",
                $"public class {componentDetails.ComponentName}CommandReceiverSubscriptionManager : SubscriptionManager<{componentDetails.ComponentName}CommandReceiver>",
                t =>
                {
                    t.Line($@"
private readonly World world;
private readonly WorkerSystem workerSystem;
private readonly ComponentUpdateSystem componentUpdateSystem;

private Dictionary<EntityId, HashSet<Subscription<{componentDetails.ComponentName}CommandReceiver>>> entityIdToReceiveSubscriptions;

private HashSet<EntityId> entitiesMatchingRequirements = new HashSet<EntityId>();
private HashSet<EntityId> entitiesNotMatchingRequirements = new HashSet<EntityId>();

public {componentDetails.ComponentName}CommandReceiverSubscriptionManager(World world)
{{
    this.world = world;

    // Check that these are there
    workerSystem = world.GetExistingSystem<WorkerSystem>();
    componentUpdateSystem = world.GetExistingSystem<ComponentUpdateSystem>();

    var constraintSystem = world.GetExistingSystem<ComponentConstraintsCallbackSystem>();

    constraintSystem.RegisterAuthorityCallback({componentDetails.ComponentName}.ComponentId, authorityChange =>
    {{
        if (authorityChange.Authority == Authority.Authoritative)
        {{
            if (!entitiesNotMatchingRequirements.Contains(authorityChange.EntityId))
            {{
                return;
            }}

            workerSystem.TryGetEntity(authorityChange.EntityId, out var entity);

            foreach (var subscription in entityIdToReceiveSubscriptions[authorityChange.EntityId])
            {{
                subscription.SetAvailable(new {componentDetails.ComponentName}CommandReceiver(world, entity, authorityChange.EntityId));
            }}

            entitiesMatchingRequirements.Add(authorityChange.EntityId);
            entitiesNotMatchingRequirements.Remove(authorityChange.EntityId);
        }}
        else if (authorityChange.Authority == Authority.NotAuthoritative)
        {{
            if (!entitiesMatchingRequirements.Contains(authorityChange.EntityId))
            {{
                return;
            }}

            workerSystem.TryGetEntity(authorityChange.EntityId, out var entity);

            foreach (var subscription in entityIdToReceiveSubscriptions[authorityChange.EntityId])
            {{
                ResetValue(subscription);
                subscription.SetUnavailable();
            }}

            entitiesNotMatchingRequirements.Add(authorityChange.EntityId);
            entitiesMatchingRequirements.Remove(authorityChange.EntityId);
        }}
    }});
}}

public override Subscription<{componentDetails.ComponentName}CommandReceiver> Subscribe(EntityId entityId)
{{
    if (entityIdToReceiveSubscriptions == null)
    {{
        entityIdToReceiveSubscriptions = new Dictionary<EntityId, HashSet<Subscription<{componentDetails.ComponentName}CommandReceiver>>>();
    }}

    var subscription = new Subscription<{componentDetails.ComponentName}CommandReceiver>(this, entityId);

    if (!entityIdToReceiveSubscriptions.TryGetValue(entityId, out var subscriptions))
    {{
        subscriptions = new HashSet<Subscription<{componentDetails.ComponentName}CommandReceiver>>();
        entityIdToReceiveSubscriptions.Add(entityId, subscriptions);
    }}

    if (workerSystem.TryGetEntity(entityId, out var entity)
        && componentUpdateSystem.HasComponent({componentDetails.ComponentName}.ComponentId, entityId)
        && componentUpdateSystem.GetAuthority(entityId, {componentDetails.ComponentName}.ComponentId) != Authority.NotAuthoritative)
    {{
        entitiesMatchingRequirements.Add(entityId);
        subscription.SetAvailable(new {componentDetails.ComponentName}CommandReceiver(world, entity, entityId));
    }}
    else
    {{
        entitiesNotMatchingRequirements.Add(entityId);
    }}

    subscriptions.Add(subscription);
    return subscription;
}}

public override void Cancel(ISubscription subscription)
{{
    var sub = ((Subscription<{componentDetails.ComponentName}CommandReceiver>) subscription);
    if (sub.HasValue)
    {{
        var receiver = sub.Value;
        receiver.IsValid = false;
        receiver.RemoveAllCallbacks();
    }}

    var subscriptions = entityIdToReceiveSubscriptions[sub.EntityId];
    subscriptions.Remove(sub);
    if (subscriptions.Count == 0)
    {{
        entityIdToReceiveSubscriptions.Remove(sub.EntityId);
        entitiesMatchingRequirements.Remove(sub.EntityId);
        entitiesNotMatchingRequirements.Remove(sub.EntityId);
    }}
}}

public override void ResetValue(ISubscription subscription)
{{
    var sub = ((Subscription<{componentDetails.ComponentName}CommandReceiver>) subscription);
    if (sub.HasValue)
    {{
        sub.Value.RemoveAllCallbacks();
    }}
}}
");
                });
        }

        private static TypeBlock GenerateCommandSender(UnityComponentDetails componentDetails, string qualifiedNamespace, string componentNamespace)
        {
            Logger.Trace($"Generating {qualifiedNamespace}.{componentDetails.ComponentName}CommandSender class.");

            return Scope.Type($"public class {componentDetails.ComponentName}CommandSender", c =>
            {
                c.Line($@"
public bool IsValid;

private readonly Entity entity;
private readonly CommandSystem commandSender;
private readonly CommandCallbackSystem callbackSystem;

private int callbackEpoch;
");
                c.Line($@"
internal {componentDetails.ComponentName}CommandSender(Entity entity, World world)
{{
    this.entity = entity;
    callbackSystem = world.GetOrCreateSystem<CommandCallbackSystem>();
    // todo check that this exists
    commandSender = world.GetExistingSystem<CommandSystem>();

    IsValid = true;
}}
");
                foreach (var commandDetails in componentDetails.CommandDetails)
                {
                    var receivedCommandResponseType = $"{componentNamespace}.{commandDetails.CommandName}.ReceivedResponse";
                    var commandRequest = $"{componentDetails.ComponentName}.{commandDetails.CommandName}.Request";

                    c.Line($@"
public void Send{commandDetails.CommandName}Command(EntityId targetEntityId, {commandDetails.FqnRequestType} request, Action<{receivedCommandResponseType}> callback = null)
{{
    var commandRequest = new {commandRequest}(targetEntityId, request);
    Send{commandDetails.CommandName}Command(commandRequest, callback);
}}

public void Send{commandDetails.CommandName}Command({componentNamespace}.{commandDetails.CommandName}.Request request, Action<{componentNamespace}.{commandDetails.CommandName}.ReceivedResponse> callback = null)
{{
    int validCallbackEpoch = callbackEpoch;
    var requestId = commandSender.SendCommand(request, entity);
    if (callback != null)
    {{
        Action<{componentNamespace}.{commandDetails.CommandName}.ReceivedResponse> wrappedCallback = response =>
        {{
            if (!this.IsValid || validCallbackEpoch != this.callbackEpoch)
            {{
                return;
            }}

            callback(response);
        }};
        callbackSystem.RegisterCommandResponseCallback(requestId, wrappedCallback);
    }}
}}
");
                }

                c.Line(@"
public void RemoveAllCallbacks()
{{
    ++callbackEpoch;
}}
");
            });
        }

        private static TypeBlock GenerateCommandReceiver(UnityComponentDetails componentDetails, string qualifiedNamespace, string componentNamespace)
        {
            Logger.Trace($"Generating {qualifiedNamespace}.{componentDetails.ComponentName}CommandReceiver class.");

            return Scope.Type($"public class {componentDetails.ComponentName}CommandReceiver", c =>
            {
                c.Line(@"
public bool IsValid;

private readonly EntityId entityId;
private readonly CommandCallbackSystem callbackSystem;
private readonly CommandSystem commandSystem;
");

                foreach (var commandDetails in componentDetails.CommandDetails)
                {
                    c.Line($@"
private Dictionary<Action<{componentNamespace}.{commandDetails.CommandName}.ReceivedRequest>, ulong> {commandDetails.CamelCaseCommandName}CallbackToCallbackKey;

public event Action<{componentNamespace}.{commandDetails.CommandName}.ReceivedRequest> On{commandDetails.CommandName}RequestReceived
{{
    add
    {{
        if ({commandDetails.CamelCaseCommandName}CallbackToCallbackKey == null)
        {{
            {commandDetails.CamelCaseCommandName}CallbackToCallbackKey = new Dictionary<Action<{componentNamespace}.{commandDetails.CommandName}.ReceivedRequest>, ulong>();
        }}

        var key = callbackSystem.RegisterCommandRequestCallback(entityId, value);
        {commandDetails.CamelCaseCommandName}CallbackToCallbackKey.Add(value, key);
    }}
    remove
    {{
        if (!{commandDetails.CamelCaseCommandName}CallbackToCallbackKey.TryGetValue(value, out var key))
        {{
            return;
        }}

        callbackSystem.UnregisterCommandRequestCallback(key);
        {commandDetails.CamelCaseCommandName}CallbackToCallbackKey.Remove(value);
    }}
}}
");
                }

                c.Line($@"
internal {componentDetails.ComponentName}CommandReceiver(World world, Entity entity, EntityId entityId)
{{
    this.entityId = entityId;
    callbackSystem = world.GetOrCreateSystem<CommandCallbackSystem>();
    commandSystem = world.GetExistingSystem<CommandSystem>();
    // should check the system actually exists

    IsValid = true;
}}
");

                foreach (var commandDetails in componentDetails.CommandDetails)
                {
                    c.Line($@"
public void Send{commandDetails.CommandName}Response({componentNamespace}.{commandDetails.CommandName}.Response response)
{{
    commandSystem.SendResponse(response);
}}

public void Send{commandDetails.CommandName}Response(long requestId, {commandDetails.FqnResponseType} response)
{{
    commandSystem.SendResponse(new {componentNamespace}.{commandDetails.CommandName}.Response(requestId, response));
}}

public void Send{commandDetails.CommandName}Failure(long requestId, string failureMessage)
{{
    commandSystem.SendResponse(new {componentNamespace}.{commandDetails.CommandName}.Response(requestId, failureMessage));
}}
");
                }

                c.Method("public void RemoveAllCallbacks()", m =>
                {
                    foreach (var commandDetails in componentDetails.CommandDetails)
                    {
                        m.Line($@"
if ({commandDetails.CamelCaseCommandName}CallbackToCallbackKey != null)
{{
    foreach (var callbackToKey in {commandDetails.CamelCaseCommandName}CallbackToCallbackKey)
    {{
        callbackSystem.UnregisterCommandRequestCallback(callbackToKey.Value);
    }}

    {commandDetails.CamelCaseCommandName}CallbackToCallbackKey.Clear();
}}
");
                    }
                });
            });
        }
    }
}
