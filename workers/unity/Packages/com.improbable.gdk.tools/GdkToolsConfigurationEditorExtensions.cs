using UnityEditor;

namespace Improbable.Gdk.Tools
{
    /// <summary>
    ///     An editor window that allows you to configure the GDK Tools.
    ///     Adds a menu item to Improbable/Configure Tools which opens this menu.
    /// </summary>
    public class GdkToolsConfigurationWindow : EditorWindow
    {
        private const string WindowTitle = "Gdk Tools Config";
        private const string MenuItemTitle = "Improbable/Configure Tools";
        private const int MenuItemPriority = 70;

        private ScriptableGdkToolsConfiguration toolsConfig;

        [MenuItem(MenuItemTitle, false, MenuItemPriority)]
        public static void ShowWindow()
        {
            GetWindow<GdkToolsConfigurationWindow>(false, WindowTitle, true);
        }

        public void OnGUI()
        {
            OneTimeInit();
            toolsConfig.OnGUI();
        }

        private void OneTimeInit()
        {
            if (toolsConfig != null)
            {
                return;
            }

            toolsConfig = ScriptableGdkToolsConfiguration.GetOrCreateInstance();
        }
    }

    /// <summary>
    ///     Defines a custom inspector window that allows you to configure the GDK Tools.
    /// </summary>
    [CustomEditor(typeof(ScriptableGdkToolsConfiguration))]
    public class GdkToolsConfigurationInspector : Editor
    {
        private ScriptableGdkToolsConfiguration toolsConfig;

        private void OnEnable()
        {
            if (toolsConfig != null)
            {
                return;
            }

            toolsConfig = ScriptableGdkToolsConfiguration.GetOrCreateInstance();
        }

        public override void OnInspectorGUI()
        {
            toolsConfig.OnGUI();
        }
    }
}


