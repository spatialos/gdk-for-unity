using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.CodeWriter.Scopes;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public static class UnityComponentReaderWriterGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static CodeWriter Generate(UnityComponentDetails details)
        {
            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "System",
                    "System.Collections.Generic",
                    "Improbable.Worker.CInterop",
                    "Improbable.Gdk.Core",
                    "Improbable.Gdk.Subscriptions",
                    "Unity.Entities",
                    "Entity = Unity.Entities.Entity"
                );

                cgw.Namespace(details.Namespace, ns =>
                {
                    ns.Type(GenerateComponentReaderSubscriptionManager(details));
                    ns.Type(GenerateComponentWriterSubscriptionManager(details));
                    ns.Type(GenerateComponentReader(details));
                    ns.Type(GenerateComponentWriter(details));
                });
            });
        }

        private static TypeBlock GenerateComponentReaderSubscriptionManager(UnityComponentDetails componentDetails)
        {
            Logger.Trace($"Generating {componentDetails.Namespace}.{componentDetails.Name}ReaderSubscriptionManager class.");
            var componentReaderType = $"{componentDetails.Name}Reader";

            return Scope.AnnotatedType("AutoRegisterSubscriptionManager",
                $"public class {componentDetails.Name}ReaderSubscriptionManager : ReaderSubscriptionManager<{componentDetails.Name}.Component, {componentReaderType}>",
                rsm =>
                {
                    rsm.Line($@"
public {componentDetails.Name}ReaderSubscriptionManager(World world) : base({componentDetails.Name}.ComponentId, world)
{{
}}

protected override {componentReaderType} CreateReader(Entity entity, EntityId entityId)
{{
    return new {componentReaderType}(world, entity, entityId);
}}
");
                });
        }

        private static TypeBlock GenerateComponentWriterSubscriptionManager(UnityComponentDetails componentDetails)
        {
            Logger.Trace($"Generating {componentDetails.Namespace}.{componentDetails.Name}WriterSubscriptionManager class.");
            var componentWriterType = $"{componentDetails.Name}Writer";

            return Scope.AnnotatedType("AutoRegisterSubscriptionManager",
                $"public class {componentDetails.Name}WriterSubscriptionManager : WriterSubscriptionManager<{componentWriterType}>",
                wsm =>
                {
                    wsm.Line($@"
public {componentDetails.Name}WriterSubscriptionManager(World world) : base({componentDetails.Name}.ComponentId, world)
{{
}}

protected override {componentWriterType} CreateWriter(Entity entity, EntityId entityId)
{{
    return new {componentWriterType}(world, entity, entityId);
}}
");
                });
        }

        private static TypeBlock GenerateComponentReader(UnityComponentDetails componentDetails)
        {
            Logger.Trace($"Generating {componentDetails.Namespace}.{componentDetails.Name}Reader class.");

            return Scope.Type($"public class {componentDetails.Name}Reader : IRequireable",
                reader =>
                {
                    reader.Line($@"
public bool IsValid {{ get; set; }}

protected readonly ComponentUpdateSystem ComponentUpdateSystem;
protected readonly ComponentCallbackSystem CallbackSystem;
protected readonly EntityManager EntityManager;
protected readonly Entity Entity;
protected readonly EntityId EntityId;

public {componentDetails.Name}.Component Data
{{
    get
    {{
        if (!IsValid)
        {{
            throw new InvalidOperationException(""Cannot read component data when Reader is not valid."");
        }}

        return EntityManager.GetComponentData<{componentDetails.Name}.Component>(Entity);
    }}
}}

public Authority Authority
{{
    get
    {{
        if (!IsValid)
        {{
            throw new InvalidOperationException(""Cannot read authority when Reader is not valid"");
        }}

        return ComponentUpdateSystem.GetAuthority(EntityId, {componentDetails.Name}.ComponentId);
    }}
}}

private Dictionary<Action<Authority>, ulong> authorityCallbackToCallbackKey;
public event Action<Authority> OnAuthorityUpdate
{{
    add
    {{
        if (authorityCallbackToCallbackKey == null)
        {{
            authorityCallbackToCallbackKey = new Dictionary<Action<Authority>, ulong>();
        }}

        var key = CallbackSystem.RegisterAuthorityCallback(EntityId, {componentDetails.Name}.ComponentId, value);
        authorityCallbackToCallbackKey.Add(value, key);
    }}
    remove
    {{
        if (!authorityCallbackToCallbackKey.TryGetValue(value, out var key))
        {{
            return;
        }}

        CallbackSystem.UnregisterCallback(key);
        authorityCallbackToCallbackKey.Remove(value);
    }}
}}

private Dictionary<Action<{componentDetails.Name}.Update>, ulong> updateCallbackToCallbackKey;
public event Action<{componentDetails.Name}.Update> OnUpdate
{{
    add
    {{
        if (updateCallbackToCallbackKey == null)
        {{
            updateCallbackToCallbackKey = new Dictionary<Action<{componentDetails.Name}.Update>, ulong>();
        }}

        var key = CallbackSystem.RegisterComponentUpdateCallback(EntityId, value);
        updateCallbackToCallbackKey.Add(value, key);
    }}
    remove
    {{
        if (!updateCallbackToCallbackKey.TryGetValue(value, out var key))
        {{
            return;
        }}

        CallbackSystem.UnregisterCallback(key);
        updateCallbackToCallbackKey.Remove(value);
    }}
}}
");
                    foreach (var fieldDetails in componentDetails.FieldDetails)
                    {
                        reader.Line($@"
private Dictionary<Action<{fieldDetails.Type}>, ulong> {fieldDetails.CamelCaseName}UpdateCallbackToCallbackKey;
public event Action<{fieldDetails.Type}> On{fieldDetails.PascalCaseName}Update
{{
    add
    {{
        if ({fieldDetails.CamelCaseName}UpdateCallbackToCallbackKey == null)
        {{
            {fieldDetails.CamelCaseName}UpdateCallbackToCallbackKey = new Dictionary<Action<{fieldDetails.Type}>, ulong>();
        }}

        var key = CallbackSystem.RegisterComponentUpdateCallback<{componentDetails.Name}.Update>(EntityId, update =>
        {{
            if (update.{fieldDetails.PascalCaseName}.HasValue)
            {{
                value(update.{fieldDetails.PascalCaseName}.Value);
            }}
        }});
        {fieldDetails.CamelCaseName}UpdateCallbackToCallbackKey.Add(value, key);
    }}
    remove
    {{
        if (!{fieldDetails.CamelCaseName}UpdateCallbackToCallbackKey.TryGetValue(value, out var key))
        {{
            return;
        }}

        CallbackSystem.UnregisterCallback(key);
        {fieldDetails.CamelCaseName}UpdateCallbackToCallbackKey.Remove(value);
    }}
}}
");
                    }

                    foreach (var eventDetails in componentDetails.EventDetails)
                    {
                        var eventType = $"{componentDetails.Name}.{eventDetails.PascalCaseName}.Event";

                        reader.Line($@"
private Dictionary<Action<{eventDetails.FqnPayloadType}>, ulong> {eventDetails.CamelCaseName}EventCallbackToCallbackKey;
public event Action<{eventDetails.FqnPayloadType}> On{eventDetails.PascalCaseName}Event
{{
    add
    {{
        if ({eventDetails.CamelCaseName}EventCallbackToCallbackKey == null)
        {{
            {eventDetails.CamelCaseName}EventCallbackToCallbackKey = new Dictionary<Action<{eventDetails.FqnPayloadType}>, ulong>();
        }}

        var key = CallbackSystem.RegisterComponentEventCallback<{eventType}>(EntityId, ev => value(ev.Payload));
        {eventDetails.CamelCaseName}EventCallbackToCallbackKey.Add(value, key);
    }}
    remove
    {{
        if (!{eventDetails.CamelCaseName}EventCallbackToCallbackKey.TryGetValue(value, out var key))
        {{
            return;
        }}

        CallbackSystem.UnregisterCallback(key);
        {eventDetails.CamelCaseName}EventCallbackToCallbackKey.Remove(value);
    }}
}}
");
                    }

                    reader.Line($@"
internal {componentDetails.Name}Reader(World world, Entity entity, EntityId entityId)
{{
    Entity = entity;
    EntityId = entityId;

    IsValid = true;

    ComponentUpdateSystem = world.GetExistingSystem<ComponentUpdateSystem>();
    CallbackSystem = world.GetExistingSystem<ComponentCallbackSystem>();
    EntityManager = world.EntityManager;
}}
");

                    reader.Method($"public void RemoveAllCallbacks()", m =>
                    {
                        m.Line($@"
if (authorityCallbackToCallbackKey != null)
{{
    foreach (var callbackToKey in authorityCallbackToCallbackKey)
    {{
        CallbackSystem.UnregisterCallback(callbackToKey.Value);
    }}

    authorityCallbackToCallbackKey.Clear();
}}

if (updateCallbackToCallbackKey != null)
{{
    foreach (var callbackToKey in updateCallbackToCallbackKey)
    {{
        CallbackSystem.UnregisterCallback(callbackToKey.Value);
    }}

    updateCallbackToCallbackKey.Clear();
}}
");

                        foreach (var fieldDetails in componentDetails.FieldDetails)
                        {
                            m.Line($@"
if ({fieldDetails.CamelCaseName}UpdateCallbackToCallbackKey != null)
{{
    foreach (var callbackToKey in {fieldDetails.CamelCaseName}UpdateCallbackToCallbackKey)
    {{
        CallbackSystem.UnregisterCallback(callbackToKey.Value);
    }}

    {fieldDetails.CamelCaseName}UpdateCallbackToCallbackKey.Clear();
}}
");
                        }

                        foreach (var eventDetails in componentDetails.EventDetails)
                        {
                            m.Line($@"
if ({eventDetails.CamelCaseName}EventCallbackToCallbackKey != null)
{{
    foreach (var callbackToKey in {eventDetails.CamelCaseName}EventCallbackToCallbackKey)
    {{
        CallbackSystem.UnregisterCallback(callbackToKey.Value);
    }}

    {eventDetails.CamelCaseName}EventCallbackToCallbackKey.Clear();
}}
");
                        }
                    });
                });
        }

        private static TypeBlock GenerateComponentWriter(UnityComponentDetails componentDetails)
        {
            Logger.Trace($"Generating {componentDetails.Namespace}.{componentDetails.Name}Writer class.");

            return Scope.Type($"public class {componentDetails.Name}Writer : {componentDetails.Name}Reader",
                writer =>
                {
                    writer.Line($@"
internal {componentDetails.Name}Writer(World world, Entity entity, EntityId entityId)
    : base(world, entity, entityId)
{{
}}
");
                    writer.Method($"public void SendUpdate({componentDetails.Name}.Update update)", m =>
                    {
                        m.Line($"var component = EntityManager.GetComponentData<{componentDetails.Name}.Component>(Entity);");

                        foreach (var fieldDetails in componentDetails.FieldDetails)
                        {
                            m.Line($@"
if (update.{fieldDetails.PascalCaseName}.HasValue)
{{
    component.{fieldDetails.PascalCaseName} = update.{fieldDetails.PascalCaseName}.Value;
}}
");
                        }

                        m.Line($"EntityManager.SetComponentData(Entity, component);");
                    });

                    foreach (var eventDetails in componentDetails.EventDetails)
                    {
                        var eventType = $"{componentDetails.Name}.{eventDetails.PascalCaseName}.Event";
                        writer.Line($@"
public void Send{eventDetails.PascalCaseName}Event({eventDetails.FqnPayloadType} {eventDetails.CamelCaseName})
{{
    var eventToSend = new {eventType}({eventDetails.CamelCaseName});
    ComponentUpdateSystem.SendEvent(eventToSend, EntityId);
}}
");
                    }

                    writer.Line($@"
public void AcknowledgeAuthorityLoss()
{{
    ComponentUpdateSystem.AcknowledgeAuthorityLoss(EntityId, {componentDetails.Name}.ComponentId);
}}
");
                });
        }
    }
}
