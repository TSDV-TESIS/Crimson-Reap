using System;
using System.Collections;
using Events.Scriptables;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Utils
{
    public class RumbleController : MonoBehaviour
    {
        [SerializeField] private string rumblePref;
        [SerializeField] private string rumbleFlagPref;
        [SerializeField] private float defaultRumbleMultiplier = 1.0f;
        [SerializeField] private RumbleChannel onRumble;
        
        private Coroutine _isRumbling;

        public void OnEnable()
        {
            onRumble?.onTypedEvent.AddListener(StartRumble);
        }

        public void OnDisable()
        {
            onRumble?.onTypedEvent.RemoveListener(StartRumble);
        }

        public void StartRumble(RumbleControllerData rumbleControllerData)
        {
            if (_isRumbling != null)
            {
                StopCoroutine(_isRumbling);
            }

            if (Gamepad.current != null)
            {
                _isRumbling = StartCoroutine(RumbleCoroutine(rumbleControllerData.duration, rumbleControllerData.forceAmount));
            }
        }

        private IEnumerator RumbleCoroutine(float rumbleDuration, float lowFrequency)
        {
            float rumbleMultiplier = PlayerPrefs.GetFloat(rumblePref);
            bool rumbleFlag = PlayerPrefs.GetInt(rumbleFlagPref) == 1;
            float multiplier = rumbleFlag ? rumbleMultiplier : defaultRumbleMultiplier;
            
            Gamepad.current.SetMotorSpeeds(lowFrequency * multiplier, lowFrequency * multiplier);
            yield return new WaitForSecondsRealtime(rumbleDuration);
            Gamepad.current.SetMotorSpeeds(0, 0);
        }
    }
}