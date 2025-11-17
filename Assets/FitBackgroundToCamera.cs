using UnityEngine;

namespace Assets
{
    [ExecuteAlways]
    public class FitBackgroundToCamera : MonoBehaviour
    {
        public Camera cam;
        public float distance = 10f;

        void Start()
        {
            FitToCamera();
        }

#if UNITY_EDITOR
        void OnValidate()
        {
            FitToCamera();
        }
#endif

        public void FitToCamera()
        {
            if (cam == null) return;

            transform.rotation = cam.transform.rotation;


            float height = 2f * distance * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
            float width = height * cam.aspect;

            transform.position = cam.transform.position + cam.transform.forward * distance;
            transform.rotation = cam.transform.rotation;
            transform.localScale = new Vector3(width, height, 1);
        }
    }
}