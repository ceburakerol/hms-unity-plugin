using HuaweiMobileServices.Base;
using HuaweiMobileServices.Game;
using HuaweiMobileServices.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HmsPlugin
{
    public class LeaderboardManager : MonoBehaviour
    {
        #region Variables

        public IRankingsClient rankingsClient;

        #endregion

        #region Actions

        public Action<int> OnIsUserScoreShownOnLeaderboardsSuccessAction { get; set; }
        public Action<HMSException> OnIsUserScoreShownOnLeaderboardsFailureAction { get; set; }

        public Action<int> OnSetUserScoreShownOnLeaderboardsSuccessAction { get; set; }
        public Action<HMSException> OnSetUserScoreShownOnLeaderboardsFailureAction { get; set; }

        public Action OnShowLeaderboardsSuccessAction { get; set; }
        public Action<HMSException> OnShowLeaderboardsFailureAction { get; set; }

        public Action<IList<Ranking>> OnGetLeaderboardsDataSuccessAction { get; set; }
        public Action<HMSException> OnGetLeaderboardsDataFailureAction { get; set; }

        public Action<Ranking> OnGetLeaderboardDataSuccessAction { get; set; }
        public Action<HMSException> OnGetLeaderboardDataFailureAction { get; set; }

        public Action<RankingScores> OnGetScoresFromLeaderboardSuccessAction { get; set; }
        public Action<HMSException> OnGetScoresFromLeaderboardFailureAction { get; set; }

        public Action<ScoreSubmissionInfo> OnSubmitScoreSuccessAction { get; set; }
        public Action<HMSException> OnSubmitScoreFailureAction { get; set; }

        #endregion

        #region Events

        private void OnIsUserScoreShownOnLeaderboardsSuccess(int id)
        {
            Debug.Log("HMS Games: GetUserScoreShownOnLeaderboards SUCCESS " + id);
        }

        private void OnIsUserScoreShownOnLeaderboardsFailure(HMSException exception)
        {
            Debug.Log("HMS Games: GetUserScoreShownOnLeaderboards ERROR " + exception.Message);
        }

        private void OnSetUserScoreShownOnLeaderboardsSuccess(int id)
        {
            Debug.Log("HMS Games: SetUserScoreShownOnLeaderboards SUCCESS " + id);
        }

        private void OnSetUserScoreShownOnLeaderboardsFailure(HMSException exception)
        {
            Debug.Log("HMS Games: SetUserScoreShownOnLeaderboards ERROR " + exception.Message);
        }

        private void OnShowLeaderboardsSuccess()
        {
            Debug.Log("HMS Games: ShowLeaderboards SUCCESS ");
        }

        private void OnShowLeaderboardsFailure(HMSException exception)
        {
            Debug.Log("HMS Games: ShowLeaderboards ERROR " + exception.Message);
        }

        private void OnSubmitScoreSuccess(ScoreSubmissionInfo scoreSubmission)
        {
            Debug.Log("HMS Games: SubmitScore SUCCESS " + scoreSubmission.PlayerId);
        }

        private void OnSubmitScoreFailure(HMSException exception)
        {
            Debug.Log("HMS Games: SubmitScore ERROR " + exception.Message);
        }

        private void OnGetLeaderboardsDataSuccess(IList<Ranking> ranking)
        {
            Debug.Log("HMS Games: GetLeaderboardsData SUCCESS " + ranking);
        }

        private void OnGetLeaderboardsDataFailure(HMSException exception)
        {
            Debug.Log("HMS Games: GetLeaderboardsData ERROR " + exception.Message);
        }

        private void OnGetLeaderboardDataSuccess(Ranking ranking)
        {
            Debug.Log("HMS Games: GetLeaderboardsData SUCCESS " + ranking.RankingDisplayName);
        }

        private void OnGetLeaderboardDataFailure(HMSException exception)
        {
            Debug.Log("HMS Games: OnGetLeaderboardDataFailure " + exception.Message);
        }

        private void OnGetScoresFromLeaderboardSuccess(RankingScores rankingScores)
        {
            Debug.Log("HMS Games: GetScoresFromLeaderboard SUCCESS " + rankingScores.RankingScore);
        }

        private void OnGetScoresFromLeaderboardFailure(HMSException exception)
        {
            Debug.Log("HMS Games: GetScoresFromLeaderboard ERROR " + exception.Message);
        }

        #endregion

        #region Unity Events

        public void Start()
        {
            OnIsUserScoreShownOnLeaderboardsSuccessAction = OnIsUserScoreShownOnLeaderboardsSuccess;
            OnIsUserScoreShownOnLeaderboardsFailureAction = OnIsUserScoreShownOnLeaderboardsFailure;

            OnSetUserScoreShownOnLeaderboardsSuccessAction = OnSetUserScoreShownOnLeaderboardsSuccess;
            OnSetUserScoreShownOnLeaderboardsFailureAction = OnSetUserScoreShownOnLeaderboardsFailure;

            OnShowLeaderboardsSuccessAction = OnShowLeaderboardsSuccess;
            OnShowLeaderboardsFailureAction = OnShowLeaderboardsFailure;

            OnSubmitScoreSuccessAction = OnSubmitScoreSuccess;
            OnSubmitScoreFailureAction = OnSubmitScoreFailure;

            OnGetLeaderboardsDataSuccessAction = OnGetLeaderboardsDataSuccess;
            OnGetLeaderboardsDataFailureAction = OnGetLeaderboardsDataFailure;

            OnGetLeaderboardDataSuccessAction = OnGetLeaderboardDataSuccess;
            OnGetLeaderboardDataFailureAction = OnGetLeaderboardDataFailure;

            OnGetScoresFromLeaderboardSuccessAction = OnGetScoresFromLeaderboardSuccess;
            OnGetScoresFromLeaderboardFailureAction = OnGetScoresFromLeaderboardFailure;
        }

        #endregion

        public void IsUserScoreShownOnLeaderboards()
        {
            ITask<int> task = rankingsClient.GetRankingSwitchStatus();
            task.AddOnSuccessListener((result) =>
            {
                Debug.Log("[HMS GAMES] isUserScoreShownOnLeaderboards SUCCESS" + result);
                OnIsUserScoreShownOnLeaderboardsSuccessAction?.Invoke(result);

            }).AddOnFailureListener((exception) =>
            {
                Debug.Log("[HMS GAMES] isUserScoreShownOnLeaderboards ERROR");
                OnIsUserScoreShownOnLeaderboardsFailureAction?.Invoke(exception);
            });
        }

        public void SetUserScoreShownOnLeaderboards(int active)
        {
            ITask<int> task = rankingsClient.SetRankingSwitchStatus(active);
            task.AddOnSuccessListener((result) =>
            {
                Debug.Log("[HMS GAMES] SetUserScoreShownOnLeaderboards SUCCESS" + result);
                OnSetUserScoreShownOnLeaderboardsSuccessAction?.Invoke(result);

            }).AddOnFailureListener((exception) =>
            {
                Debug.Log("[HMS GAMES] SetUserScoreShownOnLeaderboards ERROR");
                OnSetUserScoreShownOnLeaderboardsFailureAction?.Invoke(exception);
            });
        }

        public void SubmitScore_Click()
        {
            SubmitScore("19910B6BDF499E6E6E64247827946415C86202FC38A56B8FE03CA3BA09A0AA40", 9999);
        }

        public void SubmitScore(string leaderboardId, long score)
        {
            ITask<ScoreSubmissionInfo> task = rankingsClient.SubmitScoreWithResult(leaderboardId, score);
            task.AddOnSuccessListener((scoreInfo) =>
            {
                OnSubmitScoreSuccessAction?.Invoke(scoreInfo);
            }).AddOnFailureListener((error) =>
            {
                OnSubmitScoreFailureAction?.Invoke(error);
            });
        }

        public void SubmitScore(string leaderboardId, long score, string scoreTips)
        {
            ITask<ScoreSubmissionInfo> task = rankingsClient.SubmitScoreWithResult(leaderboardId, score, scoreTips);
            task.AddOnSuccessListener((scoreInfo) =>
            {
                OnSubmitScoreSuccessAction?.Invoke(scoreInfo);
            }).AddOnFailureListener((error) =>
            {
                OnSubmitScoreFailureAction?.Invoke(error);
            });
        }

        public void ShowLeaderboards()
        {
            rankingsClient.ShowTotalRankings(() =>
            {
                Debug.Log("[HMS GAMES] ShowLeaderboards SUCCESS");
                OnShowLeaderboardsSuccessAction?.Invoke();

            }, (exception) =>
            {
                Debug.Log("[HMS GAMES] ShowLeaderboards ERROR");
                OnShowLeaderboardsFailureAction?.Invoke(exception);
            });
        }

        public void GetLeaderboardsData()
        {
            ITask<IList<Ranking>> task = rankingsClient.GetRankingSummary(true);
            task.AddOnSuccessListener((result) =>
            {
                Debug.Log("[HMS GAMES] GetLeaderboardsData SUCCESS");
                OnGetLeaderboardsDataSuccessAction?.Invoke(result);
            }).AddOnFailureListener((exception) =>
            {
                Debug.Log("[HMS GAMES] GetLeaderboardsData ERROR");
                OnGetLeaderboardsDataFailureAction?.Invoke(exception);
            });
        }

        public void GetLeaderboardData(string leaderboardId)
        {
            ITask<Ranking> task = rankingsClient.GetRankingSummary(leaderboardId, true);
            task.AddOnSuccessListener((result) =>
            {
                Debug.Log("[HMS GAMES] GetLeaderboardsData SUCCESS");
                OnGetLeaderboardDataSuccessAction?.Invoke(result);

            }).AddOnFailureListener((exception) =>
            {
                Debug.Log("[HMS GAMES] GetLeaderboardsData ERROR");
                OnGetLeaderboardDataFailureAction?.Invoke(exception);
            });
        }

        public void GetScoresFromLeaderboard(string leaderboardId, int timeDimension, int maxResults, int offsetPlayerRank, int pageDirection)
        {

            ITask<RankingScores> task = rankingsClient.GetRankingTopScores(leaderboardId, timeDimension, maxResults, offsetPlayerRank, pageDirection);
            task.AddOnSuccessListener((result) =>
            {
                Debug.Log("[HMS GAMES] GetScoresFromLeaderboard SUCCESS");
                OnGetScoresFromLeaderboardSuccessAction?.Invoke(result);

            }).AddOnFailureListener((exception) =>
            {
                Debug.Log("[HMS GAMES] GetScoresFromLeaderboard ERROR");
                OnGetScoresFromLeaderboardFailureAction?.Invoke(exception);
            });
        }
    }
}