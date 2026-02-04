using BepInEx;
using HarmonyLib;
using UnityEngine;
using System.Collections;
using System.Reflection;
using System;

namespace counterMod
{
    [BepInPlugin("com.david.flipstats", "Flip Stats Tracker", "1.0.2")]
    public class FlipStatsPlugin : BaseUnityPlugin
    {
        public static int totalFlips = 0;
        public static int headsCount = 0;
        // This tracks the sum of probabilities for every flip performed
        public static double cumulativeExpectedHeads = 0d; 
        public static double currentProbability = 0d;

        public static double currentStreak = 0d;

        void Awake()
        {
            Logger.LogInfo("Flip Stats Tracker loaded!");
            new Harmony("com.david.flipstats").PatchAll();
        }

        void OnGUI()
{
            if (totalFlips > 0)
            {
                double luckFactor = headsCount - cumulativeExpectedHeads;
                double actualRate = ((double)headsCount / totalFlips) * 100d;
                
                // Calculate the probability of the current streak occurring
                // Math.Pow(0.5, 3) = 0.125
                double streakChance = Math.Pow(currentProbability, currentStreak);
                
                GUI.Box(new Rect(10, 10, 300, 130), "Flip Statistics");
                GUI.Label(new Rect(20, 30, 260, 20), $"Total Flips: {totalFlips}");
                // CHANGED :D1 to :F1
                GUI.Label(new Rect(20, 50, 260, 20), $"Actual Heads: {actualRate:F1}% ({headsCount})");
                GUI.Label(new Rect(20, 70, 260, 20), $"Expected Heads: {cumulativeExpectedHeads:F1}");
                
                string luckColor = luckFactor >= 0 ? "green" : "red";
                GUI.Label(new Rect(20, 90, 260, 20), $"Luck Factor: <color={luckColor}>{luckFactor:+0.00;-0.00} Heads</color>");

                if (currentStreak > 0)
                {
                    // Using :P2 displays it as a Percentage (0.01 becomes 1.00%)
                    // Or use :E2 if you want scientific notation for rare streaks
                    GUI.Label(new Rect(20, 110, 260, 20), $"Streak ({currentStreak}x) Odds: {streakChance:P4}");
                }
            }
        }
    }

    [HarmonyPatch(typeof(CoinFlip), "Flip")]
    class FlipTrackerPatch
    {
        static void Prefix(CoinFlip __instance)
        {
            FlipStatsPlugin.currentProbability = __instance.flipHeadsChance;
        }

        static IEnumerator Postfix(IEnumerator original, CoinFlip __instance)
        {
            while (original.MoveNext()) yield return original.Current;
            
            var field = typeof(CoinFlip).GetField("prevWasHeads", BindingFlags.NonPublic | BindingFlags.Instance);
            bool wasHeads = (bool)field.GetValue(__instance);
            
            // MATH UPDATE:
            // Add the probability of THIS specific flip to our running expected total
            FlipStatsPlugin.cumulativeExpectedHeads += FlipStatsPlugin.currentProbability;
            FlipStatsPlugin.totalFlips++;
            
            if (wasHeads)
            {
                FlipStatsPlugin.headsCount++;
                FlipStatsPlugin.currentStreak++;
            } 
            else
            {
                FlipStatsPlugin.currentStreak = 0d;    
            }
        }
    }
}