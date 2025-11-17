using UnityEngine;

namespace Assets
{
    public class TextureButton : MonoBehaviour
    {

        public string categoryName;
        public TextureChanger textureManager;
        public FaceTextureCombiner faceCombiner;
        public int colorIndex;
        public Color color;

        public void OnClick()
        {
            if (categoryName == "Eyes")
            {
                faceCombiner.currentEyeColor = color;
                faceCombiner.RebuildFace();
            }
            else if (categoryName == "Hair")
            {
                textureManager.currentHairColor = color;
                textureManager.ChangeHairColor();
            }
            else if (categoryName == "Skin")
            {
                textureManager.currentSkinColor = color;
                textureManager.ChangeSkinColor();
            }
            else if (categoryName == "BG")
            {
                textureManager.currentBG = colorIndex;
                textureManager.ChangeBG();
            }
        }
    }
}