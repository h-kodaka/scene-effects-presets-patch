using System.Reflection;
using HarmonyLib;

namespace SceneEffectsPresetsWindowMemory.Patches
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
            WindowPositionMemory.LoadAndApply();
        }
    }
}
