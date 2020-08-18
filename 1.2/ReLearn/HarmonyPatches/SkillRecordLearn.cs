using HarmonyLib;
using RimWorld;
using Verse;

namespace ReLearn
{
    [HarmonyPatch(typeof(SkillRecord), nameof(SkillRecord.Learn))]
    [HarmonyPatch(new[] {typeof(float), typeof(bool)})]
    [HarmonyPatch(MethodType.Normal)]
    internal static class SkillRecordLearn
    {
        [HarmonyPrefix]
        private static void Prefix(Pawn ___pawn, SkillDef ___def, int ___levelInt, ref float xp)
        {
            if (xp > 0 && Rand.Chance(Mod.MultiplicationChancePerXpGainTick.Value))
            {
                ReLearnComp comp = ___pawn.GetComp<ReLearnComp>();
                if (comp == null) return;

                int maxLevel = comp.Experience.GetMaxLevel(___def);
                if (___levelInt < maxLevel)
                {
                    if (Mod.XpMultiplierTypeSettingHandle.Value == Mod.XpMultiplierType.PerLevel)
                        xp *= 1 + Mod.XpMultiplier.Value * (maxLevel - ___levelInt);
                    else
                        xp *= 1 + Mod.XpMultiplier.Value;
                }
            }
        }

        [HarmonyPostfix]
        private static void Postfix(Pawn ___pawn, SkillDef ___def, int ___levelInt)
        {
            ReLearnComp comp = ___pawn.GetComp<ReLearnComp>();
            if (comp == null) return;

            int maxLevel = comp.Experience.GetMaxLevel(___def);
            if (___levelInt > maxLevel) comp.Experience.SetMaxLevel(___def, ___levelInt);
        }
    }
}