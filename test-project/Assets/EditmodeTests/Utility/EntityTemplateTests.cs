using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Tests;
using Improbable.Worker.CInterop;
using NUnit.Framework;

namespace Improbable.Gdk.EditmodeTests.Utility
{
    [TestFixture]
    public class EntityTemplateTests
    {
        [Test]
        public void AddComponent_should_throw_if_duplicate_component_is_added()
        {
            var template = GetBasicTemplate();
            Assert.Throws<InvalidOperationException>(() =>
            {
                template.AddComponent(new Position.Snapshot(), "write-acesss");
            });
        }

        [Test]
        public void AddComponent_should_ignore_EntityAcl()
        {
            var template = GetBasicTemplate();
            template.AddComponent(new EntityAcl.Snapshot(), "test");

            Assert.IsFalse(template.HasComponent<EntityAcl.Snapshot>());
        }

        [Test]
        public void GetComponent_should_return_the_component_if_present()
        {
            var template = GetBasicTemplate();

            var exhaustiveSingular = new ExhaustiveSingular.Snapshot
            {
                Field2 = 100 // Field to test equality for.
            };

            template.AddComponent(exhaustiveSingular, "");
            var returned = template.GetComponent<ExhaustiveSingular.Snapshot>();
            Assert.IsTrue(returned.HasValue);
            Assert.AreEqual(exhaustiveSingular.Field2, returned.Value.Field2);
        }

        [Test]
        public void GetComponent_should_return_null_if_component_not_present()
        {
            var template = GetBasicTemplate();
            var returned = template.GetComponent<ExhaustiveSingular.Snapshot>();
            Assert.IsFalse(returned.HasValue);
        }

        [Test]
        public void HasComponent_should_return_false_if_component_not_present()
        {
            var template = GetBasicTemplate();
            Assert.IsFalse(template.HasComponent<ExhaustiveSingular.Snapshot>());
        }

        [Test]
        public void HasComponent_should_return_true_if_component_present()
        {
            var exhaustiveSingular = new ExhaustiveSingular.Snapshot
            {
                Field7 = "",
                Field3 = new byte[] { }
            };

            var template = GetBasicTemplate();
            template.AddComponent(exhaustiveSingular, "write-access");

            Assert.IsTrue(template.HasComponent<ExhaustiveSingular.Snapshot>());
        }

        [Test]
        public void ReplaceComponent_should_replace_the_underlying_component()
        {
            var originalSnapshot = new ExhaustiveSingular.Snapshot
            {
                Field7 = "",
                Field3 = new byte[] { }
            };

            var template = GetBasicTemplate();
            template.AddComponent(originalSnapshot, "");

            var snapshotToReplace = new ExhaustiveSingular.Snapshot
            {
                Field2 = 100 // Field to test equality for.
            };

            template.SetComponent(snapshotToReplace);

            var component = template.GetComponent<ExhaustiveSingular.Snapshot>().Value;
            Assert.AreEqual(snapshotToReplace.Field2, component.Field2);
        }

        [Test]
        public void RemoveComponent_should_remove_component_if_present()
        {
            var exhaustiveSingular = new ExhaustiveSingular.Snapshot
            {
                Field7 = "",
                Field3 = new byte[] { }
            };

            var template = GetBasicTemplate();
            template.AddComponent(exhaustiveSingular, "");
            template.RemoveComponent<ExhaustiveSingular.Snapshot>();

            Assert.IsFalse(template.GetComponent<ExhaustiveSingular.Snapshot>().HasValue);
        }

        [Test]
        public void GetEntity_should_throw_exception_if_no_position_component_is_added()
        {
            var template = new EntityTemplate();
            Assert.IsTrue(DidGetEntityThrow(template));
        }

        [Test]
        public void GetEntity_should_not_throw_if_only_position_is_added()
        {
            var template = GetBasicTemplate();
            Assert.IsFalse(DidGetEntityThrow(template));
        }

        [Test]
        public void GetEntity_should_not_throw_with_arbritrary_components()
        {
            var exhaustiveSingular = new ExhaustiveSingular.Snapshot
            {
                Field7 = "",
                Field3 = new byte[] { }
            };

            var template = GetBasicTemplate();
            template.AddComponent(exhaustiveSingular, "write-access");

            Assert.IsFalse(DidGetEntityThrow(template));
        }

        // Helper method to ensure that underlying allocated data is disposed properly.
        private bool DidGetEntityThrow(EntityTemplate template)
        {
            Entity entity = null;
            try
            {
                entity = template.GetEntity();
            }
            catch (InvalidOperationException)
            {
                return true;
            }
            finally
            {
                if (entity != null)
                {
                    foreach (var id in entity.GetComponentIds())
                    {
                        var data = entity.Get(id);
                        data.Value.SchemaData.Value.Destroy();
                    }
                }
            }

            return false;
        }

        private EntityTemplate GetBasicTemplate()
        {
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(), "write-acesss");
            return template;
        }
    }
}
