using UnityEngine;

namespace Assets
{
    public class MeshButton : MonoBehaviour
    {
        public MeshManager meshManager;
        public TextureChanger textureChanger;
        public int styleIndex;
        public void OnClick()
        {
            meshManager.ChangeMesh("Hair", styleIndex);
            textureChanger.OnHairstyleChanged();
        }
        public void ChangeTops()
        {
            meshManager.ChangeMesh("Tops", styleIndex);
        }
        public void ChangeBottoms()
        {
            meshManager.ChangeMesh("Bottoms", styleIndex);
        }
        public void ChangeShoes()
        {
            meshManager.ChangeMesh("Shoes", styleIndex);
        }
    }
}