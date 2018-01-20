using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using Harmony;
using HugsLib;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse;

// Syntax: [Game Major] [Mod Major] [Game Minor] [Mod Minor]
[assembly: AssemblyVersion("0.1.1.*")]

namespace TradeScalar
{
    public class TradeScalar : ModBase
    {
        public override string ModIdentifier => "Supply_And_Demand";

        private TradeScalar() { }

        ///<summary>
        /// Defines the method for scaling the result of RandomCountOf by colony wealth and difficulty level.
        ///</summary>
        [HarmonyPatch(typeof(StockGenerator))]
        [HarmonyPatch("RandomCountOf")]
        public static class scaleWares_patch
        {
            [HarmonyPostfix]
            public static void Scaled(ref int __result, StockGenerator __instance)
            {
                if (!__instance.trader.orbital && !__instance.trader.defName.Contains("Base_")) return;

                var random = UnityEngine.Random.Range(-3, 3);

                // Uses Verse.Find to get colony wealth.
                var wealthTotal = Find.VisibleMap.wealthWatcher.WealthTotal;

                // Finds the tradePriceFactorLoss. Value is 1, 0.95, 0.9, 0.85, or 0.8
                var tradeLoss = (1 - Find.Storyteller.difficulty.tradePriceFactorLoss);

                // Minimum value of scalar is 1 when wealthTotal = 0
                __result = random + (int)Math.Round(Math.Log(Math.E + (wealthTotal * tradeLoss)) * __result);
            }
        }
    }
}