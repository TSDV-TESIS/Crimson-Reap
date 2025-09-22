using CameraScripts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class FovsUIListener : MonoBehaviour
    {
        [SerializeField] private FovsData fovsData;
        [SerializeField] private TextMeshProUGUI text;
        
        private int _lastIndex;
        
        void OnEnable()
        {
            _lastIndex = fovsData.activeCameraPropertyIndex;

            WriteFovText();
        }
        
        void Update()
        {
            if (_lastIndex == fovsData.activeCameraPropertyIndex) return;

            WriteFovText();
        }

        private void WriteFovText()
        {
            CameraProperties activeCameraProperty = fovsData.cameraProperties[fovsData.activeCameraPropertyIndex];

            text.text = $"Distance {activeCameraProperty.cameraDistance}, FOV {activeCameraProperty.FOV}";
        }
    }
}
