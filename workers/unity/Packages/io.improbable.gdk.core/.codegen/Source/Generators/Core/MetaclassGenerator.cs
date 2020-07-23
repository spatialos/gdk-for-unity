using System.Linq;
using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.CodeWriter.Scopes;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public static class MetaclassGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static CodeWriter Generate(UnityComponentDetails componentDetails)
        {
            var commandDetailsList = componentDetails.CommandDetails;
            var rootNamespace = $"global::{componentDetails.Namespace}.{componentDetails.Name}";

            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "System",
                    "Improbable.Gdk.Core",
                    "Improbable.Gdk.Core.Commands"
                );

                cgw.Namespace(componentDetails.Namespace, ns =>
                {
                    ns.Type($"public partial class {componentDetails.Name}", partial =>
                    {
                        Logger.Trace($"Generating {componentDetails.Namespace}.{componentDetails.Name}.ComponentMetaclass class.");

                        partial.Type("public class ComponentMetaclass : IComponentMetaclass", componentMetaclass =>
                        {
                            componentMetaclass.Line($@"
public uint ComponentId => {componentDetails.ComponentId};
public string Name => ""{componentDetails.Name}"";

public Type Data {{ get; }} = typeof({rootNamespace}.Component);
public Type Authority {{ get; }} = typeof({rootNamespace}.HasAuthority);
public Type Snapshot {{ get; }} = typeof({rootNamespace}.Snapshot);
public Type Update {{ get; }} = typeof({rootNamespace}.Update);

public Type ReplicationHandler {{ get; }} = typeof({rootNamespace}.ComponentReplicator);
public Type Serializer {{ get; }} = typeof({rootNamespace}.ComponentSerializer);
public Type DiffDeserializer {{ get; }} = typeof({rootNamespace}.DiffComponentDeserializer);

public Type DiffStorage {{ get; }} = typeof({rootNamespace}.DiffComponentStorage);
public Type ViewStorage {{ get; }} = typeof({rootNamespace}.{componentDetails.Name}ViewStorage);
public Type EcsViewManager {{ get; }} = typeof({rootNamespace}.EcsViewManager);
public Type DynamicInvokable {{ get; }} = typeof({rootNamespace}.{componentDetails.Name}Dynamic);
");

                            componentMetaclass.Initializer("public ICommandMetaclass[] Commands { get; } = new ICommandMetaclass[]",
                                () =>
                                {
                                    return commandDetailsList.Select(command => $"new {command.PascalCaseName}Metaclass()");
                                });
                        });

                        foreach (var command in commandDetailsList)
                        {
                            partial.Type(GenerateCommandMetaclass(componentDetails.Namespace, componentDetails.Name, command));
                        }
                    });
                });
            });
        }

        private static TypeBlock GenerateCommandMetaclass(string qualifiedNamespace, string componentName, UnityCommandDetails command)
        {
            Logger.Trace($"Generating {qualifiedNamespace}.{componentName}.{command.PascalCaseName}Metaclass class.");

            var rootNamespace = $"global::{qualifiedNamespace}.{componentName}";

            return Scope.Type($"public class {command.PascalCaseName}Metaclass : ICommandMetaclass",
                commandMetaclass =>
                {
                    commandMetaclass.Line($@"
public uint ComponentId => {rootNamespace}.ComponentId;
public uint CommandIndex => {command.CommandIndex};
public string Name => ""{command.PascalCaseName}"";

public Type DiffDeserializer {{ get; }} = typeof({rootNamespace}.{command.PascalCaseName}DiffCommandDeserializer);
public Type Serializer {{ get; }} = typeof({rootNamespace}.{command.PascalCaseName}CommandSerializer);

public Type MetaDataStorage {{ get; }} = typeof({rootNamespace}.{command.PascalCaseName}CommandMetaDataStorage);
public Type SendStorage {{ get; }} = typeof({rootNamespace}.{command.PascalCaseName}CommandsToSendStorage);
public Type DiffStorage {{ get; }} = typeof({rootNamespace}.Diff{command.PascalCaseName}CommandStorage);

public Type Response {{ get; }} = typeof({rootNamespace}.{command.PascalCaseName}.Response);
public Type ReceivedResponse {{ get; }} = typeof({rootNamespace}.{command.PascalCaseName}.ReceivedResponse);
public Type Request {{ get; }} = typeof({rootNamespace}.{command.PascalCaseName}.Request);
public Type ReceivedRequest {{ get; }} = typeof({rootNamespace}.{command.PascalCaseName}.ReceivedRequest);
");
                });
        }
    }
}
