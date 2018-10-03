using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Improbable.Worker.Core;
using UnityEngine;

namespace Improbable.Gdk.Core.EditmodeTests
{
    [TestFixture]
    public class EntityBuilderTest
    {
        [Test]
        public void EntityBuilder_should_throw_exception_if_no_position_component_is_added()
        {
            var builder = EntityBuilder.Begin();
            Assert.Throws<InvalidEntityException>(() => builder.Build());
        }

        [Test]
        public void EntityBuilder_should_throw_exception_if_component_is_added_twice()
        {
            var builder = EntityBuilder.Begin();
            builder.AddPosition(0, 0, 0, "write-access");
            Assert.Throws<InvalidOperationException>(() => builder.AddPosition(0, 0, 0, "write-access"));

            DisposeEntityTemplate(builder.Build());
        }

        [Test]
        public void EntityBuilder_should_throw_exception_if_Build_called_more_than_once()
        {
            var builder = EntityBuilder.Begin();
            builder.AddPosition(0, 0, 0, "write-access");
            var template = builder.Build();
            Assert.Throws<InvalidOperationException>(() => builder.Build());

            DisposeEntityTemplate(template);
        }

        [Test]
        public void EntityBuilder_should_Build_if_only_position_is_added()
        {
            var builder = EntityBuilder.Begin();
            builder.AddPosition(0, 0, 0, "write-access");

            var template = new EntityTemplate(new Entity()); // Stops IDE complaining about null check below.

            // Wrap in try - finally to ensure dispose gets called even if the assert fails.
            try
            {
                Assert.DoesNotThrow(() => template = builder.Build());
            }
            finally
            {
                DisposeEntityTemplate(template);
            }
        }

        [Test]
        public void EntityBuilder_should_Build_if_arbitrary_components_are_added()
        {
            var builder = EntityBuilder.Begin();
            builder.AddPosition(0, 0, 0, "write-access");
            builder.AddComponent(GetComponentDataWithId(1000), "write-access");
            builder.AddComponent(GetComponentDataWithId(1001), "write-access");
            var template = new EntityTemplate(new Entity()); // Stops IDE complaining about null check below.

            // Wrap in try - finally to ensure dispose gets called even if the assert fails.
            try
            {
                Assert.DoesNotThrow(() => template = builder.Build());
            }
            finally
            {
                DisposeEntityTemplate(template);
            }
        }

        [Test]
        public void EntityTemplate_should_throw_if_used_twice()
        {
            var builder = EntityBuilder.Begin();
            builder.AddPosition(0, 0, 0, "write-access");

            var template = builder.Build();
            template.GetEntity();

            // Wrap in try - finally to ensure dispose gets called even if the assert fails.
            try
            {
                Assert.Throws<InvalidOperationException>(() => template.GetEntity());
            }
            finally
            {
                DisposeEntityTemplate(template);
            }
        }

        private ComponentData GetComponentDataWithId(uint componentId)
        {
            var schemaComponentData = new SchemaComponentData(componentId);
            return new ComponentData(schemaComponentData);
        }

        /// <summary>
        ///     Disposes the underlying SchemaComponentData in native memory for a given Entity.
        ///     If the memory is not manually disposed, it will cause a leak!
        /// </summary>
        private void DisposeEntityTemplate(EntityTemplate template)
        {
            var entity = template.template;
            var componentIds = entity.GetComponentIds();

            foreach (var id in componentIds)
            {
                var componentData = entity.Get(id);
                componentData.Value.SchemaData.Value.Dispose();
            }
        }
    }
}
