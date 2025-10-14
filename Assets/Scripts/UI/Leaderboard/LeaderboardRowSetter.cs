using System;
using TMPro;
using UnityEngine;

namespace UI.Leaderboard
{
    public class LeaderboardRowSetter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private TextMeshProUGUI timeText;

        public void SetData(string userName, int timestamp)
        {
            nameText.text = userName;
            TimeSpan t = TimeSpan.FromMilliseconds(timestamp);
            timeText.text = string.Format("{0:D2}m:{1:D2}s:{2:D3}ms", 
                t.Minutes, 
                t.Seconds, 
                t.Milliseconds);
        }
    }
}
