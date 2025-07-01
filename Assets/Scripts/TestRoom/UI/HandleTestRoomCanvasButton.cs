using UnityEngine;

namespace TestRoom.UI
{
    public class HandleTestRoomCanvasButton : MonoBehaviour
    {
        [SerializeField] private GameObject offText;
        [SerializeField] private GameObject onText;

        public void OnInteract(bool value)
        {
            if (value)
            {
                offText?.SetActive(false);
                onText?.SetActive(true);
            }
            else
            {
                offText?.SetActive(true);
                onText?.SetActive(false);
            }
        }
    }
}
