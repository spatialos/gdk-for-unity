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
                    "Improbable.Gdk.Core",
                    "Improbable.Gdk.Subscriptions",
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

            var commandSenderType = $"{componentDetails.ComponentName}CommandSender";

            return Scope.AnnotatedType("AutoRegisterSubscriptionManager",
                $"public class {componentDetails.ComponentName}CommandSenderSubscriptionManager : CommandSenderSubscriptionManagerBase<{commandSenderType}>",
                t =>
                {
                    t.Line($@"
public {componentDetails.ComponentName}CommandSenderSubscriptionManager(World world) : base(world)
{{
}}

protected override {commandSenderType} CreateSender(Entity entity, World world)
{{
    return new {commandSenderType}(entity, world);
}}
");
                });
        }

        private static TypeBlock GenerateCommandReceiverSubscriptionManager(UnityComponentDetails componentDetails, string qualifiedNamespace)
        {
            Logger.Trace($"Generating {qualifiedNamespace}.{componentDetails.ComponentName}CommandReceiverSubscriptionManager class.");

            var commandReceiverType = $"{componentDetails.ComponentName}CommandReceiver";

            return Scope.AnnotatedType("AutoRegisterSubscriptionManager",
                $"public class {componentDetails.ComponentName}CommandReceiverSubscriptionManager : CommandReceiverSubscriptionManagerBase<{commandReceiverType}>",
                t =>
                {
                    t.Line($@"
public {componentDetails.ComponentName}CommandReceiverSubscriptionManager(World world) : base(world, {componentDetails.ComponentName}.ComponentId)
{{
}}

protected override {commandReceiverType} CreateReceiver(World world, Entity entity, EntityId entityId)
{{
    return new {commandReceiverType}(world, entity, entityId);
}}
");
                });
        }

        private static TypeBlock GenerateCommandSender(UnityComponentDetails componentDetails, string qualifiedNamespace, string componentNamespace)
        {
            Logger.Trace($"Generating {qualifiedNamespace}.{componentDetails.ComponentName}CommandSender class.");

            var commandSenderType = $"{componentDetails.ComponentName}CommandSender";

            return Scope.Type($"public class {commandSenderType} : ICommandSender", c =>
            {
                c.Line($@"
private readonly Entity entity;
private readonly CommandSystem commandSender;
private readonly CommandCallbackSystem callbackSystem;
private int callbackEpoch;

public bool IsValid {{ get; set; }}
");
                c.Line($@"
internal {commandSenderType}(Entity entity, World world)
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
{
    ++callbackEpoch;
}
");
            });
        }

        private static TypeBlock GenerateCommandReceiver(UnityComponentDetails componentDetails, string qualifiedNamespace, string componentNamespace)
        {
            Logger.Trace($"Generating {qualifiedNamespace}.{componentDetails.ComponentName}CommandReceiver class.");

            var commandReceiverType = $"{componentDetails.ComponentName}CommandReceiver";

            return Scope.Type($"public class {commandReceiverType} : ICommandReceiver", c =>
            {
                c.Line(@"
private readonly EntityId entityId;
private readonly CommandCallbackSystem callbackSystem;
private readonly CommandSystem commandSystem;

public bool IsValid { get; set; }
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
internal {commandReceiverType}(World world, Entity entity, EntityId entityId)
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
