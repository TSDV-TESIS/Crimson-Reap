using UnityEngine;

namespace Events.Scriptables
{
    [CreateAssetMenu(fileName = "Vec2FloatEventChannel", menuName = "Events/Vector2 Float Channel", order = 0)]
    public class Vector2FloatEventChannel : TwoValuesEventChannelSO<Vector2, float>
    {
    }
}