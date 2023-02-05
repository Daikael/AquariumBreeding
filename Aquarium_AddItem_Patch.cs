using HarmonyLib;
using System.Collections.Generic;
using Daikael.AquariumBreeding;

namespace Daikael.Subnautica.AquariumBreeding
{
    [HarmonyPatch(typeof(Aquarium))]
    [HarmonyPatch("AddItem")]
    public class Aquarium_AddItem_Patch
    {
        public static Dictionary<Aquarium, Daikael.AquariumBreeding.AquariumBreeding> aquariumInfo = new Dictionary<Aquarium, Daikael.AquariumBreeding.AquariumBreeding>();

        [HarmonyPostfix]
        public static void Postfix(Aquarium __instance) => Daikael.AquariumBreeding.AquariumBreeding.Update(__instance);
    }
}
