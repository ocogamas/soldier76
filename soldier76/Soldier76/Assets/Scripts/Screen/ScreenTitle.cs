﻿using System.Collections;
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

    

    private int currentStageIndex = 0;


    #region Mono
    
    void Start()
    {
        this.titleRoot.SetActive(true);
        this.menuRoot.SetActive(false);
        
        // 譜面を読み込み済みの場合はタイトルをすっ飛ばす
    	if (RhythmGameDataManager.musicScoreDictionary.Count > 0)
    	{   		
    		this.titleRoot.SetActive(false);
    		this.menuRoot.SetActive(true);    		

    		this.informationText.text = "譜面の読み込み完了";

    		this.stageText.text = RhythmGameDataManager.masterStageRecordDataList.dataList[this.currentStageIndex].stageName;
    	}
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

        MasterStageRecordData stageRecordData = RhythmGameDataManager.masterStageRecordDataList.dataList[this.currentStageIndex];

        RhythmGameDataManager.masterStageRecordData = stageRecordData;
        RhythmGameDataManager.musicScoreRecordDataList = RhythmGameDataManager.musicScoreDictionary[stageRecordData.stageName];

        StartCoroutine(loadSceneCoroutine("RhythmGame"));
    }

    public void OnClickMinusButton()
    {
        if (RhythmGameDataManager.masterStageRecordDataList != null && 
    	    RhythmGameDataManager.masterStageRecordDataList.dataList != null)
        {
            if (this.currentStageIndex == 0)
            {
                this.currentStageIndex = RhythmGameDataManager.masterStageRecordDataList.dataList.Count - 1;
            }
            else
            {
                this.currentStageIndex--;
            }

            this.stageText.text = RhythmGameDataManager.masterStageRecordDataList.dataList[this.currentStageIndex].stageName;
        }
    }

    public void OnClickPlusButton()
    {
        if (RhythmGameDataManager.masterStageRecordDataList != null &&
    	    RhythmGameDataManager.masterStageRecordDataList.dataList != null)
        {
            if (this.currentStageIndex == RhythmGameDataManager.masterStageRecordDataList.dataList.Count - 1)
            {
                this.currentStageIndex = 0;
            }
            else
            {
                this.currentStageIndex++;
            }

            this.stageText.text = RhythmGameDataManager.masterStageRecordDataList.dataList[this.currentStageIndex].stageName;
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

        RhythmGameDataManager.masterStageRecordDataList = masterStage.GetDataList();

        if (RhythmGameDataManager.masterStageRecordDataList != null)
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
        
        UpdateCheckSaveDataList updateCheckSaveDataList = DataManager.Load<UpdateCheckSaveDataList>(DataManager.UPDATE_INFO);
        if (updateCheckSaveDataList == null)
        {
        	updateCheckSaveDataList = new UpdateCheckSaveDataList();
        	updateCheckSaveDataList.dataList = new List<UpdateCheckSaveData>();
        }

        foreach (MasterStageRecordData recordData in RhythmGameDataManager.masterStageRecordDataList.dataList)
        { 
        	bool isSkip = false;
        	UpdateCheckSaveData updateCheckSaveData = null;
        	// 保存されているデータバージョンと同じ譜面ならスキップ
        	foreach (UpdateCheckSaveData data in updateCheckSaveDataList.dataList)
        	{
        		if (data.stageName == recordData.stageName)
        		{
        			if (data.version == recordData.version)
        			{
        				//Debug.Log_blue("譜面読み込みをスキップ " + recordData.stageName, this);
        				isSkip = true;
        			}
        			updateCheckSaveData = data;
        			break;
        		}
        	}
        	
        	if (isSkip)
        	{
        		continue;
        	}
        	
        	// バージョンを更新
        	if (updateCheckSaveData != null)
        	{
        		Debug.Log_blue("バージョンを更新 " + recordData.stageName + ", version = " + recordData.version, this);
        		updateCheckSaveData.version = recordData.version;
        	}
        	else
        	{
        		Debug.Log_blue("新規譜面を登録 " + recordData.stageName + ", version = " + recordData.version, this);
        		updateCheckSaveData = new UpdateCheckSaveData();
        		updateCheckSaveData.stageName = recordData.stageName;
        		updateCheckSaveData.bpm = recordData.bpm;
        		updateCheckSaveData.version = recordData.version;
        		updateCheckSaveDataList.dataList.Add(updateCheckSaveData);
        	}
        	
        	
        	this.informationText.text = "譜面読み込み中 " + recordData.stageName;
            yield return null;
 
            string sheetId = spreadSheetInfoDictionary[recordData.stageName];

          
            string url = this.networkManager.GetSpreadSheetURLWithSheetId(sheetId);
            string result = this.networkManager.Request(url, recordData.stageName);

            ResponseObjectMusicScore masterMusicScore = JsonFx.Json.JsonReader.Deserialize<ResponseObjectMusicScore>(result);
            masterMusicScore.SetupEntry();
            MasterMusicScoreRecordDataList scoreRecordDataList = masterMusicScore.GetDataList();

            RhythmGameDataManager.musicScoreDictionary.Add(recordData.stageName, scoreRecordDataList);

            for (int i=0; i<scoreRecordDataList.dataList.Count; i++)
            {
                MasterMusicScoreRecordData scoreRecordData = scoreRecordDataList.dataList[i];
                //Debug.Log_cyan("downloadMusicScoreListIfNeeded > i=" + i + ", position = " + scoreRecordData.position + ", drum = " + scoreRecordData.drum, this);
            }
        }
        
        DataManager.Save(DataManager.UPDATE_INFO, updateCheckSaveDataList);

        this.informationText.text = "譜面の読み込み完了";

        this.stageText.text = RhythmGameDataManager.masterStageRecordDataList.dataList[this.currentStageIndex].stageName;


        yield return null;
    }

    #endregion // Private
}
