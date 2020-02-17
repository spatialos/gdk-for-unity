using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public static class UnityEventGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static CodeWriter Generate(UnityComponentDetails details)
        {
            var componentName = details.Name;

            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "System.Collections.Generic",
                    "Improbable.Gdk.Core",
                    "Improbable.Worker",
                    "Unity.Entities"
                );

                cgw.Namespace(details.Namespace, ns =>
                {
                    ns.Type($"public partial class {componentName}", partial =>
                    {
                        foreach (var eventDetail in details.EventDetails)
                        {
                            var eventName = eventDetail.PascalCaseName;
                            var payloadType = eventDetail.FqnPayloadType;

                            Logger.Trace($"Generating {details.Namespace}.{componentName}.{eventName} class.");
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
            });
        }
    }
}
