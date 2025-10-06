using System;
using UnityEngine;

namespace CameraScripts
{
    [CreateAssetMenu(fileName = "CameraZoomZoneProperties", menuName = "Scriptable Objects/CameraZoomZoneProperties")]
    public class CameraZoomZoneProperties : ScriptableObject
    {
        public float zoomIn;
        public float focalLengthInZoom;
        public float focalDistanceInZoom;
    }
}
