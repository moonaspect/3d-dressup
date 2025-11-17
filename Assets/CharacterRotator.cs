using UnityEngine;

namespace Assets
{
    public class CharacterRotator : MonoBehaviour
    {
        public float rotationSpeed = 100f;
        private int rotationDirection = 0;
        public FaceTextureCombiner faceCombiner;

        void Update()
        {
            if (rotationDirection != 0)
            {
                transform.Rotate(Vector3.up, rotationDirection * rotationSpeed * Time.deltaTime);
            }
        }
        public void StartRotateLeft()
        {
            rotationDirection = -1;
            faceCombiner.enableBlinking = false;
        }

        public void StartRotateRight()
        {
            rotationDirection = 1;
            faceCombiner.enableBlinking = false;
        }

        public void StopRotate()
        {
            rotationDirection = 0;
            faceCombiner.enableBlinking = true;
        }
    }
}