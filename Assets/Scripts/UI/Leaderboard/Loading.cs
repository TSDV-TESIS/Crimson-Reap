using System;
using TMPro;
using UnityEngine;

namespace UI.Leaderboard
{
    public class Loading : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI loadingText;
        [SerializeField] private float secondsPerDot = 0.5f;
        
        private int _dots;
        private float _timePassed;
        private void OnEnable()
        {
            _dots = 0;
            _timePassed = 0;
        }

        public void Update()
        {
            _timePassed += Time.unscaledDeltaTime;
            
            if (_timePassed > secondsPerDot)
            {
                _dots++;
                loadingText.text += ".";
                if (_dots == 2) loadingText.text = "Loading.";
                _timePassed = 0f;
            }
        }
    }
}
