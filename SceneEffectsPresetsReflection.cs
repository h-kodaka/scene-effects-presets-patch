using System;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace SceneEffectsPresetsPatch
{
    internal static class SceneEffectsPresetsReflection
    {
        internal const string OriginalPluginGuid = "com.shallty.SceneEffectsPresets";
        internal const string OriginalTypeName = "SceneEffectsPresets.SceneEffectsPresets";

        private static Type _pluginType;
        private static FieldInfo _windowRectField;
        private static FieldInfo _showMixPresetsField;
        private static MethodInfo _awakeMethod;
        private static MethodInfo _onGuiMethod;
        private static FieldInfo _toggleUiField;
        private static bool _resolved;

        internal static bool IsOriginalPresent => AccessTools.TypeByName(OriginalTypeName) != null;

        internal static MethodInfo AwakeMethod
        {
            get
            {
                EnsureResolved();
                return _awakeMethod;
            }
        }

        internal static MethodInfo OnGuiMethod
        {
            get
            {
                EnsureResolved();
                return _onGuiMethod;
            }
        }

        internal static bool EnsureResolved()
        {
            if (_resolved)
                return _pluginType != null && _windowRectField != null;

            _resolved = true;
            _pluginType = AccessTools.TypeByName(OriginalTypeName);
            if (_pluginType == null)
                return false;

            _windowRectField = AccessTools.Field(_pluginType, "windowRect");
            _showMixPresetsField = AccessTools.Field(_pluginType, "showMixPresets");
            _toggleUiField = AccessTools.Field(_pluginType, "toggleUI");
            _awakeMethod = AccessTools.Method(_pluginType, "Awake");
            _onGuiMethod = AccessTools.Method(_pluginType, "OnGUI");

            return _windowRectField != null
                && _showMixPresetsField != null
                && _toggleUiField != null
                && _awakeMethod != null
                && _onGuiMethod != null;
        }

        internal static Rect GetWindowRect()
        {
            return (Rect)_windowRectField.GetValue(null);
        }

        internal static void SetWindowRect(Rect rect)
        {
            _windowRectField.SetValue(null, rect);
        }

        internal static bool GetShowMixPresets()
        {
            return (bool)_showMixPresetsField.GetValue(null);
        }

        internal static void SetShowMixPresets(bool value)
        {
            _showMixPresetsField.SetValue(null, value);
        }

        internal static bool GetToggleUi()
        {
            return (bool)_toggleUiField.GetValue(null);
        }
    }
}
