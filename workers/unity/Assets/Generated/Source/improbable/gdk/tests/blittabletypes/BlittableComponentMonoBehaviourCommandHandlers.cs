
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Unity.Entities;

namespace Generated.Improbable.Gdk.Tests.BlittableTypes
{
    public partial class BlittableComponent
    {
        public partial class Requirables
        {
            [InjectableId(InjectableType.CommandSender, 1001)]
            internal class CommandSenderCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new CommandSender(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandSender, 1001)]
            [InjectionCondition(InjectionCondition.RequireNothing)]
            public class CommandSender : IInjectable
            {
                public CommandSender(Entity entity, EntityManager entityManager, ILogDispatcher logger)
                {

                }
            }

            [InjectableId(InjectableType.CommandRequestHandler, 1001)]
            internal class CommandRequestHandlerCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new CommandRequestHandler(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandRequestHandler, 1001)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public class CommandRequestHandler : IInjectable
            {
                public CommandRequestHandler(Entity entity, EntityManager entityManager, ILogDispatcher logger)
                {

                }
            }

            [InjectableId(InjectableType.CommandResponseHandler, 1001)]
            internal class CommandResponseHandlerCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new CommandResponseHandler(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandResponseHandler, 1001)]
            [InjectionCondition(InjectionCondition.RequireNothing)]
            public class CommandResponseHandler : IInjectable
            {
                public CommandResponseHandler(Entity entity, EntityManager entityManager, ILogDispatcher logger)
                {

                }
            }
        }
    }
}
