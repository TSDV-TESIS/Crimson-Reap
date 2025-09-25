using UnityEngine;

namespace Player.Health
{
    [CreateAssetMenu(menuName = "Scriptable Objects/Player Death Properties", fileName = "PlayerDeathProperties")]
    public class PlayerDeathProperties : ScriptableObject
    {
        [Header("Obscuring properties")]
        [Tooltip("Time obscuring the player material")]
        public float deathObscuringTime;
        public float obscureMaxValue = 2f;
        public float obscureMinValue = 0f;
        public AnimationCurve obscuringCurve;

        [Header("Dissolving properties")]
        [Tooltip("Time dissolving the player material")]
        public float deathDissolvingTime;
        [Tooltip("Time to start dissolving the player material in relation to when death happened")]
        public float deathDissolvingStartTime;
        public float dissolvingMaxValue = 1f;
        public float dissolvingMinValue = 0f;
        public AnimationCurve dissolvingCurve;

        [Header("Vfx properties")] 
        public string stopVfxEventName = "Stop";
    }
}