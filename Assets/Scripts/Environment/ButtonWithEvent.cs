using UnityEngine;
using UnityEngine.Events;

namespace Environment
{
    public class ButtonWithEvent : Button
    {
        [SerializeField] private UnityEvent<bool> onButtonInteracted;

        protected override void Interacted(bool value)
        {
            onButtonInteracted?.Invoke(value);
        }
    }
}
