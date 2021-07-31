using System.Collections.Generic;
using System.Linq;
using HugsLib;
using HugsLib.Settings;
using ReLearn.Util;
using Verse;

namespace ReLearn
{
    public class Mod : ModBase
    {
        internal static SettingHandle<float> XpMultiplier;
        internal static SettingHandle<float> MultiplicationChancePerXpGainTick;
        internal static SettingHandle<XpMultiplierType> XpMultiplierTypeSettingHandle;

        public override string ModIdentifier => "ReLearn";

        public override void DefsLoaded()
        {
            XpMultiplier = Settings.GetHandle("xpMultiplier", "xpMultiplier_title".Translate(),
                "xpMultiplier_desc".Translate(), 1f);
            XpMultiplier.CustomDrawer =
                rect => UIUtils.CustomDrawer_Filter(rect, XpMultiplier, 0f, 5.0f);
            MultiplicationChancePerXpGainTick = Settings.GetHandle("multiplicationChancePerXpGainTick",
                "multiplicationChancePerXpGainTick_title".Translate(),
                "multiplicationChancePerXpGainTick_desc".Translate(), 1f);
            MultiplicationChancePerXpGainTick.CustomDrawer =
                rect => UIUtils.CustomDrawer_Filter(rect, MultiplicationChancePerXpGainTick, 0f, 1.0f);
            XpMultiplierTypeSettingHandle = Settings.GetHandle("enumXpMultiplierType",
                "enumXpMultiplierType_title".Translate(),
                "enumXpMultiplierType_desc".Translate(), XpMultiplierType.PerLevel, null, "enumXpMul_");

            if (ModIsActive)
            {
                ThinkTreeDef zombieThinkTree = DefDatabase<ThinkTreeDef>.GetNamedSilentFail("Zombie");

                IEnumerable<ThingDef> things = DefDatabase<ThingDef>.AllDefs.Where(x =>
                    typeof(Pawn).IsAssignableFrom(x.thingClass)
                    && x.race?.intelligence == Intelligence.Humanlike
                    && !x.defName.Contains("AIPawn")
                    && (zombieThinkTree == null || x.race.thinkTreeMain != zombieThinkTree));

                foreach (ThingDef t in things)
                {
                    if (t.comps == null) t.comps = new List<CompProperties>(1);
                    t.comps.Add(new ReLearnCompProp());
                }
            }
        }

        internal enum XpMultiplierType
        {
            PerLevel,
            Fixed
        }
    }
}