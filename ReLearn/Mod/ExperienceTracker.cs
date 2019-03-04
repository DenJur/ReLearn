using System.Collections.Generic;
using RimWorld;
using Verse;

namespace ReLearn
{
    public class ExperienceTracker : IExposable
    {
        private readonly Pawn pawn;
        private Dictionary<SkillDef, int> maxLevels;

        public ExperienceTracker(Pawn pawn)
        {
            this.pawn = pawn;
        }

        public void ExposeData()
        {
            Scribe_Collections.Look(ref maxLevels, "maxLevelsAchieved", LookMode.Def, LookMode.Value);
        }

        internal void FillExperience()
        {
            maxLevels = new Dictionary<SkillDef, int>();
            foreach (SkillRecord skill in pawn.skills.skills) maxLevels.Add(skill.def, skill.levelInt);
        }

        public int GetMaxLevel(SkillDef skill)
        {
            if (!maxLevels.ContainsKey(skill)) maxLevels.Add(skill, pawn.skills.GetSkill(skill).levelInt);

            return maxLevels[skill];
        }

        public void SetMaxLevel(SkillDef skill, int level)
        {
            if (!maxLevels.ContainsKey(skill))
                maxLevels.Add(skill, level);
            else
                maxLevels[skill] = level;
        }
    }
}