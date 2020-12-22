using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using HuaweiMobileServices.Ads;

namespace HmsPlugin
{
    public class RewardAdManager : MonoBehaviour
    {
        public string REWARD_AD_ID = "y826cynklq";

        public Action OnRewardAdClosedAction { get; set; }
        public Action<int> OnRewardAdFailedToShowAction { get; set; }
        public Action OnRewardAdOpenedAction { get; set; }
        public Action<Reward> OnRewardedAction { get; set; }

        private RewardAd rewardAd = null;

        void Start()
        {
            Debug.Log("[HMS] RewardAdManager Start");
            HwAds.Init();

            OnRewardAdClosedAction = OnRewardAdClosed;
            OnRewardAdFailedToShowAction = OnRewardAdFailedToShow;
            OnRewardAdOpenedAction = OnRewardAdOpened;
            OnRewardedAction = OnRewarded;

            LoadNextRewardedAd();
        }

        public void OnRewardAdClosed()
        {
            Debug.Log("[HMS] OnRewardAdClosed");
        }

        public void OnRewardAdFailedToShow(int id)
        {
            Debug.Log("[HMS] OnRewardAdFailedToShow " + id);
        }

        public void OnRewardAdOpened()
        {
            Debug.Log("[HMS] OnRewardAdOpened");
        }

        public void OnRewarded(Reward reward)
        {
            Debug.Log("[HMS] OnRewarded" + reward.Name + " - " + reward.Amount);
        }

        private void LoadNextRewardedAd()
        {
            Debug.Log("[HMS] AdsManager LoadNextRewardedAd");
            rewardAd = new RewardAd(REWARD_AD_ID);
            rewardAd.LoadAd(
                new AdParam.Builder().Build(),
                () => Debug.Log("[HMS] Rewarded ad loaded!"),
                (errorCode) => Debug.Log($"[HMS] Rewarded ad loading failed with error ${errorCode}")
            );
        }

        public void ShowRewardedAd()
        {
            Debug.Log("[HMS] AdsManager ShowRewardedAd");
            if (rewardAd?.Loaded == true)
            {
                Debug.Log("[HMS] AdsManager rewardAd.Show");
                rewardAd.Show(new RewardAdListener(this));
            }
            else
            {
                Debug.Log("[HMS] Reward ad clicked but still not loaded");
            }
        }

        private class RewardAdListener : IRewardAdStatusListener
        {
            private readonly RewardAdManager mAdsManager;

            public RewardAdListener(RewardAdManager adsManager)
            {
                mAdsManager = adsManager;
            }

            public void OnRewardAdClosed()
            {
                Debug.Log("[HMS] AdsManager OnRewardAdClosed");
                mAdsManager.OnRewardAdClosedAction?.Invoke();
                mAdsManager.LoadNextRewardedAd();
            }

            public void OnRewardAdFailedToShow(int errorCode)
            {
                Debug.Log("[HMS] AdsManager OnRewardAdFailedToShow " + errorCode);
                mAdsManager.OnRewardAdFailedToShowAction?.Invoke(errorCode);
            }

            public void OnRewardAdOpened()
            {
                Debug.Log("[HMS] AdsManager OnRewardAdOpened");
                mAdsManager.OnRewardAdOpenedAction?.Invoke();
            }

            public void OnRewarded(Reward reward)
            {
                Debug.Log("[HMS] AdsManager OnRewarded " + reward);
                mAdsManager.OnRewardedAction?.Invoke(reward);
            }
        }
    }
}
