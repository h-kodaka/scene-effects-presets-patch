using System.Reflection;
using HarmonyLib;

namespace SceneEffectsPresetsWindowMemory.Patches
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
            WindowPositionMemory.TrySaveIfChanged();
        }
    }
}
