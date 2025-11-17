using System.Collections;
using System.Collections.Generic;
using ServiceButcator;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ColorSorting3D.LevelEditor
{
    internal class UIServices : MonoBehaviour
    {
        [SerializeField] private TMP_InputField levelInputField = default;
        [SerializeField] private Toggle availablecontainerToggle = default;
        [SerializeField] private Slider cameraPosSlider = default;

        public int GetLevelInput()
        {
            string indexStr = levelInputField.text;

            if (int.TryParse(indexStr, out var index) && index >= 0)
                return index;

            return -1;
        }

        public float GetPosSliderValue()
            => cameraPosSlider.value;

        public void SetPosSliderValue(float value)
            => cameraPosSlider.value = value;

        public bool GetToggleValue()
            => availablecontainerToggle.isOn;

        public void SetToggleValue(bool value)
            => availablecontainerToggle.isOn = value;
    }
}