using System;
using UnityEngine;
using HuaweiConstants;
using HuaweiMobileServices.Ads;
using static HuaweiConstants.UnityBannerAdPositionCode;

namespace HmsPlugin
{
    public class BannerAdsManager : MonoBehaviour
    {
        public string BANNER_AD_ID = "f0iaw23rfk";

        [HideInInspector] public bool _isInitialized;

        AdStatusListener mAdStatusListener;

        BannerAd bannerAdView = null;

        public event Action BannerLoaded;
        public event Action BannerFailedToLoad;

        public void LoadBannerAds(UnityBannerAdPositionCodeType position, string bannerSize = UnityBannerAdSize.BANNER_SIZE_320_50)
        {
            if (!_isInitialized) return;

            Debug.Log("[HMS] BannerAdManager LoadBanner Ads : ");

            mAdStatusListener = new AdStatusListener();
            mAdStatusListener.mOnAdLoaded += onAdLoadSuccess;
            mAdStatusListener.mOnAdClosed += onAdLoaClosed;
            mAdStatusListener.mOnAdImpression += onAdLoadImpression;
            mAdStatusListener.mOnAdClicked += mOnAdClicked;
            mAdStatusListener.mOnAdOpened += mOnAdOpened;
            mAdStatusListener.mOnAdFailed += mOnAdFailed;

            bannerAdView = new BannerAd(mAdStatusListener);
            bannerAdView.AdId = BANNER_AD_ID;
            bannerAdView.PositionType = (int)position;
            bannerAdView.SizeType = bannerSize;
            bannerAdView.AdStatusListener = mAdStatusListener;
            bannerAdView.LoadBanner(new AdParam.Builder().Build());
        }

        private void onAdLoadSuccess(object sender, EventArgs e)
        {
            Debug.Log("[HMS] BannerAdManager onAdLoadSuccess : ");

            BannerLoaded?.Invoke();
        }

        private void onAdLoaClosed(object sender, EventArgs e)
        {
            Debug.Log("[HMS] BannerAdManager onAdLoaClosed : ");
        }

        private void onAdLoadImpression(object sender, EventArgs e)
        {
            Debug.Log("[HMS] BannerAdManager onAdLoadImpression : ");
        }

        private void mOnAdClicked(object sender, EventArgs e)
        {
            Debug.Log("[HMS] BannerAdManager mOnAdClicked : ");
        }

        private void mOnAdFailed(object sender, EventArgs e)
        {
            Debug.Log("[HMS] BannerAdManager mOnAdFailed : ");

            BannerFailedToLoad?.Invoke();
        }

        private void mOnAdOpened(object sender, EventArgs e)
        {
            Debug.Log("[HMS] BannerAdManager mOnAdOpened : ");
        }

        void Start()
        {
            Debug.Log("[HMS] BannerAdManager Start");
            HwAds.Init();
            _isInitialized = true;

            LoadBannerAds(UnityBannerAdPositionCodeType.POSITION_BOTTOM);
            HideBannerAd();
        }

        public void ShowBannerAd()
        {
            if (bannerAdView == null)
            {
                Debug.Log("[HMS] Banner Ad is NULL");
                return;
            }

            bannerAdView.ShowBanner();
        }

        public void HideBannerAd()
        {
            if (bannerAdView == null)
            {
                Debug.Log("[HMS] Banner Ad is NULL");
                return;
            }
            bannerAdView.HideBanner();
        }
    }

}