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
public {componentDetails.Name}ReaderSubscriptionManager(World world) : base(world)
{{
}}

protected override {componentReaderType} CreateReader(Entity entity, EntityId entityId)
{{
    return new {componentReaderType}(World, entity, entityId);
}}
");
                });
        }

        private static TypeBlock GenerateComponentWriterSubscriptionManager(UnityComponentDetails componentDetails)
        {
            Logger.Trace($"Generating {componentDetails.Namespace}.{componentDetails.Name}WriterSubscriptionManager class.");
            var componentWriterType = $"{componentDetails.Name}Writer";

            return Scope.AnnotatedType("AutoRegisterSubscriptionManager",
                $"public class {componentDetails.Name}WriterSubscriptionManager : WriterSubscriptionManager<{componentDetails.Name}.Component, {componentWriterType}>",
                wsm =>
                {
                    wsm.Line($@"
public {componentDetails.Name}WriterSubscriptionManager(World world) : base(world)
{{
}}

protected override {componentWriterType} CreateWriter(Entity entity, EntityId entityId)
{{
    return new {componentWriterType}(World, entity, entityId);
}}
");
                });
        }

        private static TypeBlock GenerateComponentReader(UnityComponentDetails componentDetails)
        {
            Logger.Trace($"Generating {componentDetails.Namespace}.{componentDetails.Name}Reader class.");

            return Scope.Type($"public class {componentDetails.Name}Reader : Reader<{componentDetails.Name}.Component, {componentDetails.Name}.Update>",
                reader =>
                {
                    // Field callbacks
                    foreach (var fieldDetails in componentDetails.FieldDetails)
                    {
                        reader.Line($@"
private Dictionary<Action<{fieldDetails.Type}>, ulong> {fieldDetails.CamelCaseName}UpdateCallbackToCallbackKey;");
                    }

                    reader.Line($@"
internal {componentDetails.Name}Reader(World world, Entity entity, EntityId entityId) : base(world, entity, entityId)
{{
}}
");

                    foreach (var fieldDetails in componentDetails.FieldDetails)
                    {
                        reader.Line($@"
public event Action<{fieldDetails.Type}> On{fieldDetails.PascalCaseName}Update
{{
    add
    {{
        if (!IsValid)
        {{
            throw new InvalidOperationException(""Cannot add field update callback when Reader is not valid."");
        }}

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
        if (!IsValid)
        {{
            throw new InvalidOperationException(""Cannot add event callback when Reader is not valid."");
        }}

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

                    if (componentDetails.FieldDetails.Count > 0)
                    {
                        reader.Method($"protected override void RemoveFieldCallbacks()", m =>
                        {
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
                        });
                    }

                    if (componentDetails.EventDetails.Count > 0)
                    {
                        reader.Method($"protected override void RemoveEventCallbacks()", m =>
                        {
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
                    }
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
