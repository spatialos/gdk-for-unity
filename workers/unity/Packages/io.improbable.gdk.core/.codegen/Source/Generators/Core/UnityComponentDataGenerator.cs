using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.CodeWriter.Scopes;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public class UnityComponentDataGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static CodeWriter Generate(UnityComponentDetails componentDetails)
        {
            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "Improbable.Gdk.Core",
                    "Improbable.Worker.CInterop",
                    "System",
                    "System.Collections.Generic",
                    "System.Diagnostics",
                    "Unity.Entities"
                );

                cgw.Namespace(componentDetails.Namespace, ns =>
                {
                    ns.Type($"public partial class {componentDetails.Name}", partial =>
                    {
                        partial.Line($"public const uint ComponentId = {componentDetails.ComponentId};");

                        partial.Type(GenerateComponentStruct(componentDetails));
                        partial.Type(GenerateComponentAuthorityStruct(componentDetails));
                        partial.Type(GenerateSnapshotStruct(componentDetails));
                        partial.Type(GenerateSerializationClass(componentDetails));
                        partial.Type(GenerateUpdateStruct(componentDetails));
                        partial.Type(GenerateInternalDynamicClass(componentDetails));
                    });
                });
            });
        }

        private static TypeBlock GenerateComponentStruct(UnityComponentDetails componentDetails)
        {
            Logger.Trace($"Generating {componentDetails.Namespace}.{componentDetails.Name}.Component struct.");

            var fieldDetailsList = componentDetails.FieldDetails;

            var dirtyType = typeof(uint);
            var dirtyBytesPerEntry = Marshal.SizeOf(dirtyType);
            var dirtyBitsPerEntry = dirtyBytesPerEntry * 8;
            var dirtyBitCount = fieldDetailsList.Count / dirtyBitsPerEntry + 1;

            return Scope.Type(
                "public unsafe struct Component : IComponentData, ISpatialComponentData, ISnapshottable<Snapshot>",
                component =>
                {
                    component.Line(new[]
                    {
                        "// Bit masks for tracking which component properties were changed locally and need to be synced.",
                        $"private fixed {dirtyType.Name} dirtyBits[{dirtyBitCount}];"
                    });

                    for (var i = 0; i < fieldDetailsList.Count; i++)
                    {
                        var fieldDetails = fieldDetailsList[i];
                        if (fieldDetails.IsBlittable)
                        {
                            component.Line($@"
private {fieldDetails.Type} {fieldDetails.CamelCaseName};

public {fieldDetails.Type} {fieldDetails.PascalCaseName}
{{
    get => {fieldDetails.CamelCaseName};
    set
    {{
        MarkDataDirty({i});
        this.{fieldDetails.CamelCaseName} = value;
    }}
}}
");
                        }
                        else
                        {
                            component.Line($@"
internal uint {fieldDetails.CamelCaseName}Handle;

public {fieldDetails.Type} {fieldDetails.PascalCaseName}
{{
    get => global::{componentDetails.Namespace}.{componentDetails.Name}.ReferenceTypeProviders.{fieldDetails.PascalCaseName}Provider.Get({fieldDetails.CamelCaseName}Handle);
    set
    {{
        MarkDataDirty({i});
        global::{componentDetails.Namespace}.{componentDetails.Name}.ReferenceTypeProviders.{fieldDetails.PascalCaseName}Provider.Set({fieldDetails.CamelCaseName}Handle, value);
    }}
}}
");
                        }
                    }

                    component.Method("public bool IsDataDirty()", m =>
                    {
                        m.Line("var isDataDirty = false;");

                        m.Line(s =>
                        {
                            for (var i = 0; i < fieldDetailsList.Count / dirtyBitsPerEntry + 1; i++)
                            {
                                s.AppendLine($"isDataDirty |= (dirtyBits[{i}] != 0x0);");
                            }
                        });

                        m.Return("isDataDirty");
                    });

                    component.Line(@"
/*
The propertyIndex argument counts up from 0 in the order defined in your schema component.
It is not the schema field number itself. For example:
component MyComponent
{
    id = 1337;
    bool val_a = 1;
    bool val_b = 3;
}
In that case, val_a corresponds to propertyIndex 0 and val_b corresponds to propertyIndex 1 in this method.
This method throws an InvalidOperationException in case your component doesn't contain properties.
*/
");
                    component.Method("public bool IsDataDirty(int propertyIndex)", m =>
                    {
                        if (fieldDetailsList.Count == 0)
                        {
                            m.Line(
                                "throw new InvalidOperationException(\"IsDataDirty(int propertyIndex) may not be called on components with no properties.\");");
                        }
                        else
                        {
                            m.Line("ValidateFieldIndex(propertyIndex);");

                            m.Line(new[]
                            {
                                "// Retrieve the dirtyBits[0-n] field that tracks this property.",
                                $"var dirtyBitsByteIndex = propertyIndex >> {dirtyBytesPerEntry};",
                                $"return (dirtyBits[dirtyBitsByteIndex] & (0x1 << (propertyIndex & {dirtyBitsPerEntry - 1}))) != 0x0;"
                            });
                        }
                    });

                    component.Method(@"
// Like the IsDataDirty() method above, the propertyIndex arguments starts counting from 0.
// This method throws an InvalidOperationException in case your component doesn't contain properties.
public void MarkDataDirty(int propertyIndex)", m =>
                    {
                        if (fieldDetailsList.Count == 0)
                        {
                            m.Line(
                                "throw new InvalidOperationException(\"MarkDataDirty(int propertyIndex) may not be called on components with no properties.\");");
                        }
                        else
                        {
                            m.Line("ValidateFieldIndex(propertyIndex);");

                            m.Line(new[]
                            {
                                "// Retrieve the dirtyBits[0-n] field that tracks this property.",
                                $"var dirtyBitsByteIndex = propertyIndex >> {dirtyBytesPerEntry};",
                                $"dirtyBits[dirtyBitsByteIndex] |= ({dirtyType.Name}) (0x1 << (propertyIndex & {dirtyBitsPerEntry - 1}));"
                            });
                        }
                    });

                    component.Method("public void MarkDataClean()", m =>
                    {
                        m.Line(s =>
                        {
                            for (var i = 0; i < fieldDetailsList.Count / dirtyBitsPerEntry + 1; i++)
                            {
                                s.AppendLine($"dirtyBits[{i}] = 0x0;");
                            }
                        });
                    });

                    component.Annotate("Conditional(\"DEBUG\")")
                        .Method("private void ValidateFieldIndex(int propertyIndex)", () => new[]
                        {
                            $@"
if (propertyIndex < 0 || propertyIndex >= {fieldDetailsList.Count})
{{
    throw new ArgumentException(""\""propertyIndex\"" argument out of range. Valid range is [0, {fieldDetailsList.Count - 1}]. "" +
        ""Unless you are using custom component replication code, this is most likely caused by a code generation bug. "" +
        ""Please contact SpatialOS support if you encounter this issue."");
}}
"
                        });

                    component.Line($@"
public Snapshot ToComponentSnapshot(global::Unity.Entities.World world)
{{
    var componentDataSchema = new ComponentData({componentDetails.ComponentId}, SchemaComponentData.Create());
    Serialization.SerializeComponent(this, componentDataSchema.SchemaData.Value.GetFields(), world);
    var snapshot = Serialization.DeserializeSnapshot(componentDataSchema.SchemaData.Value.GetFields());

    componentDataSchema.SchemaData?.Destroy();
    componentDataSchema.SchemaData = null;

    return snapshot;
}}
");
                });
        }

        private static TypeBlock GenerateComponentAuthorityStruct(UnityComponentDetails componentDetails)
        {
            Logger.Trace(
                $"Generating {componentDetails.Namespace}.{componentDetails.Name}.ComponentAuthority struct.");

            return Scope.Type("public struct ComponentAuthority : ISharedComponentData, IEquatable<ComponentAuthority>",
                t =>
                {
                    t.Line(@"
public bool HasAuthority;

public ComponentAuthority(bool hasAuthority)
{
    HasAuthority = hasAuthority;
}

// todo think about whether any of this is necessary
// Unity does a bitwise equality check so this is just for users reading the struct
public static readonly ComponentAuthority NotAuthoritative = new ComponentAuthority(false);
public static readonly ComponentAuthority Authoritative = new ComponentAuthority(true);

public bool Equals(ComponentAuthority other)
{
    return this == other;
}

public override bool Equals(object obj)
{
    return obj is ComponentAuthority auth && this == auth;
}

public override int GetHashCode()
{
    return HasAuthority.GetHashCode();
}

public static bool operator ==(ComponentAuthority a, ComponentAuthority b)
{
    return a.HasAuthority == b.HasAuthority;
}

public static bool operator !=(ComponentAuthority a, ComponentAuthority b)
{
    return !(a == b);
}
");
                });
        }

        private static TypeBlock GenerateSnapshotStruct(UnityComponentDetails componentDetails)
        {
            Logger.Trace($"Generating {componentDetails.Namespace}.{componentDetails.Name}.Snapshot struct.");

            var fieldDetailsList = componentDetails.FieldDetails;

            return Scope.AnnotatedType("global::System.Serializable",
                "public struct Snapshot : ISpatialComponentSnapshot", snapshot =>
                {
                    snapshot.Line($"public uint ComponentId => {componentDetails.ComponentId};");

                    snapshot.Line(fieldDetailsList.Select(fd => $"public {fd.Type} {fd.PascalCaseName};").ToList());

                    if (fieldDetailsList.Count > 0)
                    {
                        snapshot.Method($"public Snapshot({GetConstructorArgs(fieldDetailsList)})",
                            () =>
                            {
                                return fieldDetailsList.Select(fd => $"{fd.PascalCaseName} = {fd.CamelCaseName};");
                            });
                    }
                });
        }

        private static TypeBlock GenerateSerializationClass(UnityComponentDetails componentDetails)
        {
            Logger.Trace(
                $"Generating {componentDetails.Namespace}.{componentDetails.Name}.Serialization static class.");

            var componentNamespace = $"global::{componentDetails.Namespace}.{componentDetails.Name}";
            var fieldDetailsList = componentDetails.FieldDetails;
            var shouldGenerateClearedFieldsSet = ShouldGenerateClearedFieldsSet(fieldDetailsList);

            return Scope.Type("public static class Serialization", serialization =>
            {
                serialization.Method(
                    $"public static void SerializeComponent({componentNamespace}.Component component, global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)",
                    m =>
                    {
                        foreach (var fieldDetails in fieldDetailsList)
                        {
                            m.Line(fieldDetails.GetSerializationString($"component.{fieldDetails.PascalCaseName}",
                                "obj", 0));
                        }
                    });

                serialization.Method(
                    $"public static void SerializeUpdate({componentNamespace}.Component component, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)",
                    m =>
                    {
                        m.Line("var obj = updateObj.GetFields();");

                        for (var i = 0; i < fieldDetailsList.Count; i++)
                        {
                            var fieldDetails = fieldDetailsList[i];

                            m.If($"component.IsDataDirty({i})",
                                then =>
                                {
                                    then.Line(fieldDetails.GetSerializationString(
                                        $"component.{fieldDetails.PascalCaseName}", "obj", 0));

                                    if (shouldGenerateClearedFieldsSet)
                                    {
                                        then.Line(fieldDetails.GetTrySetClearedFieldString(
                                            $"component.{fieldDetails.PascalCaseName}", "updateObj", 0));
                                    }
                                });
                        }
                    });

                serialization.Method(
                    $"public static void SerializeUpdate({componentNamespace}.Update update, global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)",
                    m =>
                    {
                        m.Line("var obj = updateObj.GetFields();");

                        foreach (var fieldDetails in fieldDetailsList)
                        {
                            m.CustomScope(cs =>
                            {
                                cs.If($"update.{fieldDetails.PascalCaseName}.HasValue", then =>
                                {
                                    then.Line($"var field = update.{fieldDetails.PascalCaseName}.Value;");
                                    then.Line(fieldDetails.GetSerializationString("field", "obj", 0));

                                    if (shouldGenerateClearedFieldsSet)
                                    {
                                        then.Line(fieldDetails.GetTrySetClearedFieldString("field", "updateObj", 0));
                                    }
                                });
                            });
                        }
                    });

                serialization.Method(
                    $"public static void SerializeSnapshot({componentNamespace}.Snapshot snapshot, global::Improbable.Worker.CInterop.SchemaObject obj)",
                    m =>
                    {
                        foreach (var fieldDetails in fieldDetailsList)
                        {
                            m.Line(fieldDetails.GetSerializationString($"snapshot.{fieldDetails.PascalCaseName}", "obj",
                                0));
                        }
                    });

                serialization.Method(
                    $"public static {componentNamespace}.Component Deserialize(global::Improbable.Worker.CInterop.SchemaObject obj, global::Unity.Entities.World world)",
                    m =>
                    {
                        m.Line($"var component = new {componentNamespace}.Component();");

                        foreach (var fieldDetails in fieldDetailsList)
                        {
                            if (!fieldDetails.IsBlittable)
                            {
                                m.Line(
                                    $"component.{fieldDetails.CamelCaseName}Handle = global::{componentDetails.Namespace}.{componentDetails.Name}.ReferenceTypeProviders.{fieldDetails.PascalCaseName}Provider.Allocate(world);");
                            }

                            m.Line(fieldDetails.GetDeserializeString($"component.{fieldDetails.PascalCaseName}", "obj",
                                0));
                        }

                        m.Return("component");
                    });

                serialization.Method(
                    $"public static {componentNamespace}.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj)",
                    m =>
                    {
                        m.Line(new[]
                        {
                            $"var update = new {componentNamespace}.Update();",
                            "var obj = updateObj.GetFields();"
                        });

                        foreach (var fieldDetails in fieldDetailsList)
                        {
                            m.Line(fieldDetails.GetDeserializeUpdateIntoUpdateString(
                                $"update.{fieldDetails.PascalCaseName}", "obj", 0));
                        }

                        m.Return("update");
                    });

                serialization.Method(
                    $"public static {componentNamespace}.Update DeserializeUpdate(global::Improbable.Worker.CInterop.SchemaComponentData data)",
                    m =>
                    {
                        m.Line(new[]
                        {
                            $"var update = new {componentNamespace}.Update();",
                            "var obj = data.GetFields();"
                        });

                        foreach (var fieldDetails in fieldDetailsList)
                        {
                            m.Line(fieldDetails.GetDeserializeDataIntoUpdateString(
                                $"update.{fieldDetails.PascalCaseName}", "obj", 0));
                        }

                        m.Return("update");
                    });

                serialization.Method(
                    $"public static {componentNamespace}.Snapshot DeserializeSnapshot(global::Improbable.Worker.CInterop.SchemaObject obj)",
                    m =>
                    {
                        m.Line($"var component = new {componentNamespace}.Snapshot();");

                        foreach (var fieldDetails in fieldDetailsList)
                        {
                            m.Line(fieldDetails.GetDeserializeString($"component.{fieldDetails.PascalCaseName}", "obj",
                                0));
                        }

                        m.Return("component");
                    });

                serialization.Method(
                    $"public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref {componentNamespace}.Component component)",
                    m =>
                    {
                        m.Line("var obj = updateObj.GetFields();");

                        foreach (var fieldDetails in fieldDetailsList)
                        {
                            m.Line(fieldDetails.GetDeserializeUpdateString($"component.{fieldDetails.PascalCaseName}",
                                "obj", 0));
                        }
                    });

                serialization.Method(
                    $"public static void ApplyUpdate(global::Improbable.Worker.CInterop.SchemaComponentUpdate updateObj, ref {componentNamespace}.Snapshot snapshot)",
                    m =>
                    {
                        m.Line($"var obj = updateObj.GetFields();");

                        foreach (var fieldDetails in fieldDetailsList)
                        {
                            m.Line(fieldDetails.GetDeserializeUpdateString($"snapshot.{fieldDetails.PascalCaseName}",
                                "obj", 0));
                        }
                    });
            });
        }

        private static TypeBlock GenerateUpdateStruct(UnityComponentDetails componentDetails)
        {
            Logger.Trace($"Generating {componentDetails.Namespace}.{componentDetails.Name}.Update struct.");

            return Scope.Type("public struct Update : ISpatialComponentUpdate",
                update =>
                {
                    update.Line(componentDetails.FieldDetails.Select(fd =>
                        $"public Option<{fd.Type}> {fd.PascalCaseName};").ToList());
                });
        }

        private static TypeBlock GenerateInternalDynamicClass(UnityComponentDetails componentDetails)
        {
            Logger.Trace(
                $"Generating {componentDetails.Namespace}.{componentDetails.Name}.{componentDetails.Name}Dynamic internal class.");

            return Scope.Type($"internal class {componentDetails.Name}Dynamic : IDynamicInvokable",
                dynamic =>
                {
                    dynamic.Line($"public uint ComponentId => {componentDetails.Name}.ComponentId;");

                    dynamic.Line(@"
internal static Dynamic.VTable<Update, Snapshot> VTable = new Dynamic.VTable<Update, Snapshot>
{
    DeserializeSnapshot = DeserializeSnapshot,
    SerializeSnapshot = SerializeSnapshot,
    DeserializeSnapshotRaw = Serialization.DeserializeSnapshot,
    SerializeSnapshotRaw = Serialization.SerializeSnapshot,
    ConvertSnapshotToUpdate = SnapshotToUpdate
};

private static Snapshot DeserializeSnapshot(ComponentData snapshot)
{
    var schemaDataOpt = snapshot.SchemaData;
    if (!schemaDataOpt.HasValue)
    {
        throw new ArgumentException($""Can not deserialize an empty {nameof(ComponentData)}"");
    }

    return Serialization.DeserializeSnapshot(schemaDataOpt.Value.GetFields());
}

private static void SerializeSnapshot(Snapshot snapshot, ComponentData data)
{
    var schemaDataOpt = data.SchemaData;
    if (!schemaDataOpt.HasValue)
    {
        throw new ArgumentException($""Can not serialise an empty {nameof(ComponentData)}"");
    }

    Serialization.SerializeSnapshot(snapshot, data.SchemaData.Value.GetFields());
}
");
                    dynamic.Method("private static Update SnapshotToUpdate(in Snapshot snapshot)", m =>
                    {
                        m.Initializer("var update = new Update", () =>
                        {
                            return componentDetails.FieldDetails.Select(fd =>
                                $"{fd.PascalCaseName} = snapshot.{fd.PascalCaseName}");
                        });

                        m.Return("update");
                    });

                    dynamic.Line(@"
public void InvokeHandler(Dynamic.IHandler handler)
{
    handler.Accept<Update, Snapshot>(ComponentId, VTable);
}
");
                });
        }

        private static bool ShouldGenerateClearedFieldsSet(IEnumerable<UnityFieldDetails> fieldDetailsList)
        {
            return fieldDetailsList.Any(fieldDetails => fieldDetails.CanBeEmpty);
        }

        private static string GetConstructorArgs(IEnumerable<UnityFieldDetails> fieldDetailsList)
        {
            var constructorArgsList =
                fieldDetailsList.Select(fieldDetails => $"{fieldDetails.Type} {fieldDetails.CamelCaseName}");
            return string.Join(", ", constructorArgsList);
        }
    }
}
