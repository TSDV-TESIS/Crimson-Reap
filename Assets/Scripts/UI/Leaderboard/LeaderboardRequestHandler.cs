using System;
using Leaderboard;
using Player;
using UnityEngine;

namespace UI.Leaderboard
{
    public class LeaderboardRequestHandler : MonoBehaviour
    {
        [SerializeField] private GameObject leaderboardRow;
        [SerializeField] private GameObject leaderboardTable;
        [SerializeField] private LeaderboardData leaderboardData;
        [SerializeField] private LeaderboardCreateEvent createNewTimeEvent;
        [SerializeField] private PlayerName playerName;
        [SerializeField] private LevelEnum level;
        
        void OnEnable()
        {
            leaderboardData.requestObtained.AddListener(HandleLeaderboard);
            createNewTimeEvent.createNewTimeFinish.AddListener(HandleGetLeaderboard);
        }

        private void OnDisable()
        {
            leaderboardData.requestObtained.RemoveListener(HandleLeaderboard);
            createNewTimeEvent.createNewTimeFinish.RemoveListener(HandleGetLeaderboard);

        }

        private void HandleGetLeaderboard()
        {
            if (createNewTimeEvent.hasError)
            {
                Debug.LogError("Error creating new time.");
            }
            leaderboardData.requestData.Invoke(level);
        }

        public void HandleSetTime(int time)
        {
            createNewTimeEvent.createNewTime.Invoke(new LeaderboardRow(playerName.playerName, time, level));
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
