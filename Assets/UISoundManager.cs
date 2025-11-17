using UnityEngine;

namespace Assets
{
    public class UISoundManager : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip clickSound;
        public AudioClip sidePanelClickSound;
        public AudioClip rollSound;
        public AudioClip cameraSound;
        public void PlayClick() => audioSource.PlayOneShot(clickSound);
        public void PlaySideClick() => audioSource.PlayOneShot(sidePanelClickSound);
        public void RandomizeClick() => audioSource.PlayOneShot(rollSound);
        public void CameraClick() => audioSource.PlayOneShot(cameraSound);
    }
}