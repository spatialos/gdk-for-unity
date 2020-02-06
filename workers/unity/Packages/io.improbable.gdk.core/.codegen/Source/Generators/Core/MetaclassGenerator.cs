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

        public static string Generate(UnityComponentDetails details, string package)
        {
            var qualifiedNamespace = package;

            var componentDetails = details;
            var commandDetailsList = details.CommandDetails;
            var rootNamespace = $"global::{qualifiedNamespace}.{componentDetails.Name}";

            var writer = CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "System",
                    "Improbable.Gdk.Core",
                    "Improbable.Gdk.Core.Commands"
                );

                cgw.Namespace(qualifiedNamespace, ns =>
                {
                    ns.Type($"public partial class {componentDetails.Name}", partial =>
                    {
                        Logger.Trace($"Generating {qualifiedNamespace}.{componentDetails.Name}.ComponentMetaclass class.");

                        partial.Type("public class ComponentMetaclass : IComponentMetaclass", componentMetaclass =>
                        {
                            componentMetaclass.Line($@"
public uint ComponentId => {componentDetails.ComponentId};
public string Name => ""{componentDetails.Name}"";

public Type Data {{ get; }} = typeof({rootNamespace}.Component);
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
                                    return commandDetailsList.Select(command => $"new {command.Name}Metaclass()");
                                });
                        });

                        foreach (var command in commandDetailsList)
                        {
                            partial.Type(GenerateCommandMetaclass(qualifiedNamespace, componentDetails.Name, command));
                        }
                    });
                });
            });

            return writer.Format();
        }

        private static TypeBlock GenerateCommandMetaclass(string qualifiedNamespace, string componentName, UnityCommandDetails command)
        {
            Logger.Trace($"Generating {qualifiedNamespace}.{componentName}.{command.Name}Metaclass class.");

            var rootNamespace = $"global::{qualifiedNamespace}.{componentName}";

            return Scope.Type($"public class {command.Name}Metaclass : ICommandMetaclass",
                commandMetaclass =>
                {
                    commandMetaclass.Line($@"
public uint CommandIndex => {command.CommandIndex};
public string Name => ""{command.Name}"";

public Type DiffDeserializer {{ get; }} = typeof({rootNamespace}.{command.Name}DiffCommandDeserializer);
public Type Serializer {{ get; }} = typeof({rootNamespace}.{command.Name}CommandSerializer);

public Type MetaDataStorage {{ get; }} = typeof({rootNamespace}.{command.Name}CommandMetaDataStorage);
public Type SendStorage {{ get; }} = typeof({rootNamespace}.{command.Name}CommandsToSendStorage);
public Type DiffStorage {{ get; }} = typeof({rootNamespace}.Diff{command.Name}CommandStorage);
");
                });
        }
    }
}
