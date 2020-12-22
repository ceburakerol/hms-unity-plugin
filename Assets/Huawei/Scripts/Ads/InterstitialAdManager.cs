using HuaweiMobileServices.Ads;
using System;
using UnityEngine;

namespace HmsPlugin
{
    public class InterstitialAdManager : MonoBehaviour
    {
        public string INTERSTITIAL_AD_ID = "z1e50gcdqr";

        public Action OnAdClickedAction { get; set; }
        public Action OnAdClosedAction { get; set; }
        public Action<int> OnAdFailedAction { get; set; }
        public Action OnAdImpressionAction { get; set; }
        public Action OnAdLeaveAction { get; set; }
        public Action OnAdLoadedAction { get; set; }
        public Action OnAdOpenedAction { get; set; }

        private InterstitialAd interstitialAd = null;

        void Start()
        {
            Debug.Log("[HMS] InterstitalAdManager Start");
            HwAds.Init();

            OnAdClickedAction = OnAdClicked;
            OnAdClosedAction = OnAdClosed;
            OnAdFailedAction = OnAdFailed;
            OnAdImpressionAction = OnAdImpression;
            OnAdLeaveAction = OnAdLeave;
            OnAdLoadedAction = OnAdLoaded;
            OnAdOpenedAction = OnAdOpened;

            LoadNextInterstitialAd();
        }

        public void OnAdClicked()
        {
            Debug.Log("[HMS] OnAdClicked");
        }

        public void OnAdClosed()
        {
            Debug.Log("[HMS] OnAdClosed");
        }

        public void OnAdFailed(int id)
        {
            Debug.Log("[HMS] OnAdFailed " + id);
        }

        public void OnAdImpression()
        {
            Debug.Log("[HMS] OnAdImpression");
        }

        public void OnAdLeave()
        {
            Debug.Log("[HMS] OnAdLeave");
        }

        public void OnAdLoaded()
        {
            Debug.Log("[HMS] OnAdLoaded");
        }

        public void OnAdOpened()
        {
            Debug.Log("[HMS] OnAdOpened");
        }

        public void LoadNextInterstitialAd()
        {
            Debug.Log("[HMS] InterstitalAdManager LoadNextInterstitialAd");
            interstitialAd = new InterstitialAd
            {
                AdId = INTERSTITIAL_AD_ID,
                AdListener = new InterstitialAdListener(this)
            };
            interstitialAd.LoadAd(new AdParam.Builder().Build());
        }

        public void ShowInterstitialAd()
        {
            Debug.Log("[HMS] InterstitialAdManager ShowInterstitialAd");
            if (interstitialAd?.Loaded == true)
            {
                Debug.Log("[HMS] InterstitalAdManager interstitialAd.Show");
                interstitialAd.Show();
            }
            else
            {
                Debug.Log("[HMS] Interstitial ad clicked but still not loaded");
            }
        }

        private class InterstitialAdListener : IAdListener
        {
            private readonly InterstitialAdManager mAdsManager;

            public InterstitialAdListener(InterstitialAdManager adsManager)
            {
                mAdsManager = adsManager;
            }

            public void OnAdClicked()
            {
                Debug.Log("[HMS] AdsManager OnAdClicked");
                mAdsManager.OnAdClickedAction?.Invoke();
            }

            public void OnAdClosed()
            {
                Debug.Log("[HMS] AdsManager OnAdClosed");
                mAdsManager.OnAdClosedAction?.Invoke();
                mAdsManager.LoadNextInterstitialAd();
            }

            public void OnAdFailed(int reason)
            {
                Debug.Log("[HMS] AdsManager OnAdFailed");
                mAdsManager.OnAdFailedAction?.Invoke(reason);
            }

            public void OnAdImpression()
            {
                Debug.Log("[HMS] AdsManager OnAdImpression");
                mAdsManager.OnAdImpressionAction?.Invoke();
            }

            public void OnAdLeave()
            {
                Debug.Log("[HMS] AdsManager OnAdLeave");
                mAdsManager.OnAdLeaveAction?.Invoke();
            }

            public void OnAdLoaded()
            {
                Debug.Log("[HMS] AdsManager OnAdLoaded");
                mAdsManager.OnAdLoadedAction?.Invoke();
            }

            public void OnAdOpened()
            {
                Debug.Log("[HMS] AdsManager OnAdOpened");
                mAdsManager.OnAdOpenedAction?.Invoke();
            }
        }
    }
}