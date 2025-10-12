using System;
using Leaderboard;
using UnityEngine;

namespace UI.Leaderboard
{
    public class LeaderboardRequestHandler : MonoBehaviour
    {
        [SerializeField] private GameObject leaderboardRow;
        [SerializeField] private GameObject leaderboardTable;
        [SerializeField] private LeaderboardData leaderboardData;
        [SerializeField] private LevelEnum level;
        
        void OnEnable()
        {
            leaderboardData.requestObtained.AddListener(HandleLeaderboard);
            leaderboardData.requestData.Invoke(level);
        }

        private void OnDisable()
        {
            leaderboardData.requestObtained.RemoveListener(HandleLeaderboard);
        }

        private void HandleLeaderboard()
        {
            Debug.Log("OBTAINED!");
            
            if (leaderboardData.hasError)
            {
                Debug.LogError("ERROR!");
                return;
            }

            foreach (LeaderboardRow row in leaderboardData.data)
            {
                Debug.Log($"{row.name} ASD");
                GameObject newRow = GameObject.Instantiate(leaderboardRow, leaderboardTable.transform);
                LeaderboardRowSetter dataSetter = newRow.GetComponent<LeaderboardRowSetter>();
                dataSetter.SetData(row.name, row.timeBeaten);
            }
        }
    }
}
