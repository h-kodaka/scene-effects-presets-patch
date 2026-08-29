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

        private static bool _loggedFirstOnGui;

        private static void Postfix()
        {
            if (!_loggedFirstOnGui)
            {
                _loggedFirstOnGui = true;
                Plugin.Log.LogInfo("[WindowPos] OnGuiPatch Postfix invoked (first frame).");
            }

            WindowPositionMemory.TrySaveIfChanged();
        }
    }
}
