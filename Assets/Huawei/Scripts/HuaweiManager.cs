using System;
using HuaweiMobileServices.Base;
using HuaweiMobileServices.Game;
using HuaweiMobileServices.Id;
using HuaweiMobileServices.Utils;
using UnityEngine;

namespace HmsPlugin
{
    public class HuaweiManager : MonoBehaviour
    {
        #region Variables

        [HideInInspector] public AccountManager accountManager;
        [HideInInspector] public BannerAdsManager bannerAdsManager;
        [HideInInspector] public InterstitialAdManager interstitialAdManager;
        [HideInInspector] public RewardAdManager rewardAdManager;
        [HideInInspector] public LeaderboardManager leaderboardManager;
        [HideInInspector] public IapManager iapManager;
        [HideInInspector] public SaveGameManager saveGameManager;

        private static HuaweiManager _instance;
        public static HuaweiManager Instance { get { return _instance; } }

        private HuaweiIdAuthService authService;

        #endregion

        #region Unity Events

        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            }
            else
            {
                _instance = this;
            }
        }

        private void Start()
        {
            Managers();
            Activate();
        }

        #endregion

        public void Managers()
        {
            accountManager = GetComponent<AccountManager>();
            bannerAdsManager = GetComponent<BannerAdsManager>();
            interstitialAdManager = GetComponent<InterstitialAdManager>();
            rewardAdManager = GetComponent<RewardAdManager>();
            leaderboardManager = GetComponent<LeaderboardManager>();
            iapManager = GetComponent<IapManager>();
            saveGameManager = GetComponent<SaveGameManager>();
        }

        public Action<AuthHuaweiId> SignInSuccess { get; set; }
        public Action<HMSException> SignInFailure { get; set; }

        public void Activate()
        {
            HuaweiMobileServicesUtil.SetApplication();
            authService = accountManager.GetGameAuthService();

            ITask<AuthHuaweiId> taskAuthHuaweiId = authService.SilentSignIn();
            taskAuthHuaweiId.AddOnSuccessListener((result) =>
            {
                accountManager.HuaweiId = result;
                IJosAppsClient josAppsClient = JosApps.GetJosAppsClient(accountManager.HuaweiId);
                josAppsClient.Init();
                PrepareLeaderboardManager();
                PrepareSaveGameManager();

            }).AddOnFailureListener((exception) =>
            {
                authService.StartSignIn(SignInSuccess, SignInFailure);
                PrepareLeaderboardManager();
                PrepareSaveGameManager();
            });
        }

        public void PrepareLeaderboardManager()
        {
            leaderboardManager.rankingsClient = Games.GetRankingsClient(accountManager.HuaweiId);
        }

        public void PrepareSaveGameManager()
        {
            saveGameManager.archiveClient = Games.GetArchiveClient(accountManager.HuaweiId);
        }

        public Action<Player> OnGetPlayerInfoSuccess { get; set; }
        public Action<HMSException> OnGetPlayerInfoFailure { get; set; }

        public void GetPlayerInfo()
        {
            if (accountManager.HuaweiId != null)
            {
                IPlayersClient playersClient = Games.GetPlayersClient(accountManager.HuaweiId);
                ITask<Player> task = playersClient.CurrentPlayer;
                task.AddOnSuccessListener((result) =>
                {
                    Debug.Log("[HMSP:] GetPlayerInfo Success");
                    OnGetPlayerInfoSuccess?.Invoke(result);

                }).AddOnFailureListener((exception) =>
                {
                    Debug.Log("[HMSP:] GetPlayerInfo Failed");
                    OnGetPlayerInfoFailure?.Invoke(exception);

                });
            }
        }
    }
}