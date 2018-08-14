using System;
using System.Collections.Generic;
using System.Linq;
using Improbable.Gdk.Core.Components;
using Improbable.Worker;
using Unity.Entities;
using UnityEngine;
using Entity = Unity.Entities.Entity;

namespace Improbable.Gdk.Core
{
    // section of stuff that will be removed soon.
    // leaving it here until Jamie is done with his refactoring :P
    public class TranslationUnityRegistry : IDisposable
    {
        public static Dictionary<World, TranslationUnityRegistry> WorldToTranslationUnit = 
            new Dictionary<World, TranslationUnityRegistry>();
        
        public readonly Dictionary<int, ComponentTranslation> TranslationUnits =
            new Dictionary<int, ComponentTranslation>();

        public Action<Entity, long> AddAllCommandRequestSenders;

        private World world;

        public TranslationUnityRegistry(Worker worker)
        {
            this.world = worker.World;
            var translationTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(ComponentTranslation).IsAssignableFrom(type) && !type.IsAbstract).ToList();

            foreach (var translationType in translationTypes)
            {
                var translator = (ComponentTranslation)Activator.CreateInstance(translationType, worker);
                TranslationUnits.Add(translator.TargetComponentType.TypeIndex, translator);

                AddAllCommandRequestSenders += translator.AddCommandRequestSender;
            }
            
            WorldToTranslationUnit.Add(world, this);
        }
        
        public void Dispose()
        {
            foreach (var translation in TranslationUnits.Values)
            {
                translation.Dispose();
            }
            
            WorldToTranslationUnit.Remove(world);

            world = null;
        }
    }
}
