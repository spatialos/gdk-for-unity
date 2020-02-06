using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public static class UnityEventGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static string Generate(UnityComponentDetails details, string package)
        {
            var qualifiedNamespace = package;
            var componentName = details.Name;

            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "System.Collections.Generic",
                    "Improbable.Gdk.Core",
                    "Improbable.Worker",
                    "Unity.Entities"
                );

                cgw.Namespace(qualifiedNamespace, ns =>
                {
                    ns.Type($"public partial class {componentName}", partial =>
                    {
                        foreach (var eventDetail in details.EventDetails)
                        {
                            var eventName = eventDetail.Name;
                            var payloadType = eventDetail.FqnPayloadType;

                            Logger.Trace($"Generating {qualifiedNamespace}.{componentName}.{eventName} class.");
                            partial.Line($@"
public static class {eventName}
{{
    public readonly struct Event : IEvent
    {{
        public readonly {payloadType} Payload;

        public Event({payloadType} payload)
        {{
            Payload = payload;
        }}
    }}
}}
");
                        }
                    });
                });
            }).Format();
        }
    }
}
