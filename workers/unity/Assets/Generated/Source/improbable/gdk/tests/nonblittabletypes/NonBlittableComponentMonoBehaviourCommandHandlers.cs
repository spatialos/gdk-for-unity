
// ===========
// DO NOT EDIT - this file is automatically regenerated.
// ===========

using Improbable.Gdk.Core;
using Improbable.Gdk.Core.GameObjectRepresentation;
using Unity.Entities;

namespace Generated.Improbable.Gdk.Tests.NonblittableTypes
{
    public partial class NonBlittableComponent
    {
        public partial class Accessors
        {
            internal class FirstCommandCommandSenderCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new FirstCommandCommandSender(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandSender, 1002, 0)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public class FirstCommandCommandSender : IInjectable
            {
                public FirstCommandCommandSender(Entity entity, EntityManager entityManager, ILogDispatcher logger)
                {

                }
            }

            internal class FirstCommandCommandRequestHandlerCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new FirstCommandCommandRequestHandler(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandRequestHandler, 1002, 0)]
            [InjectionCondition(InjectionCondition.RequireNothing)]
            public class FirstCommandCommandRequestHandler : IInjectable
            {
                public FirstCommandCommandRequestHandler(Entity entity, EntityManager entityManager, ILogDispatcher logger)
                {

                }
            }

            internal class FirstCommandCommandResponseHandlerCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new FirstCommandCommandResponseHandler(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandResponseHandler, 1002, 0)]
            [InjectionCondition(InjectionCondition.RequireNothing)]
            public class FirstCommandCommandResponseHandler : IInjectable
            {
                public FirstCommandCommandResponseHandler(Entity entity, EntityManager entityManager, ILogDispatcher logger)
                {

                }
            }

            internal class SecondCommandCommandSenderCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new SecondCommandCommandSender(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandSender, 1002, 1)]
            [InjectionCondition(InjectionCondition.RequireComponentWithAuthority)]
            public class SecondCommandCommandSender : IInjectable
            {
                public SecondCommandCommandSender(Entity entity, EntityManager entityManager, ILogDispatcher logger)
                {

                }
            }

            internal class SecondCommandCommandRequestHandlerCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new SecondCommandCommandRequestHandler(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandRequestHandler, 1002, 1)]
            [InjectionCondition(InjectionCondition.RequireNothing)]
            public class SecondCommandCommandRequestHandler : IInjectable
            {
                public SecondCommandCommandRequestHandler(Entity entity, EntityManager entityManager, ILogDispatcher logger)
                {

                }
            }

            internal class SecondCommandCommandResponseHandlerCreator : IInjectableCreator
            {
                public IInjectable CreateInjectable(Entity entity, EntityManager entityManager, ILogDispatcher logDispatcher)
                {
                    return new SecondCommandCommandResponseHandler(entity, entityManager, logDispatcher);
                }
            }

            [InjectableId(InjectableType.CommandResponseHandler, 1002, 1)]
            [InjectionCondition(InjectionCondition.RequireNothing)]
            public class SecondCommandCommandResponseHandler : IInjectable
            {
                public SecondCommandCommandResponseHandler(Entity entity, EntityManager entityManager, ILogDispatcher logger)
                {

                }
            }

        }
    }
}
