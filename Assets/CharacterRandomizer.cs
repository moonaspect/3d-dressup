using UnityEngine;

namespace Assets
{
    public class CharacterRandomizer : MonoBehaviour
    {
        public MeshManager meshManager;
        public TextureChanger textureChanger;
        public FaceTextureCombiner faceCombiner;

        public void RandomizeCharacter()
        {

            foreach (var category in meshManager.categories)
            {
                int randomIndex = Random.Range(0, category.variants.Length);
                meshManager.ChangeMesh(category.categoryName, randomIndex);
            }
            textureChanger.RandomizeTextures(faceCombiner);
        }
    }
}