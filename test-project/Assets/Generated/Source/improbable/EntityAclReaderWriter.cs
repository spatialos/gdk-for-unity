
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using System;
using System.Collections.Generic;
using Unity.Entities;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Entity = Unity.Entities.Entity;

namespace Generated.Improbable
{
    public partial class EntityAcl
    {
        public partial class Requirables
        {
            [InjectableId(InjectableType.ReaderWriter, 50)]
            internal class ReaderWriterCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new ReaderWriterImpl(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.ReaderWriter, 50)]
            [InjectionCondition(InjectionCondition.RequireComponentPresent)]
            public interface Reader : IReader<Generated.Improbable.EntityAcl.Component, Generated.Improbable.EntityAcl.Update>
            {
                event Action<global::Generated.Improbable.WorkerRequirementSet> ReadAclUpdated;
                event Action<global::System.Collections.Generic.Dictionary<uint,global::Generated.Improbable.WorkerRequirementSet>> ComponentWriteAclUpdated;
            }

            [InjectableId(InjectableType.ReaderWriter, 50)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public interface Writer : IWriter<Generated.Improbable.EntityAcl.Component, Generated.Improbable.EntityAcl.Update>
            {
            }

            internal class ReaderWriterImpl :
                ReaderWriterBase<Generated.Improbable.EntityAcl.Component, Generated.Improbable.EntityAcl.Update>, Reader, Writer
            {
                public ReaderWriterImpl(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                    : base(entity, entityManager, logDispatcher)
                {
                }

                private readonly List<Action<global::Generated.Improbable.WorkerRequirementSet>> readAclDelegates = new List<Action<global::Generated.Improbable.WorkerRequirementSet>>();

                public event Action<global::Generated.Improbable.WorkerRequirementSet> ReadAclUpdated
                {
                    add
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        readAclDelegates.Add(value);
                    }
                    remove
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        readAclDelegates.Remove(value);
                    }
                }

                private readonly List<Action<global::System.Collections.Generic.Dictionary<uint,global::Generated.Improbable.WorkerRequirementSet>>> componentWriteAclDelegates = new List<Action<global::System.Collections.Generic.Dictionary<uint,global::Generated.Improbable.WorkerRequirementSet>>>();

                public event Action<global::System.Collections.Generic.Dictionary<uint,global::Generated.Improbable.WorkerRequirementSet>> ComponentWriteAclUpdated
                {
                    add
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        componentWriteAclDelegates.Add(value);
                    }
                    remove
                    {
                        if (!VerifyNotDisposed())
                        {
                            return;
                        }

                        componentWriteAclDelegates.Remove(value);
                    }
                }

                protected override void TriggerFieldCallbacks(Generated.Improbable.EntityAcl.Update update)
                {
                    DispatchWithErrorHandling(update.ReadAcl, readAclDelegates);
                    DispatchWithErrorHandling(update.ComponentWriteAcl, componentWriteAclDelegates);
                }

                protected override void ApplyUpdate(Generated.Improbable.EntityAcl.Update update, ref Generated.Improbable.EntityAcl.Component data)
                {
                    if (update.ReadAcl.HasValue)
                    {
                        data.ReadAcl = update.ReadAcl.Value;
                    }
                    if (update.ComponentWriteAcl.HasValue)
                    {
                        data.ComponentWriteAcl = update.ComponentWriteAcl.Value;
                    }
                }
            }
        }
    }
}
