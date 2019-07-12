using System;
using System.Linq;
using NUnit.Framework;
using Unity.Entities;

namespace Improbable.Gdk.Core.EditmodeTests
{
    [TestFixture]
    public class DisableAutoCreationSystemsTests
    {
        private static readonly string[] Assemblies =
        {
            "Improbable.Gdk.Core",
            "Improbable.Gdk.TransformSynchronization",
            "Improbable.Gdk.PlayerLifecycle",
        };

        private static readonly string[] WhiteListedSystems = { };

        [Test]
        public void systems_should_have_DisableAutoCreation_tag()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies().Where(t =>
                Assemblies.Contains(t.GetName().Name)).ToList();
            Assert.AreEqual(assemblies.Count(), Assemblies.Length, "Not all specified assemblies were found.");

            foreach (var assembly in assemblies)
            {
                var systemTypes = assembly.GetTypes().Where(t =>
                        t.IsSubclassOf(typeof(ComponentSystemBase)) &&
                        !WhiteListedSystems.ToList().Contains(t.Name) &&
                        t.GetCustomAttributes(typeof(DisableAutoCreationAttribute), true).Length == 0)
                    .Select(t => t.Name);

                Assert.AreEqual(systemTypes.Count(), 0,
                    $"The following systems don't have the DisableAutoCreation attribute: {string.Join(", ", systemTypes)}");
            }
        }
    }
}
