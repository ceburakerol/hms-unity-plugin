using System;
using System.Collections.Generic;
using UnityEngine;
using HuaweiMobileServices.Base;
using HuaweiMobileServices.Game;
using HuaweiMobileServices.Id;
using HuaweiMobileServices.Utils;

namespace HmsPlugin
{
    public class AccountManager : MonoBehaviour
    {
        private static HuaweiIdAuthService DefaultAuthService
        {
            get
            {
                Debug.Log("[HMS]: GET AUTH");
                var authParams = new HuaweiIdAuthParamsHelper(HuaweiIdAuthParams.DEFAULT_AUTH_REQUEST_PARAM).SetIdToken().CreateParams();
                Debug.Log("[HMS]: AUTHPARAMS AUTHSERVICE " + authParams);
                var result = HuaweiIdAuthManager.GetService(authParams);
                Debug.Log("[HMS]: RESULT AUTHSERVICE "+ result);
                return result;
            }
        }

        private static HuaweiIdAuthService DefaultGameAuthService
        {
            get
            {
                IList<Scope> scopes = new List<Scope>();
                scopes.Add(GameScopes.DRIVE_APP_DATA);
                Debug.Log("[HMS]: GET AUTH GAME ");
                var authParams = new HuaweiIdAuthParamsHelper(HuaweiIdAuthParams.DEFAULT_AUTH_REQUEST_PARAM_GAME).SetScopeList(scopes).CreateParams();
                Debug.Log("[HMS]: AUTHPARAMS GAME " + authParams);
                var result = HuaweiIdAuthManager.GetService(authParams);
                Debug.Log("[HMS]: RESULT GAME " + result);
                return result;
            }
        }

        public IRankingsClient rankingsClient;

        private HuaweiIdAuthService authService;

        public AuthHuaweiId HuaweiId { get; set; }

        public Action<AuthHuaweiId> OnSignInSuccessAction { get; set; }
        public Action<HMSException> OnSignInFailedAction { get; set; }

        public void OnSignInSuccess(AuthHuaweiId authHuaweiId)
        {
            Debug.Log("[HMS]: SIGN IN SUCCESS " + authHuaweiId.DisplayName);
            rankingsClient = Games.GetRankingsClient(authHuaweiId);
        }

        public void OnSignInFailure(HMSException error)
        {
            Debug.Log("[HMS]: SIGN IN FAIL " + error.Message);
        }

        void Awake()
        {
            Debug.Log("[HMS]: AWAKE AUTHSERVICE");
            authService = DefaultAuthService;
        }

        private void Start()
        {
            OnSignInSuccessAction = OnSignInSuccess;
            OnSignInFailedAction = OnSignInFailure;
            HuaweiMobileServicesUtil.SetApplication();
        }

        public void SignIn()
        {
            Debug.Log("[HMS]: SIGNING IN " + authService);
            authService.StartSignIn((authId) =>
            {
                HuaweiId = authId;
                OnSignInSuccessAction?.Invoke(authId);
            }, (error) =>
            {
                HuaweiId = null;
                OnSignInFailedAction?.Invoke(error);
            });
        }

        public void SilentSign()
        {
            ITask<AuthHuaweiId> taskAuthHuaweiId = authService.SilentSignIn();
            taskAuthHuaweiId.AddOnSuccessListener((result) =>
            {
                HuaweiId = result;
                OnSignInSuccessAction?.Invoke(result);
            }).AddOnFailureListener((exception) =>
            {
                HuaweiId = null;
                OnSignInFailedAction?.Invoke(exception);
            });
        }

        public void SignOut()
        {
            Debug.Log("[HMS]: SIGNING OUT " + authService);
            authService.SignOut();
            HuaweiId = null;
        }

        public HuaweiIdAuthService GetGameAuthService()
        {
            return DefaultGameAuthService;
        }
    }
}
