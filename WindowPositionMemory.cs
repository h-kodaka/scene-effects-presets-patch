using UnityEngine;

namespace SceneEffectsPresetsPatch
{
    internal static class WindowPositionMemory
    {
        private static Rect _lastSavedRect;
        private static bool _lastSavedShowMixPresets;
        private static bool _hasSavedSnapshot;

        internal static void LoadAndApply()
        {
            if (!Plugin.Enabled.Value)
                return;

            if (!Plugin.HasSavedPosition.Value)
            {
                SeedSnapshotFromCurrent();
                return;
            }

            var rect = ClampToScreen(new Rect(
                Plugin.WindowX.Value,
                Plugin.WindowY.Value,
                Plugin.WindowWidth.Value,
                Plugin.WindowHeight.Value));

            SceneEffectsPresetsReflection.SetWindowRect(rect);
            SceneEffectsPresetsReflection.SetShowMixPresets(Plugin.ShowMixPresets.Value);

            _lastSavedRect = rect;
            _lastSavedShowMixPresets = Plugin.ShowMixPresets.Value;
            _hasSavedSnapshot = true;
        }

        internal static void TrySaveIfChanged()
        {
            if (!Plugin.Enabled.Value || !SceneEffectsPresetsReflection.GetToggleUi())
                return;

            var rect = SceneEffectsPresetsReflection.GetWindowRect();
            var showMixPresets = SceneEffectsPresetsReflection.GetShowMixPresets();

            if (_hasSavedSnapshot
                && RectApproximatelyEquals(rect, _lastSavedRect)
                && showMixPresets == _lastSavedShowMixPresets)
            {
                return;
            }

            Save(rect, showMixPresets);
        }

        internal static void SaveCurrent()
        {
            if (!Plugin.Enabled.Value)
                return;

            if (!SceneEffectsPresetsReflection.GetToggleUi() && !_hasSavedSnapshot)
                return;

            Save(
                SceneEffectsPresetsReflection.GetWindowRect(),
                SceneEffectsPresetsReflection.GetShowMixPresets());
        }

        private static void Save(Rect rect, bool showMixPresets)
        {
            rect = ClampToScreen(rect);

            Plugin.WindowX.Value = rect.x;
            Plugin.WindowY.Value = rect.y;
            Plugin.WindowWidth.Value = rect.width;
            Plugin.WindowHeight.Value = rect.height;
            Plugin.ShowMixPresets.Value = showMixPresets;
            Plugin.HasSavedPosition.Value = true;

            _lastSavedRect = rect;
            _lastSavedShowMixPresets = showMixPresets;
            _hasSavedSnapshot = true;
        }

        private static void SeedSnapshotFromCurrent()
        {
            _lastSavedRect = SceneEffectsPresetsReflection.GetWindowRect();
            _lastSavedShowMixPresets = SceneEffectsPresetsReflection.GetShowMixPresets();
            _hasSavedSnapshot = true;
        }

        internal static Rect ClampToScreen(Rect rect)
        {
            if (rect.width > Screen.width)
                rect.width = Screen.width;
            if (rect.height > Screen.height)
                rect.height = Screen.height;
            if (rect.xMax > Screen.width)
                rect.x -= rect.xMax - Screen.width;
            if (rect.yMax > Screen.height)
                rect.y -= rect.yMax - Screen.height;
            if (rect.x < 0f)
                rect.x = 0f;
            if (rect.y < 0f)
                rect.y = 0f;

            return rect;
        }

        private static bool RectApproximatelyEquals(Rect a, Rect b)
        {
            return Mathf.Approximately(a.x, b.x)
                && Mathf.Approximately(a.y, b.y)
                && Mathf.Approximately(a.width, b.width)
                && Mathf.Approximately(a.height, b.height);
        }
    }
}
