using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HuaweiMobileServices;
using HuaweiMobileServices.Game;
using HuaweiMobileServices.Id;
using HuaweiMobileServices.Base;
using HuaweiMobileServices.Utils;
using System;
using HmsPlugin;
using UnityEngine.UI;

public class GameDemoManager : MonoBehaviour
{

    private bool achievements = true;
    private bool leaderboards = true;
    private bool customUnit = true;
    private const string MAX_FILE_SIZE = "Max File Size: {0}";
    private const string MAX_IMAGE_SIZE = "Max Image Size: {0}";


    IAchievementsClient achievementsClient;
    IRankingsClient rankingsClient;

    GameManager gameManager;
    LeaderboardManager leaderboardManager;
    AchievementsManager achievementsManager;
    SaveGameManager saveGameManager;
    private Text fileSize, imageSize;
    private InputField InputFieldDesc, InputFieldPlayedTime, InputFieldProgress;
    void Start()
    {
        gameManager = GetComponent<GameManager>();//GameManager.GetInstance();

        saveGameManager = GetComponent<SaveGameManager>();//SaveGameManager.GetInstance();

        leaderboardManager = GetComponent<LeaderboardManager>();//LeaderboardManager.GetInstance();

        achievementsManager = GetComponent<AchievementsManager>();//AchievementsManager.GetInstance();
        achievementsManager.OnShowAchievementsSuccess = OnShowAchievementsSuccess;
        achievementsManager.OnShowAchievementsFailure = OnShowAchievementsFailure;
        achievementsManager.OnRevealAchievementSuccess = OnRevealAchievementSuccess;
        achievementsManager.OnRevealAchievementFailure = OnRevealAchievementFailure;
        achievementsManager.OnIncreaseStepAchievementSuccess = OnIncreaseStepAchievementSuccess;
        achievementsManager.OnIncreaseStepAchievementFailure = OnIncreaseStepAchievementFailure;
        achievementsManager.OnUnlockAchievementSuccess = OnUnlockAchievementSuccess;
        achievementsManager.OnUnlockAchievementFailure = OnUnlockAchievementFailure;

        fileSize = GameObject.Find("FileSize").GetComponent<Text>();
        imageSize = GameObject.Find("ImageSize").GetComponent<Text>();
        InputFieldDesc = GameObject.Find("Description").GetComponent<InputField>();
        InputFieldPlayedTime = GameObject.Find("PlayedTime").GetComponent<InputField>();
        InputFieldProgress = GameObject.Find("Progress").GetComponent<InputField>();



    }
    public void GetMaxImageSize()
    {
        ITask<int> detailSizeTask = saveGameManager.GetMaxImageSize(); ;
        detailSizeTask.AddOnSuccessListener((result) =>
        {
            Debug.Log("[HMSP:] GetMaxImageSize Success " + result);
            imageSize.text = string.Format(MAX_IMAGE_SIZE, result);
        }).AddOnFailureListener((exception) =>
        {
            Debug.Log("[HMSP:] GetMaxImageSize Failed");
        });
    }
    public void GetMaxFileSize()
    {
        ITask<int> detailSizeTask = saveGameManager.GetMaxFileSize(); ;
        detailSizeTask.AddOnSuccessListener((result) =>
        {
            Debug.Log("[HMSP:] GetMaxFileSize Success " + result);
            fileSize.text = string.Format(MAX_FILE_SIZE, result);
        }).AddOnFailureListener((exception) =>
        {
            Debug.Log("[HMSP:] GetMaxFileSize Failed");
        });
    }
    public void CommitGame()
    {
        //Example Image Path: give statics path of image on phone
        string ImagePath = Application.streamingAssetsPath; 
        if( InputFieldDesc.text != null && InputFieldProgress.text != null && InputFieldPlayedTime.text != null)
        {
            string description = InputFieldDesc.text;
            long playedTime = long.Parse(InputFieldPlayedTime.text);
            long progress = long.Parse(InputFieldPlayedTime.text);
            saveGameManager.Commit(description, playedTime, progress, ImagePath, "png");
        }    
        else
            Debug.Log("[HMSP:] Fill box");
    }
    public void ShowArchive()
    {
        saveGameManager.ShowArchive();
    }

    // SHOW ACHIEVEMENTS
    public void ShowAchievements()
    {

        achievementsManager.ShowAchievements();
    }

    private void OnShowAchievementsSuccess()
    {
        Debug.Log("HMS Games: ShowAchievements SUCCESS ");
    }

    private void OnShowAchievementsFailure(HMSException exception)
    {
        Debug.Log("HMS Games: ShowAchievements ERROR ");
    }

    // GET ACHIEVEMENT LIST

    public void GetAchievementsList()
    {
        achievementsManager.GetAchievementsList();
        achievementsManager.OnGetAchievementsListSuccess = OnGetAchievemenListSuccess;
        achievementsManager.OnGetAchievementsListFailure = OnGetAchievementListFailure;

    }

    private void OnGetAchievemenListSuccess(IList<Achievement> achievementList)
    {
        Debug.Log("HMS Games: GetAchievementsList SUCCESS ");
    }

    private void OnGetAchievementListFailure(HMSException error)
    {
        Debug.Log("HMS Games: GetAchievementsList ERROR ");
    }

    // REVEAL ACHIEVEMENT
    public void RevealAchievement(string achievementId)
    {
        achievementsManager.RevealAchievement(achievementId);
    }

    public void OnRevealAchievementSuccess()
    {
        Debug.Log("HMS Games: RevealAchievement SUCCESS ");
    }

    private void OnRevealAchievementFailure(HMSException error)
    {
        Debug.Log("HMS Games: RevealAchievement ERROR ");
    }

    // INCREASE STEP ACHIEVEMENT
    public void IncreaseStepAchievement(string achievementId, int stepIncrement = 1)
    {
        achievementsManager.IncreaseStepAchievement(achievementId, stepIncrement);
    }

    private void OnIncreaseStepAchievementSuccess()
    {
        Debug.Log("HMS Games: IncreaseStepAchievement SUCCESS ");
    }

    private void OnIncreaseStepAchievementFailure(HMSException error)
    {
        Debug.Log("HMS Games: IncreaseStepAchievement ERROR ");
    }

    // Set Step Achivement
    public void SetStepAchievement(string achievementId, int stepsNum)
    {
        achievementsManager.SetStepAchievement(achievementId, stepsNum);
    }

    private void OnSetStepAchievementSuccess()
    {
        Debug.Log("HMS Games: SetStepAchievement SUCCESS ");
    }

    private void OnSetStepAchievemenFailure(HMSException error)
    {
        Debug.Log("HMS Games: SetStepAchievement ERROR ");
    }

    // Unlock Achievement

    public void UnlockAchievement(string achievementId)
    {
        achievementsManager.UnlockAchievement(achievementId);
    }

    private void OnUnlockAchievementSuccess()
    {
        Debug.Log("HMS Games: UnlockAchievement SUCCESS ");
    }

    private void OnUnlockAchievementFailure(HMSException error)
    {
        Debug.Log("HMS Games: UnlockAchievement ERROR ");
    }

    /******************  LEADERBOARDS  ********************/

}
