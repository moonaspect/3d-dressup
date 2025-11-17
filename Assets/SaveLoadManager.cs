using UnityEngine;

namespace Assets
{
    public class SaveLoadManager : MonoBehaviour
    {
        public TextureChanger texChanger;
        public MeshManager meshManager;
        public FaceTextureCombiner faceCombiner;
        private const string HairIndexKey = "HairIndex";
        private const string BGIndexKey = "BGIndex";
        private const string TopIndexKey = "TopIndex";
        private const string BottomIndexKey = "BottomIndex";
        private const string ShoesIndexKey = "ShoesIndex";
        private const string HaircolorIndexKey = "HaircolorIndex";
        private const string SkincolorIndexKey = "SkincolorIndex";
        private const string EyecolorIndexKey = "EyecolorIndex";
        private const string EyesIndexKey = "EyesIndex";
        private const string BlushIndexKey = "BlushIndex";
        private const string LipsIndexKey = "LipsIndex";
        public void SaveCharacter()
        {
            PlayerPrefs.SetInt(HairIndexKey, meshManager.GetCurrentIndex("Hair"));
            PlayerPrefs.SetInt(TopIndexKey, meshManager.GetCurrentIndex("Tops"));
            PlayerPrefs.SetInt(BottomIndexKey, meshManager.GetCurrentIndex("Bottoms"));
            PlayerPrefs.SetInt(ShoesIndexKey, meshManager.GetCurrentIndex("Shoes"));

            PlayerPrefs.SetInt(BGIndexKey, texChanger.currentBG);

            SaveColor(HaircolorIndexKey, texChanger.currentHairColor);
            SaveColor(SkincolorIndexKey, texChanger.currentSkinColor);

            SaveColor(EyecolorIndexKey, faceCombiner.currentEyeColor);

            PlayerPrefs.SetInt(EyesIndexKey, faceCombiner.currentEyeStyle);
            PlayerPrefs.SetInt(BlushIndexKey, faceCombiner.currentBlush);
            PlayerPrefs.SetInt(LipsIndexKey, faceCombiner.currentLips);


            PlayerPrefs.Save();
        }
        private static void SaveColor(string key, Color color)
        {
            PlayerPrefs.SetString(key, ColorUtility.ToHtmlStringRGBA(color));
        }

        public void LoadCharacter()
        {
            if (!PlayerPrefs.HasKey(HairIndexKey)) return;

            meshManager.SetCategoryIndex("Hair", PlayerPrefs.GetInt(HairIndexKey));
            meshManager.SetCategoryIndex("Tops", PlayerPrefs.GetInt(TopIndexKey));
            meshManager.SetCategoryIndex("Bottoms", PlayerPrefs.GetInt(BottomIndexKey));
            meshManager.SetCategoryIndex("Shoes", PlayerPrefs.GetInt(ShoesIndexKey));

            texChanger.currentBG = PlayerPrefs.GetInt(BGIndexKey);
            texChanger.ChangeBG();

            texChanger.currentHairColor = LoadColor(HaircolorIndexKey);
            texChanger.ChangeHairColor();
            texChanger.currentSkinColor = LoadColor(SkincolorIndexKey);
            texChanger.ChangeSkinColor();

            faceCombiner.currentEyeColor = LoadColor(EyecolorIndexKey);
            faceCombiner.currentEyeStyle = PlayerPrefs.GetInt(EyesIndexKey);
            faceCombiner.currentBlush = PlayerPrefs.GetInt(BlushIndexKey);
            faceCombiner.currentLips = PlayerPrefs.GetInt(LipsIndexKey);

            faceCombiner.RebuildFace();
        }

        private static Color LoadColor(string key)
        {
            if (PlayerPrefs.HasKey(key))
            {
                Color color;
                if (ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString(key), out color))
                    return color;
            }
            return Color.white;
        }
    }
}