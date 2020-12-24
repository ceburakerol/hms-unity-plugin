using HuaweiMobileServices.Base;
using HuaweiMobileServices.Game;
using HuaweiMobileServices.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace HmsPlugin
{
    public class SaveGameManager : MonoBehaviour
    {
        public IArchivesClient archiveClient;

        public void CommitGame()
        {
            Commit("TestSaveGameJSON", 100, 100, Application.streamingAssetsPath, "png", "{\"ID\":1,\"Name\":\"Burak\",\"Address\":\"Turkey\"}");
        }

        public void Commit(string description, long playedTime, long progress, string ImagePath, string imageType, string Json)
        {
            if (archiveClient == null) archiveClient = Games.GetArchiveClient(HuaweiManager.Instance.accountManager.HuaweiId);

            AndroidBitmap testBitmap = new AndroidBitmap(AndroidBitmapFactory.DecodeFile(ImagePath));
            ArchiveSummaryUpdate archiveSummaryUpdate = new ArchiveSummaryUpdate.Builder().SetActiveTime(playedTime)
                .SetCurrentProgress(progress)
                .SetDescInfo(description)
                .SetThumbnail(testBitmap)
                .SetThumbnailMimeType(imageType)
                .Build();
            ArchiveDetails archiveContents = new ArchiveDetails.Builder().Build();
            byte[] byteArray = Encoding.UTF8.GetBytes(Json); //TODO : Serialize Save JSON
            archiveContents.Set(byteArray);
            bool isSupportCache = true;
            ITask<ArchiveSummary> addArchiveTask = archiveClient.AddArchive(archiveContents, archiveSummaryUpdate, isSupportCache);
            ArchiveSummary archiveSummary = null;
            addArchiveTask.AddOnSuccessListener((result) =>
            {
                archiveSummary = result;
                if (archiveSummary != null)
                {
                    Debug.Log("[HMSP:] AddArchive archiveSummary " + archiveSummary.FileName);
                }
            }).AddOnFailureListener((exception) =>
            {
                Debug.Log("[HMS:] AddArchive fail: " + exception.ErrorCode + " :: " + exception.WrappedExceptionMessage + " ::  " + exception.WrappedCauseMessage);
            });
        }

        public void ShowArchive()
        {
            if (archiveClient == null) archiveClient = Games.GetArchiveClient(HuaweiManager.Instance.accountManager.HuaweiId);

            bool param = true;
            ITask<IList<ArchiveSummary>> taskDisplay = archiveClient.GetArchiveSummaryList(param);
            taskDisplay.AddOnSuccessListener((result) =>
            {
                if (result == null)
                    Debug.Log("[HMS:]Archive Summary is null ");
                if (result.Count > 0)
                    Debug.Log("[HMS:]Archive Summary List size " + result.Count);

                string title = "";
                bool allowAddBtn = true, allowDeleteBtn = true;
                int maxArchive = 100;
                archiveClient.ShowArchiveListIntent(title, allowAddBtn, allowDeleteBtn, maxArchive);

            }).AddOnFailureListener((exception) =>
            {
                Debug.Log("[HMS:] ShowArchive ERROR " + exception.WrappedExceptionMessage);
            });
        }

        private void HandleDifference(OperationResult operationResult)
        {
            if (archiveClient == null) archiveClient = Games.GetArchiveClient(HuaweiManager.Instance.accountManager.HuaweiId);

            if (operationResult != null)
            {
                Difference archiveDifference = operationResult.Difference;
                Archive openedArchive = archiveDifference.RecentArchive();
                Archive serverArchive = archiveDifference.ServerArchive;
                if (serverArchive == null)
                {
                    return;
                }
                ITask<OperationResult> task = archiveClient.UpdateArchive(serverArchive);
                task.AddOnSuccessListener((result) =>
                {
                    Debug.Log("OperationResult:" + ((operationResult == null) ? "" : operationResult.Different.ToString()));
                    if (operationResult != null && !operationResult.Different)
                    {
                        Archive archive = operationResult.Difference.RecentArchive();
                        if (archive != null && archive.Summary != null)
                            Debug.Log("OperationResult:" + archive.Summary.Id);
                        else
                            HandleDifference(operationResult);
                    }
                }).AddOnFailureListener((exception) =>
                {
                    Debug.Log("[HMS:] HandleDifference ERROR " + exception.ErrorCode);
                });
            }
        }

        public void UpdateSavedGame(String archiveID,ArchiveSummaryUpdate archiveSummaryUpdate, ArchiveDetails archiveContents)
        {
            if (archiveClient == null) archiveClient = Games.GetArchiveClient(HuaweiManager.Instance.accountManager.HuaweiId);

            String archiveId = archiveID;
            ITask<OperationResult> taskUpdateArchive = archiveClient.UpdateArchive(archiveId, archiveSummaryUpdate, archiveContents);
            taskUpdateArchive.AddOnSuccessListener((archiveDataOrConflict) =>
            {
                Debug.Log("[HMS:] taskUpdateArchive" + archiveDataOrConflict.Difference);
                Debug.Log("isDifference:" + ((archiveDataOrConflict == null) ? "" : archiveDataOrConflict.Difference.ToString()));
            }).AddOnFailureListener((exception) =>
            {
                Debug.Log("[HMS:] UpdateSavedGame ERROR " + exception.ErrorCode);
            });
        }

        public void LoadingSavedGame(String archiveId)
        {
            if (archiveClient == null) archiveClient = Games.GetArchiveClient(HuaweiManager.Instance.accountManager.HuaweiId);

            int conflictPolicy = getConflictPolicy();
            ITask<OperationResult> taskLoadSavedGame;
            if (conflictPolicy == -1)
            {
                taskLoadSavedGame = archiveClient.LoadArchiveDetails(archiveId);
            }
            else
            {
                taskLoadSavedGame = archiveClient.LoadArchiveDetails(archiveId, conflictPolicy);
            }
            taskLoadSavedGame.AddOnSuccessListener((archiveDataOrConflict) =>
            {
                Debug.Log("[HMS:] taskUpdateArchive" + archiveDataOrConflict.Difference);
                Debug.Log("isDifference:" + ((archiveDataOrConflict == null) ? "" : archiveDataOrConflict.Difference.ToString()));
                Debug.Log("Archive Content : " + archiveDataOrConflict.Archive.Details.ToString()); //TODO : Deserialize Save JSON
            }).AddOnFailureListener((exception) =>
            {
                Debug.Log("[HMS:] LoadingSavedGame ERROR " + exception.ErrorCode);
            });
        }

        public void DeleteSavedGames(ArchiveSummary archiveSummary)
        {
            if (archiveClient == null) archiveClient = Games.GetArchiveClient(HuaweiManager.Instance.accountManager.HuaweiId);

            ITask<String> removeArchiveTask = archiveClient.RemoveArchive(archiveSummary);
            removeArchiveTask.AddOnSuccessListener((result) =>
            {
                String deletedArchiveId = result;
                Debug.Log("[HMS:] deletedArchiveId" + result);

            }).AddOnFailureListener((exception) =>
            {
                Debug.Log("[HMS:] DeleteSavedGames ERROR " + exception.ErrorCode);
            });
        }

        public void LoadThumbnail(Archive archive)
        {
            if (archiveClient == null) archiveClient = Games.GetArchiveClient(HuaweiManager.Instance.accountManager.HuaweiId);

            if (archive.Summary.HasThumbnail())
            {
                ITask<AndroidBitmap> coverImageTask = archiveClient.GetThumbnail(archive.Summary.Id);
                coverImageTask.AddOnSuccessListener((result) =>
                {
                    Debug.Log("[HMS:] AndroidBitmap put it UI");


                }).AddOnFailureListener((exception) =>
                {
                    Debug.Log("[HMS:] LoadThumbnail ERROR " + exception.ErrorCode);
                });
            }
        }

        public void GetMaxImageSize()
        {
            if (archiveClient == null) archiveClient = Games.GetArchiveClient(HuaweiManager.Instance.accountManager.HuaweiId);

            ITask<int> detailSizeTask = archiveClient.LimitThumbnailSize;
            detailSizeTask.AddOnSuccessListener((result) =>
            {
                Debug.Log("[HMSP:] GetMaxImageSize Success " + result);
            }).AddOnFailureListener((exception) =>
            {
                Debug.Log("[HMSP:] GetMaxImageSize Failed");
            });
        }
        public void GetMaxFileSize()
        {
            if (archiveClient == null) archiveClient = Games.GetArchiveClient(HuaweiManager.Instance.accountManager.HuaweiId);

            ITask<int> detailSizeTask = archiveClient.LimitDetailsSize;
            detailSizeTask.AddOnSuccessListener((result) =>
            {
                Debug.Log("[HMSP:] GetMaxFileSize Success " + result);
            }).AddOnFailureListener((exception) =>
            {
                Debug.Log("[HMSP:] GetMaxFileSize Failed");
            });
        }

        private int getConflictPolicy()
        {
            return 0;
        }
    }
}