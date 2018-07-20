using Generated.Playground;
using Improbable.Gdk.Core.MonoBehaviours;
using NUnit.Framework;
using Unity.Entities;

namespace Improbable.Gdk.Core.EditmodeTests
{
    [TestFixture]
    public class ReaderBaseTests
    {
        private struct TestComponentData : IComponentData, ISpatialComponentData
        {
            public BlittableBool DirtyBit { get; set; }


            public struct Update : ISpatialComponentUpdate<TestComponentData>
            {
                public Option<float> Horizontal;
                public Option<float> Vertical;
                public Option<BlittableBool> Running;
            }

            public class Reader : ReaderBase<TestComponentData, Update>
            {
                public Reader(Entity entity, EntityManager manager) : base(entity, manager)
                {
                }
            }
        }
    }
}
