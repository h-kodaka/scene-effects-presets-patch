using System;
using KKAPI.Studio;
using KKAPI.Studio.UI.Toolbars;
using KKAPI.Utilities;
using UnityEngine;

namespace SceneEffectsPresetsPatch
{
    internal static class StudioToolbar
    {
        private static SimpleToolbarToggle _toggle;
        private static bool _registered;
        private static bool _syncing;

        internal static void Register(Plugin owner)
        {
            if (_registered)
                return;

            _registered = true;
            StudioAPI.StudioLoadedChanged += (_, __) => CreateButton(owner);

            if (StudioAPI.StudioLoaded)
                CreateButton(owner);
        }

        internal static void SyncFromUiState()
        {
            if (_toggle == null || _toggle.IsDisposed)
                return;

            var open = SceneEffectsPresetsReflection.GetToggleUi();
            if (_toggle.Toggled.Value == open)
                return;

            _syncing = true;
            try
            {
                _toggle.Toggled.OnNext(open);
            }
            finally
            {
                _syncing = false;
            }
        }

        private static void CreateButton(Plugin owner)
        {
            if (_toggle != null)
                return;

            Texture2D icon;
            try
            {
                icon = ResourceUtils.GetEmbeddedResource("toolbar_icon.png").LoadTexture(TextureFormat.ARGB32);
            }
            catch (Exception ex)
            {
                Plugin.Log.LogWarning("Failed to load Studio toolbar icon for Scene Effects Presets: " + ex.Message);
                return;
            }

            icon.filterMode = FilterMode.Point;
            icon.wrapMode = TextureWrapMode.Clamp;

            _toggle = new SimpleToolbarToggle(
                "SceneEffectsPresets",
                "Scene Effects Presets",
                () => icon,
                SceneEffectsPresetsReflection.GetToggleUi(),
                owner,
                OnToolbarValueChanged);

            ToolbarManager.AddLeftToolbarControl(_toggle);
            Plugin.Log.LogInfo("Studio toolbar button for Scene Effects Presets registered.");
        }

        private static void OnToolbarValueChanged(bool enabled)
        {
            if (_syncing)
                return;

            if (enabled)
                SceneEffectsPresetsReflection.ReloadFilesList();

            SceneEffectsPresetsReflection.SetToggleUi(enabled);
        }
    }
}
