using UnityEngine;

namespace SceneEffectsPresetsPatch
{
    internal static class WindowPositionMemory
    {
        private const string LogPrefix = "[WindowPos]";

        private static Rect _lastSavedRect;
        private static bool _lastSavedShowMixPresets;
        private static bool _hasSavedSnapshot;
        private static bool _loggedToggleUiOff;

        internal static void LoadAndApply()
        {
            if (!Plugin.Enabled.Value)
            {
                Plugin.Log.LogInfo($"{LogPrefix} LoadAndApply skipped (Awake): Enabled is false.");
                return;
            }

            if (!Plugin.HasSavedPosition.Value)
            {
                Plugin.Log.LogInfo($"{LogPrefix} LoadAndApply skipped: no saved position (HasSavedPosition=false).");
                SeedSnapshotFromCurrent();
                return;
            }

            var configRect = new Rect(
                Plugin.WindowX.Value,
                Plugin.WindowY.Value,
                Plugin.WindowWidth.Value,
                Plugin.WindowHeight.Value);
            var beforeRect = SceneEffectsPresetsReflection.GetWindowRect();
            var beforeMix = SceneEffectsPresetsReflection.GetShowMixPresets();

            var rect = ClampToScreen(configRect);

            SceneEffectsPresetsReflection.SetWindowRect(rect);
            SceneEffectsPresetsReflection.SetShowMixPresets(Plugin.ShowMixPresets.Value);

            _lastSavedRect = rect;
            _lastSavedShowMixPresets = Plugin.ShowMixPresets.Value;
            _hasSavedSnapshot = true;

            Plugin.Log.LogInfo(
                $"{LogPrefix} Restored: config x={configRect.x:0.##}, y={configRect.y:0.##}, "
                + $"w={configRect.width:0.##}, h={configRect.height:0.##}, mix={Plugin.ShowMixPresets.Value}; "
                + $"before x={beforeRect.x:0.##}, y={beforeRect.y:0.##}, w={beforeRect.width:0.##}, h={beforeRect.height:0.##}, mix={beforeMix}; "
                + $"applied x={rect.x:0.##}, y={rect.y:0.##}, w={rect.width:0.##}, h={rect.height:0.##}, "
                + $"toggleUI={SceneEffectsPresetsReflection.GetToggleUi()}");
        }

        internal static void TrySaveIfChanged()
        {
            if (!Plugin.Enabled.Value)
                return;

            if (!SceneEffectsPresetsReflection.GetToggleUi())
            {
                if (!_loggedToggleUiOff)
                {
                    _loggedToggleUiOff = true;
                    Plugin.Log.LogInfo($"{LogPrefix} TrySaveIfChanged waiting: toggleUI is false (window not open).");
                }

                return;
            }

            _loggedToggleUiOff = false;

            var rect = SceneEffectsPresetsReflection.GetWindowRect();
            var showMixPresets = SceneEffectsPresetsReflection.GetShowMixPresets();

            if (_hasSavedSnapshot
                && RectApproximatelyEquals(rect, _lastSavedRect)
                && showMixPresets == _lastSavedShowMixPresets)
            {
                return;
            }

            Save(rect, showMixPresets, "OnGUI");
        }

        internal static void SaveCurrent()
        {
            if (!Plugin.Enabled.Value)
            {
                Plugin.Log.LogInfo($"{LogPrefix} SaveCurrent skipped (OnDestroy): Enabled is false.");
                return;
            }

            if (!SceneEffectsPresetsReflection.GetToggleUi() && !_hasSavedSnapshot)
            {
                Plugin.Log.LogInfo($"{LogPrefix} SaveCurrent skipped (OnDestroy): window was never restored or opened.");
                return;
            }

            var rect = SceneEffectsPresetsReflection.GetWindowRect();
            var showMixPresets = SceneEffectsPresetsReflection.GetShowMixPresets();
            Plugin.Log.LogInfo(
                $"{LogPrefix} SaveCurrent (OnDestroy): current x={rect.x:0.##}, y={rect.y:0.##}, "
                + $"w={rect.width:0.##}, h={rect.height:0.##}, mix={showMixPresets}, "
                + $"toggleUI={SceneEffectsPresetsReflection.GetToggleUi()}");

            Save(rect, showMixPresets, "OnDestroy");
        }

        private static void Save(Rect rect, bool showMixPresets, string reason)
        {
            var rawRect = rect;
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

            Plugin.Log.LogInfo(
                $"{LogPrefix} Saved ({reason}): raw x={rawRect.x:0.##}, y={rawRect.y:0.##}, "
                + $"w={rawRect.width:0.##}, h={rawRect.height:0.##}, mix={showMixPresets}; "
                + $"stored x={rect.x:0.##}, y={rect.y:0.##}, w={rect.width:0.##}, h={rect.height:0.##}, "
                + $"HasSavedPosition=true");
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
