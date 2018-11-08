using System.Collections.Generic;
using System.Linq;
using Improbable.CodeGeneration.FileHandling;
using Improbable.CodeGeneration.Jobs;
using Improbable.CodeGeneration.Utils;

namespace Improbable.Gdk.CodeGenerator
{
    public class CommandSendingSytemJob : CodegenJob
    {
        private const string GeneratedFileName = "CommandSendingSystem.cs";

        private readonly List<CommandGroupInfo> commandGroupInfo;
        
        public CommandSendingSytemJob(string outputDirectory, IFileSystem fileSystem, List<UnitySchemaFile> schemaFiles) : base(outputDirectory, fileSystem)
        {
            InputFiles = schemaFiles.Select(schemaFile => schemaFile.CompletePath).ToList();
            OutputFiles = new List<string> {GeneratedFileName};
            
            commandGroupInfo = new List<CommandGroupInfo>();

            foreach (var schemaFile in schemaFiles)
            {
                var qualifiedNamespace = Formatting.CapitaliseQualifiedNameParts(schemaFile.Package);

                foreach (var component in schemaFile.ComponentDefinitions)
                {
                    if (component.CommandDefinitions.Count > 0)
                    {
                        commandGroupInfo.Add(new CommandGroupInfo
                        {
                            QualifiedNamespace = qualifiedNamespace + "." + Formatting.QualifiedNameToCapitalisedCamelCase(component.Name),
                            CommandDefinitions =  component.CommandDefinitions
                        });
                    }
                }
            }
        }

        protected override void RunImpl()
        {
            var generator = new UnityCommandSendingSystemGenerator();
            var sendingCode = generator.Generate(commandGroupInfo);
            Content.Add(GeneratedFileName, sendingCode);
        }
    }
}