using System.Collections;
using UnityEngine;

namespace Assets
{
    public class ScreenshotExporter : MonoBehaviour
    {
        int width;
        int height;
        public GameObject rightButton;
        public GameObject leftButton;
        public RectTransform mainPanel;
        public void CaptureScreenshot()
        {
            StartCoroutine(CaptureRoutine());
        }
        private IEnumerator CaptureRoutine()
        {
            rightButton.SetActive(false);
            leftButton.SetActive(false);

            RectTransform panelRect = mainPanel;
            Vector3[] corners = new Vector3[4];
            panelRect.GetWorldCorners(corners);
            float panelScreenWidth = corners[2].x - corners[0].x;

            width = Mathf.RoundToInt(Screen.width - panelScreenWidth);
            height = Screen.height;

            yield return new WaitForEndOfFrame();

            Texture2D screenshot = new Texture2D(width, height, TextureFormat.RGB24, false);
            screenshot.filterMode = FilterMode.Point;
            screenshot.ReadPixels(new Rect(0, 0, width, height), 0, 0);
            screenshot.Apply();

            rightButton.SetActive(true);
            leftButton.SetActive(true);

            string path = System.IO.Path.Combine(Application.persistentDataPath, "character.png");
            System.IO.File.WriteAllBytes(path, screenshot.EncodeToPNG());
            Debug.Log("Screenshot saved at: " + path);
        }
    }
}