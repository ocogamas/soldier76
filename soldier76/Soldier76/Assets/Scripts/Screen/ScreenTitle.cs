using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        // TODO:kondo 必要があれば譜面をDLする
        yield return null;
    }

    #endregion // Private
}
