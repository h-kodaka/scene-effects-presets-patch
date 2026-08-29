using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

namespace SceneEffectsPresetsPatch
{
    [BepInPlugin(GUID, PluginName, Version)]
    [BepInDependency(SceneEffectsPresetsReflection.OriginalPluginGuid, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInProcess("CharaStudio")]
    [BepInProcess("CharaStudio.exe")]
    public class Plugin : BaseUnityPlugin
    {
        public const string GUID = "local.kks.sceneeffectspresets.windowmemory";
        public const string PluginName = "SceneEffectsPresetsPatch";
        public const string Version = "1.0.0";

        internal static ManualLogSource Log;
        internal static ConfigEntry<bool> Enabled;
        internal static ConfigEntry<bool> HasSavedPosition;
        internal static ConfigEntry<float> WindowX;
        internal static ConfigEntry<float> WindowY;
        internal static ConfigEntry<float> WindowWidth;
        internal static ConfigEntry<float> WindowHeight;
        internal static ConfigEntry<bool> ShowMixPresets;

        private Harmony _harmony;

        private void Awake()
        {
            Log = Logger;

            Enabled = Config.Bind(
                "Window Memory",
                "Enabled",
                true,
                "Remember Scene Effects Presets window position and size between Studio sessions.");

            HasSavedPosition = Config.Bind(
                "Window Memory",
                "HasSavedPosition",
                false,
                "Internal flag indicating whether a saved window layout exists.");

            WindowX = Config.Bind(
                "Window Memory",
                "X",
                130f,
                "Saved window X position.");

            WindowY = Config.Bind(
                "Window Memory",
                "Y",
                230f,
                "Saved window Y position.");

            WindowWidth = Config.Bind(
                "Window Memory",
                "Width",
                330f,
                "Saved window width.");

            WindowHeight = Config.Bind(
                "Window Memory",
                "Height",
                630f,
                "Saved window height.");

            ShowMixPresets = Config.Bind(
                "Window Memory",
                "ShowMixPresets",
                false,
                "Whether the Mix Presets panel was visible when the window layout was saved.");

            if (!SceneEffectsPresetsReflection.IsOriginalPresent)
            {
                Log.LogInfo("Scene Effects Presets was not found. Window memory patch skipped.");
                return;
            }

            if (!SceneEffectsPresetsReflection.EnsureResolved())
            {
                Log.LogWarning("Scene Effects Presets was found but required members could not be resolved. Patch skipped.");
                return;
            }

            _harmony = Harmony.CreateAndPatchAll(typeof(Plugin).Assembly, GUID);
            Log.LogInfo("Scene Effects Presets detected. Window memory patch applied.");
            Log.LogInfo(
                $"[WindowPos] Config at startup: Enabled={Enabled.Value}, HasSavedPosition={HasSavedPosition.Value}, "
                + $"x={WindowX.Value:0.##}, y={WindowY.Value:0.##}, w={WindowWidth.Value:0.##}, h={WindowHeight.Value:0.##}, "
                + $"mix={ShowMixPresets.Value}");
            WindowPositionMemory.LoadAndApply();
        }

        private void OnDestroy()
        {
            WindowPositionMemory.SaveCurrent();
            _harmony?.UnpatchSelf();
        }
    }
}
