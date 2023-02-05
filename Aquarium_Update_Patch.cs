// Decompiled with JetBrains decompiler
// Type: RALIV.Subnautica.AquariumBreeding.Aquarium_Update_Patch
// Assembly: AquariumBreeding, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 83405E3D-A241-4D57-9E2B-9C03C2345D61
// Assembly location: C:\Users\jaide\Downloads\Compressed\AquariumBreeding-410-1-0-2-1581738656\AquariumBreeding\AquariumBreeding.dll

using System.Collections.Generic;
using SMLHelper;
using HarmonyLib;
using Daikael.AquariumBreeding;
using UnityEngine;
using static HandReticle;
using System;
using System.Collections;
using BepInEx;

namespace Daikael.Subnautica.AquariumBreeding
{
    [HarmonyPatch(typeof(Aquarium))]
    [HarmonyPatch("Update")]
    public class Aquarium_Update_Patch
    {

        [HarmonyPostfix]
        public static void Postfix(Aquarium __instance)
        {
            if (__instance == null)
                return;
            double timePassed = DayNightCycle.main.timePassed;
            Daikael.AquariumBreeding.AquariumBreeding aquariumInfo = Daikael.AquariumBreeding.AquariumBreeding.Get(__instance);
            Console.WriteLine("Pre-breed function checking volume");
            if (aquariumInfo == null || !__instance.storageContainer.container.HasRoomFor(1, 1))
                return;
            List<Daikael.AquariumBreeding.AquariumBreeding.AquariumBreedTime> breedInfo = aquariumInfo.BreedInfo;
            bool flag = false;
            for (int index = 0; index < breedInfo.Count; ++index)
            {
                Daikael.AquariumBreeding.AquariumBreeding.AquariumBreedTime aquariumBreedTime = breedInfo[index];
                if (!flag && aquariumBreedTime.BreedTime <= timePassed)
                    __instance.StartCoroutine(Breed(__instance, aquariumBreedTime.FishType, aquariumBreedTime.BreedCount));
                Console.WriteLine("Pre-breed function firing");
                aquariumBreedTime.BreedTime += 600.0;
            }
        }

        private static IEnumerator Breed(Aquarium aquarium, TechType fishType, int breedCount)
        {
            Console.WriteLine("Breed function started");
            Vector2int itemSize = CraftData.GetItemSize(fishType);
            ItemsContainer container = aquarium.storageContainer.container;
            var fishPrefabTask = CraftData.GetPrefabForTechTypeAsync(fishType);
            yield return fishPrefabTask;
            GameObject prefab = fishPrefabTask.GetResult();
            for (int index = 0; index < breedCount; ++index)
            {
                if (!container.HasRoomFor(itemSize.x, itemSize.y))
                    Console.WriteLine("Container full");

                Pickupable pickupable = GameObject.Instantiate(prefab).GetComponent<Pickupable>();
                aquarium.storageContainer.container.AddItem(pickupable);
            }
            Console.WriteLine("Breed function completed");
            yield break;
        }
    }
}
