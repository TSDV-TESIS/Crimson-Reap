using Events.Scriptables;
using UnityEngine;

namespace CameraScripts
{
    public class ShakeTest : MonoBehaviour
    {
        [SerializeField] private CameraShakeProfile _shakeProfile;
        [SerializeField] private ShakeProfileEventChannel cameraShakeEventChannel;

        [ContextMenu("Test")]
        void Shake()
        {
            cameraShakeEventChannel.RaiseEvent(_shakeProfile);
        }
    }
}