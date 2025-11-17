using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class TextureChanger : MonoBehaviour
    {
        [System.Serializable]
        public class TextureCategory
        {
            public string categoryName;
            public Renderer renderer;
            public int materialIndex;
        }
        [Header("Categories & Managers")]
        public List<TextureCategory> textureCategories;
        public MeshManager meshManager;
        public FaceTextureCombiner faceCombiner;

        [Header("Renderers")]
        public Renderer hairRenderer;
        public Renderer bodyRenderer;
        public Renderer bgRenderer;

        [Header("Current Colors")]
        public Color currentHairColor;
        public Color currentSkinColor;
        public int currentBG;

        public RawImage skinGradient;

        private static readonly Dictionary<string, int> currentTextureIndices = new Dictionary<string, int>();


        void Start()
        {
            ChangeBG();
            currentSkinColor = HexToColor("F1BAA0");
            currentHairColor = HexToColor("1F1B1A");
            ChangeHairColor();
            ChangeSkinColor();
        }
        private static Color HexToColor(string hex)
        {
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            Color convertedColor = new Color(r / 255f, g / 255f, b / 255f, 1f);
            return convertedColor;
        }

        public void ChangeHairColor()
        {
            var baseHair = meshManager.GetCurrentVariantTextures("Hair")[0];
            Texture2D tintedHair = TintTexture(baseHair, currentHairColor);

            Material mat = hairRenderer.material;
            mat.SetTexture("_BaseMap", tintedHair);
        }
        private static Texture2D TintTexture(Texture2D baseTex, Color tint)
        {
            Texture2D tinted = new Texture2D(baseTex.width, baseTex.height, TextureFormat.RGBA32, false);
            Color[] basePixels = baseTex.GetPixels();
            Color[] newPixels = new Color[basePixels.Length];

            for (int i = 0; i < basePixels.Length; i++)
            {
                Color c = basePixels[i];

                Color mult = new Color(c.r * tint.r, c.g * tint.g, c.b * tint.b, c.a);
                Color over = new Color(
                    c.r < 0.5f ? 2 * c.r * tint.r : 1 - 2 * (1 - c.r) * (1 - tint.r),
                    c.g < 0.5f ? 2 * c.g * tint.g : 1 - 2 * (1 - c.g) * (1 - tint.g),
                    c.b < 0.5f ? 2 * c.b * tint.b : 1 - 2 * (1 - c.b) * (1 - tint.b),
                    c.a
                );
                c = Color.Lerp(mult, over, 0.4f);
                newPixels[i] = c;
            }

            tinted.SetPixels(newPixels);
            tinted.Apply();
            return tinted;
        }
        public void ChangeBG()
        {
            Texture2D[] variants = meshManager.GetCurrentVariantTextures("BG");
            Material mat = bgRenderer.material;
            mat.SetTexture("_BaseMap", variants[currentBG]);
        }
        public void ChangeSkinColor()
        {
            var baseSkin = meshManager.GetCurrentVariantTextures("Body")[0];
            var baseHead = meshManager.GetCurrentVariantTextures("Head")[0];

            Texture2D tintedSkin = TintTexture(baseSkin, currentSkinColor);
            Texture2D tintedHead = TintTexture(baseHead, currentSkinColor);

            bodyRenderer.materials[0].SetTexture("_BaseMap", tintedHead);
            bodyRenderer.materials[1].SetTexture("_BaseMap", tintedSkin);

            faceCombiner.baseTexture = tintedHead;
            faceCombiner.RebuildFace();
        }

        public void OnHairstyleChanged()
        {
            hairRenderer = meshManager.GetCurrentRenderer("Hair");

            ChangeHairColor();
        }
        public void OnSkinColorPicked(Color newColor)
        {
            currentSkinColor = newColor;
            ChangeSkinColor();
        }
        public void OnHairColorPicked(Color newColor)
        {
            currentHairColor = newColor;
            ChangeHairColor();
        }
        public void OnEyeColorPicked(Color newColor)
        {
            faceCombiner.currentEyeColor = newColor;
            faceCombiner.RebuildFace();
        }
        public void RandomizeTextures(FaceTextureCombiner faceCombiner)
        {
            var hairVariants = meshManager.GetCurrentVariantTextures("Hair");
            if (hairVariants != null && hairVariants.Length > 0)
            {
                currentHairColor = new Color(Random.value, Random.value, Random.value, 1f);
                hairRenderer = meshManager.GetCurrentRenderer("Hair");
                ChangeHairColor();
            }

            var bgVariants = meshManager.GetCurrentVariantTextures("BG");
            if (bgVariants != null && bgVariants.Length > 0)
            {
                currentBG = Random.Range(0, bgVariants.Length);
                bgRenderer = meshManager.GetCurrentRenderer("BG");
                ChangeBG();
            }


            if (faceCombiner.eyesVariants != null && faceCombiner.eyesVariants.Length > 0)
            {
                faceCombiner.currentEyeStyle = Random.Range(0, faceCombiner.eyesVariants.Length);
                faceCombiner.currentEyeColor = new Color(Random.value, Random.value, Random.value, 1f);
            }

            if (faceCombiner.lipsVariants != null && faceCombiner.lipsVariants.Length > 0)
                faceCombiner.currentLips = Random.Range(0, faceCombiner.lipsVariants.Length);

            if (faceCombiner.blushVariants != null && faceCombiner.blushVariants.Length > 0)
                faceCombiner.currentBlush = Random.Range(0, faceCombiner.blushVariants.Length);

            Texture2D gradientTex = skinGradient.texture as Texture2D;
            float t = Random.Range(0f, 1f);
            int x = Mathf.RoundToInt(t * (gradientTex.width - 1));
            Color sampledColor = gradientTex.GetPixel(x, gradientTex.height / 2); // sample middle row
            currentSkinColor = sampledColor;
            ChangeSkinColor();
        }
    }
}