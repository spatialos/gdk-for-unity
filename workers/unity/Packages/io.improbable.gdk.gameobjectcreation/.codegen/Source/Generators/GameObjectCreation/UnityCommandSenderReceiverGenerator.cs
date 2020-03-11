using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.CodeWriter.Scopes;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public static class UnityCommandSenderReceiverGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static CodeWriter Generate(UnityComponentDetails details)
        {
            var fullyQualifiedNamespace = $"global::{details.Namespace}.{details.Name}";

            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "System",
                    "System.Collections.Generic",
                    "System.Threading.Tasks",
                    "Unity.Entities",
                    "Improbable.Gdk.Core",
                    "Improbable.Gdk.Subscriptions",
                    "Entity = Unity.Entities.Entity"
                );

                cgw.Namespace(details.Namespace, ns =>
                {
                    ns.Type(GenerateCommandSenderSubscriptionManager(details));
                    ns.Type(GenerateCommandReceiverSubscriptionManager(details));
                    ns.Type(GenerateCommandSender(details, fullyQualifiedNamespace));
                    ns.Type(GenerateCommandReceiver(details, fullyQualifiedNamespace));
                });
            });
        }

        private static TypeBlock GenerateCommandSenderSubscriptionManager(UnityComponentDetails componentDetails)
        {
            Logger.Trace($"Generating {componentDetails.Namespace}.{componentDetails.Name}CommandSenderSubscriptionManager class.");

            var commandSenderType = $"{componentDetails.Name}CommandSender";

            return Scope.AnnotatedType("AutoRegisterSubscriptionManager",
                $"public class {componentDetails.Name}CommandSenderSubscriptionManager : CommandSenderSubscriptionManagerBase<{commandSenderType}>",
                t =>
                {
                    t.Line($@"
public {componentDetails.Name}CommandSenderSubscriptionManager(World world) : base(world)
{{
}}

protected override {commandSenderType} CreateSender(Entity entity, World world)
{{
    return new {commandSenderType}(entity, world);
}}
");
                });
        }

        private static TypeBlock GenerateCommandReceiverSubscriptionManager(UnityComponentDetails componentDetails)
        {
            Logger.Trace($"Generating {componentDetails.Namespace}.{componentDetails.Name}CommandReceiverSubscriptionManager class.");

            var commandReceiverType = $"{componentDetails.Name}CommandReceiver";

            return Scope.AnnotatedType("AutoRegisterSubscriptionManager",
                $"public class {componentDetails.Name}CommandReceiverSubscriptionManager : CommandReceiverSubscriptionManagerBase<{commandReceiverType}>",
                t =>
                {
                    t.Line($@"
public {componentDetails.Name}CommandReceiverSubscriptionManager(World world) : base(world, {componentDetails.Name}.ComponentId)
{{
}}

protected override {commandReceiverType} CreateReceiver(World world, Entity entity, EntityId entityId)
{{
    return new {commandReceiverType}(world, entity, entityId);
}}
");
                });
        }

        private static TypeBlock GenerateCommandSender(UnityComponentDetails componentDetails, string fullyQualifiedNamespace)
        {
            Logger.Trace($"Generating {componentDetails.Namespace}.{componentDetails.Name}CommandSender class.");

            var commandSenderType = $"{componentDetails.Name}CommandSender";

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
                    var receivedCommandResponseType = $"{fullyQualifiedNamespace}.{commandDetails.PascalCaseName}.ReceivedResponse";
                    var commandRequest = $"{componentDetails.Name}.{commandDetails.PascalCaseName}.Request";

                    c.Line($@"
public Task<{receivedCommandResponseType}> Send{commandDetails.PascalCaseName}Command(EntityId targetEntityId, {commandDetails.FqnRequestType} request)
{{
    var commandRequest = new {commandRequest}(targetEntityId, request);
    return Send{commandDetails.PascalCaseName}Command(commandRequest);
}}

public Task<{receivedCommandResponseType}> Send{commandDetails.PascalCaseName}Command({fullyQualifiedNamespace}.{commandDetails.PascalCaseName}.Request request)
{{
    var validCallbackEpoch = callbackEpoch;
    var requestId = commandSender.SendCommand(request, entity);
    var tcs = new TaskCompletionSource<{receivedCommandResponseType}>();

    callbackSystem.RegisterCommandResponseCallback<{fullyQualifiedNamespace}.{commandDetails.PascalCaseName}.ReceivedResponse>(requestId, response =>
    {{
        if (!this.IsValid || validCallbackEpoch != this.callbackEpoch)
        {{
            tcs.SetCanceled();
            return;
        }}

        tcs.TrySetResult(response);
    }});
    
    return tcs.Task;
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

        private static TypeBlock GenerateCommandReceiver(UnityComponentDetails componentDetails, string fullyQualifiedNamespace)
        {
            Logger.Trace($"Generating {componentDetails.Namespace}.{componentDetails.Name}CommandReceiver class.");

            var commandReceiverType = $"{componentDetails.Name}CommandReceiver";

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
private Dictionary<Action<{fullyQualifiedNamespace}.{commandDetails.PascalCaseName}.ReceivedRequest>, ulong> {commandDetails.CamelCaseName}CallbackToCallbackKey;

public event Action<{fullyQualifiedNamespace}.{commandDetails.PascalCaseName}.ReceivedRequest> On{commandDetails.PascalCaseName}RequestReceived
{{
    add
    {{
        if ({commandDetails.CamelCaseName}CallbackToCallbackKey == null)
        {{
            {commandDetails.CamelCaseName}CallbackToCallbackKey = new Dictionary<Action<{fullyQualifiedNamespace}.{commandDetails.PascalCaseName}.ReceivedRequest>, ulong>();
        }}

        var key = callbackSystem.RegisterCommandRequestCallback(entityId, value);
        {commandDetails.CamelCaseName}CallbackToCallbackKey.Add(value, key);
    }}
    remove
    {{
        if (!{commandDetails.CamelCaseName}CallbackToCallbackKey.TryGetValue(value, out var key))
        {{
            return;
        }}

        callbackSystem.UnregisterCommandRequestCallback(key);
        {commandDetails.CamelCaseName}CallbackToCallbackKey.Remove(value);
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
public void Send{commandDetails.PascalCaseName}Response({fullyQualifiedNamespace}.{commandDetails.PascalCaseName}.Response response)
{{
    commandSystem.SendResponse(response);
}}

public void Send{commandDetails.PascalCaseName}Response(long requestId, {commandDetails.FqnResponseType} response)
{{
    commandSystem.SendResponse(new {fullyQualifiedNamespace}.{commandDetails.PascalCaseName}.Response(requestId, response));
}}

public void Send{commandDetails.PascalCaseName}Failure(long requestId, string failureMessage)
{{
    commandSystem.SendResponse(new {fullyQualifiedNamespace}.{commandDetails.PascalCaseName}.Response(requestId, failureMessage));
}}
");
                }

                c.Method("public void RemoveAllCallbacks()", m =>
                {
                    foreach (var commandDetails in componentDetails.CommandDetails)
                    {
                        m.Line($@"
if ({commandDetails.CamelCaseName}CallbackToCallbackKey != null)
{{
    foreach (var callbackToKey in {commandDetails.CamelCaseName}CallbackToCallbackKey)
    {{
        callbackSystem.UnregisterCommandRequestCallback(callbackToKey.Value);
    }}

    {commandDetails.CamelCaseName}CallbackToCallbackKey.Clear();
}}
");
                    }
                });
            });
        }
    }
}
