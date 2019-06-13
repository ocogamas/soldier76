using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class ScreenTitle : MonoBehaviour
{
	#region Serialize Field
	
    [SerializeField] private AudioSource kickAudioSource;
    [SerializeField] private NetworkManager networkManager;

    [SerializeField] private Text informationText;
    [SerializeField] private Text stageText;

    [SerializeField] private GameObject titleRoot;
    [SerializeField] private GameObject menuRoot;
    
    #endregion

    private MasterStageRecordDataList masterStageRecordDataList;

    private int currentStageIndex = 0;

    // 譜面の名前をキーにして譜面を格納する
    private Dictionary<string, MasterMusicScoreRecordDataList> musicScoreDictionary;

    #region Mono
    
    void Start()
    {
        this.titleRoot.SetActive(true);
        this.menuRoot.SetActive(false);

        this.musicScoreDictionary = new Dictionary<string, MasterMusicScoreRecordDataList>();
    }
    
    #endregion // Mono
    

    #region Button

    /// <summary>
    /// タイトル画面のスタートボタン
    /// これを押したらマスタデータとか落とす
    /// </summary>
    public void OnClickTitleStartButton()
    {
        StartCoroutine(checkUpdate());
    }

    public void OnClickRGStartButton()
    {
        this.kickAudioSource.PlayOneShot(this.kickAudioSource.clip);

        MasterStageRecordData stageRecordData = this.masterStageRecordDataList.dataList[this.currentStageIndex];

        RhythmGameDataManager.masterStageRecordData = stageRecordData;
        RhythmGameDataManager.musicScoreRecordDataList = this.musicScoreDictionary[stageRecordData.stageName];

        StartCoroutine(loadSceneCoroutine("RhythmGame"));
    }

    public void OnClickMinusButton()
    {
        if (this.masterStageRecordDataList != null && this.masterStageRecordDataList.dataList != null)
        {
            if (this.currentStageIndex == 0)
            {
                this.currentStageIndex = this.masterStageRecordDataList.dataList.Count - 1;
            }
            else
            {
                this.currentStageIndex--;
            }

            this.stageText.text = this.masterStageRecordDataList.dataList[this.currentStageIndex].stageName;
        }
    }

    public void OnClickPlusButton()
    {
        if (this.masterStageRecordDataList != null && this.masterStageRecordDataList.dataList != null)
        {
            if (this.currentStageIndex == this.masterStageRecordDataList.dataList.Count - 1)
            {
                this.currentStageIndex = 0;
            }
            else
            {
                this.currentStageIndex++;
            }

            this.stageText.text = this.masterStageRecordDataList.dataList[this.currentStageIndex].stageName;
        }
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
        this.informationText.text = "MainSpreadSheet通信開始";
        yield return null;
        
        string mainSpreadSheet = this.networkManager.RequestMainSpreadSheet();

        this.informationText.text = "MainSpreadSheet通信成功";
        yield return null;
        
        ResponseObjectMasterStage masterStage = JsonFx.Json.JsonReader.Deserialize<ResponseObjectMasterStage>(mainSpreadSheet);

        masterStage.SetupEntry();

        this.masterStageRecordDataList = masterStage.GetDataList();

        if (this.masterStageRecordDataList != null)
        {
           　yield return StartCoroutine(downloadMusicScoreListIfNeeded(masterStage));
        }

        this.titleRoot.SetActive(false);
        this.menuRoot.SetActive(true);

        yield return null;
    }

    private IEnumerator downloadMusicScoreListIfNeeded(ResponseObjectMasterStage masterStage)
    {
        this.informationText.text = "SubSpreadSheet一覧取得の通信開始";
        yield return null;
        
        Dictionary<string, string> spreadSheetInfoDictionary = SpreadSheetInfoUtility.GetSpreadSheetInfoDictionary(this.networkManager);

        this.informationText.text = "SubSpreadSheet一覧取得の通信成功";
        yield return null;

        // TODO:kondo 
        // 必要があれば譜面をDLする
        // 各譜面のバージョン情報を保存しておき、
        // 保存していた値とDLした値が異なっていれば、
        // 譜面をダウンロードすること
        //
        //

        foreach (MasterStageRecordData recordData in this.masterStageRecordDataList.dataList)
        {         
        	this.informationText.text = "譜面読み込み中 " + recordData.stageName;
            yield return null;
 
            string sheetId = spreadSheetInfoDictionary[recordData.stageName];

            //Debug.Log_orange("downloadMusicScoreListIfNeeded > sheetId = " + sheetId, this);

            string url = this.networkManager.GetSpreadSheetURLWithSheetId(sheetId);
            string result = this.networkManager.Request(url, recordData.stageName);

            ResponseObjectMusicScore masterMusicScore = JsonFx.Json.JsonReader.Deserialize<ResponseObjectMusicScore>(result);
            masterMusicScore.SetupEntry();
            MasterMusicScoreRecordDataList scoreRecordDataList = masterMusicScore.GetDataList();

            this.musicScoreDictionary.Add(recordData.stageName, scoreRecordDataList);

            for (int i=0; i<scoreRecordDataList.dataList.Count; i++)
            {
                MasterMusicScoreRecordData scoreRecordData = scoreRecordDataList.dataList[i];
                //Debug.Log_cyan("downloadMusicScoreListIfNeeded > i=" + i + ", position = " + scoreRecordData.position + ", drum = " + scoreRecordData.drum, this);
            }
        }

        this.informationText.text = "譜面の読み込み完了";

        this.stageText.text = this.masterStageRecordDataList.dataList[this.currentStageIndex].stageName;


        yield return null;
    }

    #endregion // Private
}
