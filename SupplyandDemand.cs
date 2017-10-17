using System;
using System.Collections.Generic;
using System.Diagnostics;
using RimWorld;
using RimWorld.Planet;
using HugsLib;
using Harmony;
using Verse;
using UnityEngine;


namespace TradeScalar
{
    public class TradeScalar : ModBase
    {
        public override string ModIdentifier
        {
            get
            {
                return "Supply_And_Demand";
            }
        }
        [HarmonyPatch(typeof(StockGenerator), "RandomCountOf", null)]
        ///<summary>
        ///Defines the method for scaling the result of RandomCountOf by colony wealth and difficulty level.
        ///</summary>
        public static class scaleWares_patch
        {
            //[HarmonyPrefix]
            //public static void Conditioner()
            //{

            //}
            [HarmonyPostfix]
            public static void Scaled(ref int __result)
            {
                int wealthTotal = Mathf.RoundToInt(Find.VisibleMap.wealthWatcher.WealthTotal); //Uses Verse.Find to get colony wealth, rounded to an integer.
                float tradeLoss = (1f - Find.Storyteller.difficulty.tradePriceFactorLoss); //Finds the tradePriceFactorLoss. Value is 1, 0.95, 0.9, 0.85, or 0.8
                __result = Mathf.RoundToInt((float)Math.Log(Math.E + (wealthTotal * tradeLoss)) * __result); //Minimum value of scalar is 1 when wealthTotal = 0
            }
        }
    }
}