using System;
using HugsLib.Settings;
using UnityEngine;
using Verse;

namespace ReLearn.Util
{
    internal class UIUtils
    {
        public static bool CustomDrawer_Filter(Rect rect, SettingHandle<float> slider, float defMin, float defMax)
        {
            int labelWidth = 50;

            Rect sliderPortion = new Rect(rect);
            sliderPortion.width = sliderPortion.width - labelWidth;

            Rect labelPortion = new Rect(rect)
            {
                width = labelWidth,
                position = new Vector2(sliderPortion.position.x + sliderPortion.width + 5f,
                    sliderPortion.position.y + 4f)
            };

            sliderPortion = sliderPortion.ContractedBy(2f);

            Widgets.Label(labelPortion, Mathf.Round(slider.Value * 100f).ToString("F0") + "%");

            float val = Widgets.HorizontalSlider(sliderPortion, slider.Value, defMin, defMax, true);
            bool change = Math.Abs(slider.Value - val) > float.Epsilon;

            slider.Value = val;
            return change;
        }
    }
}