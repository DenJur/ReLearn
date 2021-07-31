using UnityEngine;
using Verse;

namespace ReLearn.Util
{
    [StaticConstructorOnStartup]
    public static class MyTextures
    {
        public static Texture2D secondSkillBarFillTex =
            SolidColorMaterials.NewSolidColorTexture(new Color(1f, 0f, 0f, 0.2f));
    }
}