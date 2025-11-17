using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class SkinColorPickerUI : MonoBehaviour
    {
        public GameObject colorPickerPanel;
        public FlexibleColorPicker hairColorPicker;
        public FlexibleColorPicker eyesColorPicker;
        public RawImage skinGradient;

        public FaceTextureCombiner faceCombiner;
        public TextureChanger texChanger;

        private string currentMode = "";

        private void OnEyeColorChanged(Color color)
        {
            faceCombiner.currentEyeColor = color;
            faceCombiner.RebuildFace();
        }
        private void OnHairColorChanged(Color color)
        {
            texChanger.currentHairColor = color;
            texChanger.ChangeHairColor();
        }
        void Start()
        {
            hairColorPicker.onColorChange.AddListener(OnHairColorChanged);
            eyesColorPicker.onColorChange.AddListener(OnEyeColorChanged);

            HideAllPickers();
            colorPickerPanel.SetActive(false);
        }


        public void ShowHairPicker() => TogglePicker("Hair");
        public void ShowEyesPicker() => TogglePicker("Eyes");
        public void ShowSkinPicker() => TogglePicker("Skin");
        private void TogglePicker(string mode)
        {

            colorPickerPanel.SetActive(true);
            currentMode = mode;
            HideAllPickers();

            switch (mode)
            {
                case "Hair":
                    hairColorPicker.gameObject.SetActive(true);
                    break;
                case "Eyes":
                    eyesColorPicker.gameObject.SetActive(true);
                    break;
                case "Skin":
                    skinGradient.gameObject.SetActive(true);
                    skinGradient.GetComponent<GradientColorPicker>().enabled = true;
                    break;
            }
        }
        private void HideAllPickers()
        {
            hairColorPicker.gameObject.SetActive(false);
            eyesColorPicker.gameObject.SetActive(false);
            skinGradient.gameObject.SetActive(false);
        }
        public void OnCloseClicked()
        {
            CloseAll();
        }

        private void CloseAll()
        {
            colorPickerPanel.SetActive(false);
            HideAllPickers();
            currentMode = "";
        }


    }
}