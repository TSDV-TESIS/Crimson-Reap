using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Leaderboard
{
    public class LeaderboardManager : MonoBehaviour
    {
        private static readonly string LeaderboardURL = "https://crimson-leaderboard.onrender.com";

        [SerializeField] private LeaderboardData leaderboardData;

        public void OnEnable()
        {
            leaderboardData.requestData.AddListener(RequestData);
        }

        public void OnDisable()
        {
            leaderboardData.requestData.RemoveListener(RequestData);
        }

        public void RequestData(LevelEnum level)
        {
            leaderboardData.isLoading = true;
            StartCoroutine(WebTestRequest(GetLevelNameByLevelEnum(level)));
        }

        private string GetLevelNameByLevelEnum(LevelEnum level)
        {
            switch (level)
            {
                case LevelEnum.Level1:
                    return LevelNames.Level1;
                case LevelEnum.Level2:
                    return LevelNames.Level2;
                case LevelEnum.Level3:
                    return LevelNames.Level3;
                case LevelEnum.Level4:
                    return LevelNames.Level4;
                default:
                    Debug.LogError("Level enum not specified correctly");
                    return "";
            }
        }

        private IEnumerator WebTestRequest(string levelName)
        {
            UnityWebRequest getLeaderboardRequest = UnityWebRequest.Get(LeaderboardURL + "/leaderboard?type=" + levelName);
            yield return getLeaderboardRequest.SendWebRequest();

            switch (getLeaderboardRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError("There was an error obtaining the leaderboard data.");
                    leaderboardData.hasError = true;
                    leaderboardData.isLoading = false;
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError("There was an HTTP error obtaining the leaderboard data.");
                    leaderboardData.hasError = true;
                    leaderboardData.isLoading = false;
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log($"Received: " + getLeaderboardRequest.downloadHandler.text);
                    GetLeaderboardResponse leaderboardResponse =
                        JsonUtility.FromJson<GetLeaderboardResponse>(getLeaderboardRequest.downloadHandler.text);

                    leaderboardData.data = leaderboardResponse.data;
                    leaderboardData.leaderboardLevel = levelName;
                    leaderboardData.isLoading = false;
                    leaderboardData.hasError = false;
                    
                    break;
            }
        }
    }
}
