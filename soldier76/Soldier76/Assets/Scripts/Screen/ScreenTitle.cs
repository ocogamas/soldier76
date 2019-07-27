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

    [SerializeField] private Text[] informationTexts;

    [SerializeField] private GameObject titleRoot;
    [SerializeField] private GameObject menuRoot;
    
    [SerializeField] private GameObject scrollContent;
    
    [SerializeField] private MusicCell musicCellPrefab;

    [SerializeField] private Text startButtonText;
    [SerializeField] private Button tutorialButton;
    
    #endregion

    

    private int currentStageIndex = 0;
    
    private int informationTextIndex = 0;


    #region Mono
    
    void Start()
    {
    	Application.targetFrameRate = 60; 
        this.titleRoot.SetActive(true);
        this.menuRoot.SetActive(false);


        UpdateCheckSaveDataList updateCheckData = DataManager.Load<UpdateCheckSaveDataList>(DataManager.UPDATE_INFO);
        if (updateCheckData == null)
        {
            this.startButtonText.text = "START\nneed first download \nabout under 1.0MB";
            this.tutorialButton.gameObject.SetActive(true);
        }
        else
        {
            this.startButtonText.text = "START";
            this.tutorialButton.gameObject.SetActive(false);
        }


        setInformationText("");
        setInformationText("");
        setInformationText("");
        setInformationText("");       
        setInformationText("");
        
        // 譜面を読み込み済みの場合はタイトルをすっ飛ばす
    	if (RhythmGameDataManager.musicScoreDictionary.Count > 0)
    	{   		
    		this.titleRoot.SetActive(false);
    		this.menuRoot.SetActive(true);    		
      
    		StartCoroutine(checkUpdate(false));


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
        StartCoroutine(checkUpdate(false));
    }

    public void OnClickTutorialButton()
    {
        StartCoroutine(checkUpdate(true));

    }


    public void OnClickClearCacheButton()    	
    {
    	DataManager.Delete(DataManager.UPDATE_INFO);
    	DataManager.Delete(DataManager.MUSIC_SCORE_DATA);
    	setInformationText("更新情報と譜面キャッシュを削除");
    }

    
    private void onClickPracticeButton(string stageName)
    {    
    	RhythmGameDataManager.isPracticeMode = true;
    	onClickRGStartButton(stageName);
    }
    
    private void onClickStandardButton(string stageName)
    {
    	RhythmGameDataManager.isPracticeMode = false;
    	onClickRGStartButton(stageName);
    }
    
      
    private void onClickRGStartButton(string stageName)
    {
    	this.kickAudioSource.PlayOneShot(this.kickAudioSource.clip);

    	Debug.Log_yellow("onClickRGStartButton > stageName = " + stageName, this);
    	Debug.Log_yellow("onClickRGStartButton > count = " + RhythmGameDataManager.musicScoreDictionary.Count, this);
    	
    	
        RhythmGameDataManager.musicScoreRecordDataList = RhythmGameDataManager.musicScoreDictionary[stageName];
        
        for (int i=0; i<RhythmGameDataManager.masterStageRecordDataList.dataList.Count; i++)
        {
        	if (RhythmGameDataManager.masterStageRecordDataList.dataList[i].stageName == stageName)
        	{     		
        		RhythmGameDataManager.masterStageRecordData = RhythmGameDataManager.masterStageRecordDataList.dataList[i];
        		break;        			
        	}
        }
        MasterStageRecordData stageRecordData = RhythmGameDataManager.masterStageRecordDataList.dataList[this.currentStageIndex];

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
    
    private void setInformationText(string text)
    {
    	foreach (Text t in this.informationTexts)
    	{
    		t.color = new Color(1.0f, 0.2f, 0.3f);
    	}
    	
    	this.informationTexts[this.informationTextIndex].text = text;
    	
    	this.informationTexts[this.informationTextIndex].color = new Color(0.0f, 1.0f, 0.3f);
    	
    	this.informationTextIndex++;
    	if (this.informationTextIndex >= this.informationTexts.Length)
    	{
    		this.informationTextIndex = 0;
    	}
    }

    private IEnumerator checkUpdate(bool isTutorial)
    {    	
    	setInformationText("MainSpreadSheet通信開始");
        yield return null;
        
        string mainSpreadSheet = this.networkManager.RequestMainSpreadSheet();

    	setInformationText("MainSpreadSheet通信成功");
        yield return null;
        
        ResponseObjectMasterStage masterStage = JsonFx.Json.JsonReader.Deserialize<ResponseObjectMasterStage>(mainSpreadSheet);

        masterStage.SetupEntry();

        RhythmGameDataManager.masterStageRecordDataList = masterStage.GetDataList();

        if (RhythmGameDataManager.masterStageRecordDataList != null)
        {
           　yield return StartCoroutine(downloadMusicScoreListIfNeeded(masterStage, isTutorial));
        }

        this.titleRoot.SetActive(false);
        this.menuRoot.SetActive(true);

        yield return null;
    }

    private IEnumerator downloadMusicScoreListIfNeeded(ResponseObjectMasterStage masterStage, bool isTutorial)
    {
    	setInformationText("SubSpreadSheet一覧取得の通信開始");
        yield return null;
        
        Dictionary<string, string> spreadSheetInfoDictionary = SpreadSheetInfoUtility.GetSpreadSheetInfoDictionary(this.networkManager);

    	setInformationText("SubSpreadSheet一覧取得の通信成功");
        yield return null;
        
        UpdateCheckSaveDataList updateCheckSaveDataList = DataManager.Load<UpdateCheckSaveDataList>(DataManager.UPDATE_INFO);
        if (updateCheckSaveDataList == null)
        {
        	updateCheckSaveDataList = new UpdateCheckSaveDataList();
        	updateCheckSaveDataList.dataList = new List<UpdateCheckSaveData>();
        }
        
        // 保存されている譜面を読み込み
        MusicScoreSaveDataDictionary musicScoreSaveDataDictionary = DataManager.Load<MusicScoreSaveDataDictionary>(DataManager.MUSIC_SCORE_DATA);
        if (musicScoreSaveDataDictionary != null && musicScoreSaveDataDictionary.dataDictionary != null)
        {
        	
        	setInformationText("保存されている譜面を読み込み開始");
        	        	
        	foreach (string stageName in musicScoreSaveDataDictionary.dataDictionary.Keys)
        	{
       	        setInformationText("保存されている譜面を読み込み " + stageName);
        		MusicScoreSaveData musicScoreSaveData = musicScoreSaveDataDictionary.dataDictionary[stageName];    
        		if (RhythmGameDataManager.musicScoreDictionary.ContainsKey(stageName) == false)
        		{
            		RhythmGameDataManager.musicScoreDictionary.Add(stageName, musicScoreSaveData.musicScoreRecordDataList);
        		}
        	}
        	
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
        				// 実際に譜面がなかった場合はスキップしない
        				if (RhythmGameDataManager.musicScoreDictionary.ContainsKey(data.stageName) == false)
        				{
        					isSkip = false;
        					Debug.Log_blue("譜面がなかったので譜面読み込みをスキップしない " + recordData.stageName, this);
        				}
        				else
        				{
        					isSkip = true;
        				}
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
        		
       	        setInformationText("バージョン更新 " + recordData.stageName);
        		Debug.Log_blue("バージョンを更新 " + recordData.stageName + ", version = " + recordData.version, this);
        		updateCheckSaveData.version = recordData.version;
        	}
        	else
        	{
       	        setInformationText("新規譜面を登録 " + recordData.stageName);
        		Debug.Log_blue("新規譜面を登録 " + recordData.stageName + ", version = " + recordData.version, this);
        		updateCheckSaveData = new UpdateCheckSaveData();
        		updateCheckSaveData.stageName = recordData.stageName;
        		updateCheckSaveData.bpm = recordData.bpm;
        		updateCheckSaveData.version = recordData.version;
        		updateCheckSaveDataList.dataList.Add(updateCheckSaveData);
        	}
        	
        	
        	setInformationText("譜面読み込み中" + recordData.stageName);
            yield return null;
 
            string sheetId = spreadSheetInfoDictionary[recordData.stageName];

          
            string url = this.networkManager.GetSpreadSheetURLWithSheetId(sheetId);
            string result = this.networkManager.Request(url, recordData.stageName);

            ResponseObjectMusicScore masterMusicScore = JsonFx.Json.JsonReader.Deserialize<ResponseObjectMusicScore>(result);
            masterMusicScore.SetupEntry();
            MasterMusicScoreRecordDataList scoreRecordDataList = masterMusicScore.GetDataList();

            if (RhythmGameDataManager.musicScoreDictionary.ContainsKey(recordData.stageName))
            {
            	RhythmGameDataManager.musicScoreDictionary.Remove(recordData.stageName);
            }
            RhythmGameDataManager.musicScoreDictionary.Add(recordData.stageName, scoreRecordDataList);

            if (isTutorial)
            {
                break;
            }
        }
        
        DataManager.Save(DataManager.UPDATE_INFO, updateCheckSaveDataList);
        
        #region 譜面の保存
        MusicScoreSaveDataDictionary saveTarget = new MusicScoreSaveDataDictionary();
        saveTarget.dataDictionary = new Dictionary<string, MusicScoreSaveData>();

        foreach (string key in RhythmGameDataManager.musicScoreDictionary.Keys)
        {
        	MasterMusicScoreRecordDataList recordList = RhythmGameDataManager.musicScoreDictionary[key];
        	
        	MusicScoreSaveData saveData = new MusicScoreSaveData();
        	saveData.musicScoreRecordDataList = recordList;
        	
        	saveTarget.dataDictionary.Add(key, saveData);
        }
        
        DataManager.Save(DataManager.MUSIC_SCORE_DATA, saveTarget);
        #endregion // 譜面の保存

        	
        setInformationText("譜面の読み込み完了");


        yield return null;
        
        StartCoroutine( setupMusicUI(isTutorial));
    }

    private IEnumerator setupMusicUI(bool isTutorial)
    {
        PlayRecordSaveDataDictionary playData = DataManager.Load<PlayRecordSaveDataDictionary>(DataManager.PLAY_RECORD_DATA);

        for (int i=0; i<RhythmGameDataManager.masterStageRecordDataList.dataList.Count; i++)
    	{
    		MasterStageRecordData data = RhythmGameDataManager.masterStageRecordDataList.dataList[i];
    		MusicCell musicCell = Object.Instantiate<MusicCell>(this.musicCellPrefab, this.scrollContent.transform);
    		musicCell.Setup(data.stageName, playData);
    		musicCell.RegisterCallbackPracticeButton(onClickPracticeButton);
    		musicCell.RegisterCallbackStandardButton(onClickStandardButton);
    		
            if (isTutorial)
            {
                break;
            }
        }
    	yield return null;
    }

    #endregion // Private
}
