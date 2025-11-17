using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class NewMonoBehaviourScript : MonoBehaviour
    {
        [Header("Preview Setup")]
        public Camera previewCamera;
        public RenderTexture renderTexture;
        public MeshManager meshManager;
        public string categoryName = "Hair";
        [Header("UI Setup")]
        public List<RawImage> previewButtons;

        [Header("Capture Settings")]
        public int previewResolution = 160;

        private Texture2D tempTex;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            GeneratePreviews();
        }

        public void GeneratePreviews()
        {
            if (previewCamera == null || renderTexture == null || meshManager == null)
            {
                Debug.LogError("Missing setup in PreviewGenerator.");
                return;
            }

            var category = System.Array.Find(meshManager.categories, c => c.categoryName == categoryName);
            if (category == null)
            {
                Debug.LogWarning($"No category named '{categoryName}' found in MeshManager.");
                return;
            }
            tempTex = new Texture2D(previewResolution, previewResolution, TextureFormat.RGBA32, false);
            SetCameraFocus(categoryName);
            for (int i = 0; i < category.variants.Length; i++)
            {

                meshManager.ChangeMesh(categoryName, i);

                previewCamera.Render();

                RenderTexture.active = renderTexture;
                tempTex.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
                tempTex.Apply();

                if (i < previewButtons.Count && previewButtons[i] != null)
                {
                    RawImage img = previewButtons[i];
                    Texture2D copy = new Texture2D(tempTex.width, tempTex.height, TextureFormat.RGBA32, false);
                    copy.SetPixels(tempTex.GetPixels());
                    copy.Apply();
                    img.texture = copy;
                }
                meshManager.ChangeMesh(categoryName, 0);
            }

            RenderTexture.active = null;

            Debug.Log($"Generated {category.variants.Length} previews for category '{categoryName}'.");
        }
        [System.Serializable]
        public class CategoryFocus
        {
            public string categoryName;
            public Transform focusPoint;
            public float fieldOfView = 15f; // Zoom level for this category
        }

        public List<CategoryFocus> focusPoints = new List<CategoryFocus>();

        private void SetCameraFocus(string categoryName)
        {
            var focus = focusPoints.Find(f => f.categoryName == categoryName);
            if (focus != null && focus.focusPoint != null)
            {
                previewCamera.transform.position = focus.focusPoint.position;
                previewCamera.transform.rotation = focus.focusPoint.rotation;

                previewCamera.fieldOfView = focus.fieldOfView;
            }
            else
            {
                Debug.LogWarning($"No focus point for category '{categoryName}'");
            }
        }
    }
}