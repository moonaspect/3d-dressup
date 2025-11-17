using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets
{
    public class GradientColorPicker : MonoBehaviour, IPointerDownHandler, IDragHandler
    {
        public RawImage gradientImage;
        public FaceTextureCombiner faceCombiner;
        public TextureChanger texChanger;
        public RectTransform pickerHandle;
        public SkinColorPickerUI pickerUI;

        private Texture2D gradientTexture;

        void Start()
        {
            gradientTexture = gradientImage.texture as Texture2D;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            PickColor(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            PickColor(eventData);
        }

        private void PickColor(PointerEventData eventData)
        {
            Vector2 localCursor;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
                gradientImage.rectTransform,
                eventData.position,
                eventData.pressEventCamera,
                out localCursor))
                return;

            Rect rect = gradientImage.rectTransform.rect;
            float x = Mathf.Clamp01((localCursor.x - rect.x) / rect.width);
            int texX = Mathf.RoundToInt(x * gradientTexture.width);
            Color color = gradientTexture.GetPixel(texX, gradientTexture.height / 2);
            texChanger.currentSkinColor = color;
            texChanger.ChangeSkinColor();

            if (pickerHandle)
                pickerHandle.anchoredPosition = new Vector2(Mathf.Clamp(localCursor.x, -gradientTexture.width / 2f, gradientTexture.width / 2f), gradientTexture.height / 2);

        }
    }
}