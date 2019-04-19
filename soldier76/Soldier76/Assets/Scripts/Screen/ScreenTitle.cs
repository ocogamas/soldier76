using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class ScreenTitle : MonoBehaviour
{
    [SerializeField] private AudioSource kickAudioSource;

    [SerializeField] private NetworkManager networkManager;

    private MasterStageRecordDataList masterStageRecordDataList;

    void Start()
    {
        Debug.Log_cyan("起動", this, 3);

        StartCoroutine(checkUpdate());
    }

    #region Button

    // ボタンをクリックするとingame.unityに移動します
    public void OnClickGameStartButton () 
    {
        this.kickAudioSource.PlayOneShot(this.kickAudioSource.clip);
        StartCoroutine(loadSceneCoroutine("ingame"));
    }

    public void OnClickRGStartButton()
    {
        this.kickAudioSource.PlayOneShot(this.kickAudioSource.clip);
        StartCoroutine(loadSceneCoroutine("RhythmGame"));

    }

    #endregion // Button


    #region Private

    private IEnumerator loadSceneCoroutine(string sceneName)
    {
        while (this.kickAudioSource.isPlaying)
        {
            yield return null;
        }
        SceneManager.LoadScene(sceneName);
    }

    private IEnumerator checkUpdate()
    {
        string mainSpreadSheet = this.networkManager.RequestMainSpreadSheet();

        ResponseObjectMasterStage masterStage = JsonFx.Json.JsonReader.Deserialize<ResponseObjectMasterStage>(mainSpreadSheet);

        masterStage.SetupEntry();

        this.masterStageRecordDataList = masterStage.GetDataList();

        if (this.masterStageRecordDataList != null)
        {
            StartCoroutine(downloadMusicScoreListIfNeeded(masterStage));
        }

        yield return null;
    }

    private IEnumerator downloadMusicScoreListIfNeeded(ResponseObjectMasterStage masterStage)
    {
        Dictionary<string, string> spreadSheetInfoDictionary = SpreadSheetInfoUtility.GetSpreadSheetInfoDictionary(this.networkManager);

        // TODO:kondo 
        // 必要があれば譜面をDLする
        // 各譜面のバージョン情報を保存しておき、
        // 保存していた値とDLした値が異なっていれば、
        // 譜面をダウンロードすること
        //
        //

        foreach (MasterStageRecordData recordData in this.masterStageRecordDataList.dataList)
        {         
 
            string sheetId = spreadSheetInfoDictionary[recordData.stageName];

            Debug.Log_orange("downloadMusicScoreListIfNeeded > sheetId = " + sheetId, this);

            string url = this.networkManager.GetSpreadSheetURLWithSheetId(sheetId);
            string result = this.networkManager.Request(url, recordData.stageName);

            ResponseObjectMusicScore masterMusicScore = JsonFx.Json.JsonReader.Deserialize<ResponseObjectMusicScore>(result);
            masterMusicScore.SetupEntry();
            MasterMusicScoreRecordDataList scoreRecordDataList = masterMusicScore.GetDataList();

            for (int i=0; i<scoreRecordDataList.dataList.Count; i++)
            {
                MasterMusicScoreRecordData scoreRecordData = scoreRecordDataList.dataList[i];
                Debug.Log_cyan("downloadMusicScoreListIfNeeded > i=" + i + ", position = " + scoreRecordData.position + ", drum = " + scoreRecordData.drum, this);
            }
        }


        yield return null;
    }

    #endregion // Private
}
