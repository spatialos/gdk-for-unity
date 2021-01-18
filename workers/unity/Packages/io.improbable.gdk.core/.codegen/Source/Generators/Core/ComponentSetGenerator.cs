using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.CodeGeneration.CodeWriter;
using Improbable.Gdk.CodeGeneration.Model.Details;
using NLog;

namespace Improbable.Gdk.CodeGenerator
{
    public static class ComponentSetGenerator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static CodeWriter Generate(IReadOnlyList<ComponentSetDetails> componentSets)
        {
            return CodeWriter.Populate(cgw =>
            {
                cgw.UsingDirectives(
                    "Improbable.Gdk.Core",
                    "System.Collections.Generic"
                );

                cgw.Namespace("Improbable.Generated", ns =>
                {
                    ns.Type("public class ComponentSets : IComponentSetManager", tb =>
                    {
                        foreach (var set in componentSets)
                        {
                            tb.Line(
                                $"public static readonly ComponentSet {set.Name} = new ComponentSet({set.ComponentSetId}, new uint[] {{ {string.Join(", ", set.ComponentIdReferences)} }});");
                        }

                        tb.Initializer("private static readonly IReadOnlyDictionary<uint, ComponentSet> keyedComponentSets = new Dictionary<uint, ComponentSet>", () => componentSets.Select(set => $"{{ {set.Name}.ComponentSetId, {set.Name} }}"));

                        tb.Method("public bool TryGetComponentSet(uint componentSetId, out ComponentSet componentSet)",
                            mb =>
                            {
                                mb.Line("return keyedComponentSets.TryGetValue(componentSetId, out componentSet);");
                            });
                    });
                });
            });
        }
    }
}
