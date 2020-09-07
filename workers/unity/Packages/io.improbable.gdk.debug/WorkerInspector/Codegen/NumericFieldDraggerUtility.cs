using System;
using System.Linq;
using System.Reflection;
using UnityEditor.UIElements;
using UnityEngine;

namespace Improbable.Gdk.Debug.WorkerInspector.Codegen
{
    internal static class NumericFieldDraggerUtility
    {
        static NumericFieldDraggerUtility()
        {
            var methods =
                typeof(IntegerField)
                    .Assembly
                    .GetTypes()
                    .First(type => type.Name == "NumericFieldDraggerUtility")
                    .GetMethods(BindingFlags.Static | BindingFlags.NonPublic);

            var accelerateMethod = methods.First(method => method.Name == "Acceleration");
            Acceleration = (Func<bool, bool, float>) Delegate.CreateDelegate(typeof(Func<bool, bool, float>), null, accelerateMethod);

            var niceDeltaMethod = methods.First(method => method.Name == "NiceDelta");
            NiceDelta = (Func<Vector2, float, float>) Delegate.CreateDelegate(typeof(Func<bool, bool, float>), null, niceDeltaMethod);

            var calculateIntDragSensitivityMethod = methods.First(method => method.Name == "CalculateIntDragSensitivity");
            CalculateIntDragSensitivity = (Func<long, long>) Delegate.CreateDelegate(typeof(Func<bool, bool, float>), null, calculateIntDragSensitivityMethod);
        }

        public static Func<bool, bool, float> Acceleration;
        public static Func<Vector2, float, float> NiceDelta;
        public static Func<long, long> CalculateIntDragSensitivity;
    }
}
