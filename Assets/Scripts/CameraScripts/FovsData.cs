using System.Collections.Generic;
using UnityEngine;

namespace CameraScripts
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Fovs Data", fileName = "FovsData")]
    public class FovsData : ScriptableObject
    {
        public List<CameraProperties> cameraProperties;
        public int activeCameraPropertyIndex = 0;
    }
}