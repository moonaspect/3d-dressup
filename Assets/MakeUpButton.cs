using UnityEngine;

namespace Assets
{
    public class MakeUpButton : MonoBehaviour
    {
        public string makeUpType;
        public int makeUpIndex;
        public FaceTextureCombiner faceCombiner;

        public void OnClick()
        {
            if (makeUpType == "Lips")
            {
                faceCombiner.currentLips = makeUpIndex;
                faceCombiner.RebuildFace();
            }
            if (makeUpType == "Blush")
            {
                faceCombiner.currentBlush = makeUpIndex;
                faceCombiner.RebuildFace();
            }
            if (makeUpType == "Eyeshape")
            {
                faceCombiner.currentEyeStyle = makeUpIndex;
                faceCombiner.RebuildFace();
            }

        }
    }
}