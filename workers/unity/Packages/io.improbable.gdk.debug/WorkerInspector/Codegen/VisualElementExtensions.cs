using UnityEngine.UIElements;

namespace Improbable.Gdk.Debug.WorkerInspector.Codegen
{
    public static class VisualElementExtensions
    {
        public static void ShiftRightMargin(this VisualElement element, uint nest, float offset = -11)
        {
            var style = element.style;
            var val = style.marginRight.value.value;
            style.marginRight = new StyleLength(val + offset * nest);
        }
    }
}
