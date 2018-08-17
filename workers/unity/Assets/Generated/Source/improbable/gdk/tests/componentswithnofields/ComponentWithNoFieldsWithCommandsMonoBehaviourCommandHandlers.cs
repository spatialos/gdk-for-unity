
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Unity.Entities;

namespace Generated.Improbable.Gdk.Tests.ComponentsWithNoFields
{
    public partial class ComponentWithNoFieldsWithCommands
    {
        public partial class Accessors
        {
            internal class CmdCommandSenderCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new CmdCommandSender(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandSender, 1005, 0)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public class CmdCommandSender : IInjectable
            {
                public CmdCommandSender(Entity entity, EntityManager entityManager, ILogDispatcher logger)
                {

                }
            }

            internal class CmdCommandRequestHandlerCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new CmdCommandRequestHandler(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandRequestHandler, 1005, 0)]
            [InjectionCondition(InjectionCondition.RequireNothing)]
            public class CmdCommandRequestHandler : IInjectable
            {
                public CmdCommandRequestHandler(Entity entity, EntityManager entityManager, ILogDispatcher logger)
                {

                }
            }

            internal class CmdCommandResponseHandlerCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new CmdCommandResponseHandler(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandResponseHandler, 1005, 0)]
            [InjectionCondition(InjectionCondition.RequireNothing)]
            public class CmdCommandResponseHandler : IInjectable
            {
                public CmdCommandResponseHandler(Entity entity, EntityManager entityManager, ILogDispatcher logger)
                {

                }
            }

        }
    }
}
