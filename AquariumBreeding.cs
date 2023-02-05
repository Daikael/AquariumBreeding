using System;
using System.Collections;
using System.Collections.Generic;
using BepInEx;
using BepInEx.Logging;
using Daikael.Subnautica.AquariumBreeding;
using HarmonyLib;
using SMLHelper;
using UnityEngine;

namespace Daikael.AquariumBreeding
{
    [BepInPlugin(myGUID, pluginName, versionString)]
    public class AquariumBreeding : BaseUnityPlugin
    {
        private static Dictionary<Aquarium, AquariumBreeding> _aquariumInfo = new Dictionary<Aquarium, AquariumBreeding>();
        private Aquarium _aquarium;
        public List<AquariumBreeding.AquariumBreedTime> BreedInfo;
        private const string myGUID = "Daikael.AquariumBreeding";
        private const string pluginName = "Aquarium Breeding";
        private const string versionString = "1.0.0";


        public static void Update(Aquarium aquarium)
        {
            if (aquarium == null)
            {
                Debug.Log("aquarium: Main update loop returned null!" + aquarium);
                return;
            }
            Debug.Log("Aquarium update firing!");
            if (aquarium.storageContainer.IsEmpty())
            {
                AquariumBreeding._aquariumInfo.Remove(aquarium);
            }
            else
            {
                Debug.Log("Main update caller firings");
                AquariumBreeding aquariumInfo;
                AquariumBreeding._aquariumInfo.TryGetValue(aquarium, out aquariumInfo);
                if (aquariumInfo == null)
                    AquariumBreeding._aquariumInfo.Add(aquarium, aquariumInfo = new AquariumBreeding(aquarium));
                aquariumInfo.AquariumUpdate();
            }
        }

        internal static AquariumBreeding Get(Aquarium aquarium)
        {
            AquariumBreeding aquariumInfo;
            _aquariumInfo.TryGetValue(aquarium, out aquariumInfo);
            return aquariumInfo;
        }



        private AquariumBreeding(Aquarium aquarium)
        {
            if (aquarium == null)
            {
                Debug.LogError("Error: Aquarium passed to AquariumBreeding constructor is null");
                return;
            }
            Debug.Log("aquarium: " + aquarium);
            this._aquarium = aquarium;
            this.BreedInfo = new List<AquariumBreeding.AquariumBreedTime>();
        }


        internal void AquariumUpdate()
        {
            try
            {
                if (DayNightCycle.main == null) return;
                double timePassed = DayNightCycle.main.timePassed;
                Debug.Log("timePassed: " + timePassed);
                if (_aquarium == null)
                {
                    Debug.Log("_aquarium: " + _aquarium);
                    return;
                }

                if (_aquarium.storageContainer == null)
                {
                    Debug.LogError("_aquarium.storageContainer is null");
                    return;
                }

                if (_aquarium.storageContainer.container == null)
                {
                    Debug.LogError("_aquarium.storageContainer.container is null");
                    return;
                }
                double num1 = timePassed % 1200.0;
                double num2 = timePassed - num1;
                Debug.Log("num1: " + num1);
                Debug.Log("num2: " + num2);
                List<AquariumBreeding.AquariumBreedTime> aquariumBreedTimeList = new List<AquariumBreeding.AquariumBreedTime>();
                ItemsContainer container = this._aquarium.storageContainer.container;
                foreach (TechType itemType in container.GetItemTypes())
                {
                    double num3 = (double)(1 % 12) / 24.0 * 1200.0;
                    double num4 = num2 + num3;
                    Debug.Log("num3: " + num3);
                    Debug.Log("num4: " + num4);
                    while (num4 < timePassed)
                        num4 += 600.0;
                    IList<InventoryItem> items = container.GetItems(itemType);
                    AquariumBreeding.AquariumBreedTime aquariumBreedTime = new AquariumBreeding.AquariumBreedTime()
                    {
                        FishType = itemType,
                        BreedTime = num4,
                        BreedCount = ((ICollection<InventoryItem>)items).Count / 2
                    };
                    aquariumBreedTimeList.Add(aquariumBreedTime);
                }
                this.BreedInfo = aquariumBreedTimeList;
            }
            catch (Exception ex)
            {
                Debug.LogError("Error in Update: " + ex.Message);
            }
        }



        public struct AquariumBreedTime
        {
            public TechType FishType;
            public double BreedTime;
            internal int BreedCount;
        }
    }
}
