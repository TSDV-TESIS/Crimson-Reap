using UnityEngine;

namespace UI
{
    public class PlayHandler : MonoBehaviour
    {
        [SerializeField] private GameObject inputField;

        public void OnClick()
        {
            inputField.SetActive(true);
        }
    }
}