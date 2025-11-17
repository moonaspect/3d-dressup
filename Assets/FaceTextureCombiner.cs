using System.Collections;
using UnityEngine;

namespace Assets
{
    [System.Serializable]
    public class TextureVariantSet
    {
        public string styleName;
        public Texture2D[] colorVariants;
    }
    public class FaceTextureCombiner : MonoBehaviour
    {
        public MeshManager meshManager;
        public Renderer faceRenderer;

        public Texture2D[] eyesVariants;
        public Texture2D[] lipsVariants;
        public Texture2D[] blushVariants;

        public int currentEyeStyle;
        public Color currentEyeColor;
        public int currentLips;
        public int currentBlush;

        private Texture2D combinedTexture;
        public Texture2D baseTexture;

        [Header("Blinking")]
        public Texture2D closedEyesTexture;
        public bool enableBlinking = true;
        public float blinkIntervalMin = 3f;
        public float blinkIntervalMax = 7f;
        public float blinkDuration = 0.1f;

        private bool isBlinking = false;

        void Start()
        {
            currentEyeColor = HexToColor("573629");
            RebuildFace();
            if (enableBlinking)
                StartCoroutine(BlinkRoutine());
        }

        public void RebuildFace()
        {

            combinedTexture = new Texture2D(baseTexture.width, baseTexture.height, TextureFormat.RGBA32, false);
            combinedTexture.SetPixels(baseTexture.GetPixels());
            ApplyOverlay(combinedTexture, blushVariants[currentBlush]);
            ApplyOverlay(combinedTexture, lipsVariants[currentLips]);
            if (isBlinking)
                ApplyOverlay(combinedTexture, TintTexture(closedEyesTexture, currentEyeColor));
            else
                ApplyOverlay(combinedTexture, TintTexture(eyesVariants[currentEyeStyle], currentEyeColor));

            combinedTexture.Apply();

            faceRenderer.materials[0].SetTexture("_BaseMap", combinedTexture);
        }
        private IEnumerator BlinkRoutine()
        {
            while (enableBlinking)
            {
                float waitTime = Random.Range(blinkIntervalMin, blinkIntervalMax);
                yield return new WaitForSeconds(waitTime);
                yield return StartCoroutine(DoBlink());
            }
        }
        private IEnumerator DoBlink()
        {
            if (isBlinking) yield break;
            isBlinking = true;


            RebuildFace();

            yield return new WaitForSeconds(blinkDuration);

            isBlinking = false;
            RebuildFace();

        }

        private static void ApplyOverlay(Texture2D baseTex, Texture2D overlay)
        {

            Color[] basePixels = baseTex.GetPixels();
            Color[] overlayPixels = overlay.GetPixels();

            for (int i = 0; i < basePixels.Length; i++)
            {
                Color src = overlayPixels[i];
                Color dst = basePixels[i];

                float a = src.a;
                basePixels[i] = new Color(
                    src.r * a + dst.r * (1 - a),
                    src.g * a + dst.g * (1 - a),
                    src.b * a + dst.b * (1 - a),
                    1f
                );
            }
            baseTex.SetPixels(basePixels);
            baseTex.Apply();

        }
        private static Texture2D TintTexture(Texture2D baseTex, Color tint)
        {
            Texture2D tinted = new Texture2D(baseTex.width, baseTex.height, TextureFormat.RGBA32, false);
            Color[] pixels = baseTex.GetPixels();

            for (int i = 0; i < pixels.Length; i++)
            {
                pixels[i] = new Color(
                    1 - (1 - pixels[i].r) * (1 - tint.r),
                    1 - (1 - pixels[i].g) * (1 - tint.g),
                    1 - (1 - pixels[i].b) * (1 - tint.b),
                    pixels[i].a
                );
            }

            tinted.SetPixels(pixels);
            tinted.Apply();
            return tinted;
        }
        private static Color HexToColor(string hex)
        {
            byte r = byte.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
            byte g = byte.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
            byte b = byte.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
            Color convertedColor = new Color(r / 255f, g / 255f, b / 255f, 1f);
            return convertedColor;
        }

    }
}