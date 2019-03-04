using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;
using ReLearn.Util;
using RimWorld;
using UnityEngine;
using Verse;

namespace ReLearn.HarmonyPatches
{
    [HarmonyPatch(typeof(SkillUI), nameof(SkillUI.DrawSkill))]
    [HarmonyPatch(new[] {typeof(SkillRecord), typeof(Rect), typeof(SkillUI.SkillDrawMode), typeof(string)})]
    [HarmonyPatch(MethodType.Normal)]
    internal static class SkillUIDrawSkill
    {
        [HarmonyTranspiler]
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instr)
        {
            MethodInfo fillableBarMethod = AccessTools.Method(typeof(Widgets), nameof(Widgets.FillableBar), new[]
            {
                typeof(Rect),
                typeof(float),
                typeof(Texture2D),
                typeof(Texture2D),
                typeof(bool)
            });
            MethodInfo drawLimit = AccessTools.Method(typeof(SkillUIDrawSkill), nameof(DrawLimit), new[]
            {
                typeof(SkillRecord),
                typeof(Rect),
                typeof(float),
                typeof(Pawn)
            });
            FieldInfo labelWidthField = AccessTools.Field(typeof(SkillUI), "levelLabelWidth");
            FieldInfo skillPawnField = AccessTools.Field(typeof(SkillRecord), "pawn");
#if DEBUG
            if (fillableBarMethod == null) Log.Error("SkillUIDrawSkill: FillableBar method is null.");
            if (drawLimit == null) Log.Error("SkillUIDrawSkill: DrawLimit method is null.");
            if (labelWidthField == null) Log.Error("SkillUIDrawSkill: LabelWidth field is null.");
            if (skillPawnField == null) Log.Error("SkillUIDrawSkill: SkillRecord pawn field is null.");
#endif
            foreach (CodeInstruction instruction in instr)
            {
                yield return instruction;
                if (instruction.opcode == OpCodes.Call && instruction.operand == fillableBarMethod)
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldarg_1);
                    yield return new CodeInstruction(OpCodes.Ldsfld, labelWidthField);
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Ldfld, skillPawnField);
                    yield return new CodeInstruction(OpCodes.Call, drawLimit);
                }
            }
        }

        private static void DrawLimit(SkillRecord skill, Rect holdingRect, float labelWidth, Pawn pawn)
        {
            if (!skill.TotallyDisabled && pawn != null &&
                pawn.GetComp<ReLearnComp>() is ReLearnComp comp)
            {
                // Draw limit fillablebar 
                float rectX = 36f + labelWidth;
                Rect rect2 = new Rect(rectX, 0f, holdingRect.width - rectX, holdingRect.height);
                int maxLevel = comp.Experience.GetMaxLevel(skill.def);
                float fillPercent = Mathf.Max(0.01f, skill.Level / 20f);
                Rect rect3 = new Rect(rect2);
                float f = rect2.width * fillPercent;
                rect3.x += f;
                float secondFillPercent = Mathf.Max(0f, (maxLevel - skill.Level) / 20f);
                rect3.width *= secondFillPercent;
                Widgets.FillableBar(rect3, 1.0f, MyTextures.secondSkillBarFillTex, null, false);

                // Max level label
                string label2 = "maxLevelString".Translate(new NamedArgument(maxLevel.ToStringCached(), "level"));
                GUIContent content = new GUIContent(label2);
                Vector2 size = Text.CurFontStyle.CalcSize(content);
                Rect rect4 = new Rect(rect2) {x = holdingRect.width - size.x - 4f};
                rect4.yMin += 3f;
                Widgets.Label(rect4, label2);
            }
        }
    }
}