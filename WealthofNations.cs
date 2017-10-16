using System;
using System.Collections.Generic;
using System.Diagnostics;
using RimWorld;
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
                return "TradeScalar";
            }
        }
        [HarmonyPatch(typeof(StockGenerator), "RandomCountOf", null)]
        ///<summary>
        ///Defines the method for scaling the result of RandomCountOf by colony wealth.
        ///</summary>
        public static class scaleWares_patch
            {
            [HarmonyPostfix]
            public static void Scaled(ref int __result)
                {
                int wealthTotal = Mathf.RoundToInt(Find.VisibleMap.wealthWatcher.WealthTotal); //Uses Verse.Find to get colony wealth, rounded to an integer.
                    __result = Mathf.RoundToInt((float)Math.Log(wealthTotal) * __result);
                }
            }
    }
}
