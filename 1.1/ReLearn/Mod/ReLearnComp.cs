using Verse;

namespace ReLearn
{
    public class ReLearnComp : ThingComp
    {
        private ExperienceTracker _experience;

        public ExperienceTracker Experience
        {
            get
            {
                if (_experience == null)
                {
                    if (parent is Pawn pawn)
                    {
                        _experience = new ExperienceTracker(pawn);
                        _experience.FillExperience();
                    }
                    else
                    {
                        Log.Error("ReLearn: Tried to add Comp to " + parent.Label + " but it is not a pawn.");
                    }
                }

                return _experience;
            }
            set => _experience = value;
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Deep.Look(ref _experience, "experience", parent as Pawn);
        }
    }
}