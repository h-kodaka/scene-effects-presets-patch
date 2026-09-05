using System.Reflection;
using HarmonyLib;

namespace SceneEffectsPresetsPatch.Patches
{
    [HarmonyPatch]
    internal static class OnGuiPatch
    {
        private static bool Prepare()
        {
            return SceneEffectsPresetsReflection.OnGuiMethod != null;
        }

        private static MethodBase TargetMethod()
        {
            return SceneEffectsPresetsReflection.OnGuiMethod;
        }

        private static void Postfix()
        {
            StudioToolbar.SyncFromUiState();
            WindowPositionMemory.TrySaveIfChanged();
        }
    }
}
