using System.Reflection;
using HarmonyLib;

namespace SceneEffectsPresetsPatch.Patches
{
    [HarmonyPatch]
    internal static class AwakePatch
    {
        private static bool Prepare()
        {
            return SceneEffectsPresetsReflection.AwakeMethod != null;
        }

        private static MethodBase TargetMethod()
        {
            return SceneEffectsPresetsReflection.AwakeMethod;
        }

        private static void Postfix()
        {
            Plugin.Log.LogInfo("[WindowPos] AwakePatch Postfix invoked.");
            WindowPositionMemory.LoadAndApply();
        }
    }
}
