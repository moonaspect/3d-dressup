using System.Linq;
using UnityEngine;

namespace Assets
{
    public class MeshManager : MonoBehaviour
    {
        [System.Serializable]
        public class MeshVariant
        {
            public GameObject mesh;
            public Texture2D[] colorVariants;
        }

        [System.Serializable]
        public class MeshCategory
        {
            public string categoryName;
            public MeshVariant[] variants;
            [HideInInspector] public int currentIndex = 0;
            [HideInInspector] public Renderer currentRenderer;
        }
        public MeshCategory[] categories;
        public TextureChanger textureChanger;
        public void ChangeMesh(string categoryName, int index)
        {
            foreach (var cat in categories)
            {
                if (cat.categoryName == categoryName)
                {
                    for (int i = 0; i < cat.variants.Length; i++)
                    {
                        bool isActive = i == index;
                        cat.variants[i].mesh.SetActive(isActive);
                        if (isActive)
                            cat.currentRenderer = cat.variants[i].mesh.GetComponent<Renderer>();
                    }

                    cat.currentIndex = index;
                    return;
                }
            }
        }
        public Renderer GetCurrentRenderer(string categoryName)
        {
            return categories.FirstOrDefault(c => c.categoryName == categoryName)?.currentRenderer;
        }
        public Texture2D[] GetCurrentVariantTextures(string categoryName)
        {
            var cat = categories.FirstOrDefault(c => c.categoryName == categoryName);

            return cat.variants[cat.currentIndex].colorVariants;
        }

        public void SetCategoryIndex(string categoryName, int index)
        {
            var cat = categories.FirstOrDefault(c => c.categoryName == categoryName);
            if (cat != null)
            {
                index = Mathf.Clamp(index, 0, cat.variants.Length - 1);
                ChangeMesh(categoryName, index);
            }
        }
        public int GetCurrentIndex(string categoryName)
        {
            var cat = categories.FirstOrDefault(c => c.categoryName == categoryName);
            return cat != null ? cat.currentIndex : 0;
        }
    }
}