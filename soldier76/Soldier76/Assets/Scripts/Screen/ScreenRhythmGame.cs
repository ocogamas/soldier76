using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenRhythmGame : MonoBehaviour
{
    public enum GameState
    {
        Init,
        EnemyReady,
        EnemyCountDown,
        EnemyTurn,
        PlayerReady,
        PlayerCountDown,
        PlayerTurn,
        Clear,
    }


    #region SerializeField

    [SerializeField] private DrumObject drumObject;
    [SerializeField] private DrumObject snareObject;
    [SerializeField] private DrumObject hihatObject;
    [SerializeField] private AudioSource drumAudioSource;
    [SerializeField] private AudioSource snareAudioSource;
    [SerializeField] private AudioSource hihatAudioSource;
    
    [SerializeField] private Text timerText;

    [SerializeField] private Text perfectCountText;
    [SerializeField] private Text greatCountText;
    [SerializeField] private Text throughMissCountText;
    [SerializeField] private Text uselessMissCountText;

    #endregion // SerializeField

    #region Const

    private const float PERFECT_INTERVAL = 0.025f;
    private const float GREAT_INTERVAL = 0.080f;

    #endregion // Const


    #region Variables

    private GameState gameState;
    private float countDownTimer;

    private float enemyTimer;
    private float playerTimer;

    // CountDown時から始まるゲーム進行タイマー
    private float progressTimer;

    private int musicScoreProgressIndex;

    private int perfectCount;
    private int greatCount;
    private int throughMissCount;
    private int uselessMissCount;

    #endregion // Variables




    #region Mono

    private void Start()
    {
        this.drumObject.RegisterCallbackOnTouchDown(onTouchDownDrumObject);
        this.snareObject.RegisterCallbackOnTouchDown(onTouchDownSnareObject);
        this.hihatObject.RegisterCallbackOnTouchDown(onTouchDownHihatObject);

        changeState(GameState.Init);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Title");
        }
        
        if (Input.GetKeyDown(KeyCode.B))
        {
        	onTouchDownDrumObject();
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
        	onTouchDownSnareObject();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
        	onTouchDownHihatObject();
        }
#endif


        this.drumObject.BodyObject().transform.Rotate(0.05f, -0.01f, 0.001f);
        this.snareObject.BodyObject().transform.Rotate(0.05f, -0.01f, 0.001f);
        this.hihatObject.BodyObject().transform.Rotate(0.05f, -0.01f, 0.001f);

        inGameMainProcess();
    }

    #endregion // Mono


    #region Action

    private void onTouchDownDrumObject()
    {
        this.drumAudioSource.PlayOneShot(this.drumAudioSource.clip);
        
        if (this.gameState == GameState.PlayerCountDown || this.gameState == GameState.PlayerTurn)
        {
            bool judgeFlag = false;
            foreach (MasterMusicScoreRecordData data in RhythmGameDataManager.musicScoreRecordDataList.dataList)
            {
            	bool judgeResult = calcOneTouchNote(data.drum, data.isDrumJudgeDone, data.time);
            	
            	if (judgeResult)
            	{
            		judgeFlag = true;
            		data.isDrumJudgeDone = true;
            	}                
            }

            // 判定も取れていないのにタップしたのだとしたら無駄押しミス
            if (judgeFlag == false)
            {
                this.uselessMissCount++;
                this.uselessMissCountText.text = this.uselessMissCount.ToString();
            }
        }
        
    }
    
    private void onTouchDownSnareObject()
    {
    	this.snareAudioSource.PlayOneShot(this.snareAudioSource.clip);

    	if (this.gameState == GameState.PlayerCountDown || this.gameState == GameState.PlayerTurn)
        {
            bool judgeFlag = false;
            foreach (MasterMusicScoreRecordData data in RhythmGameDataManager.musicScoreRecordDataList.dataList)
            {
            	bool judgeResult = calcOneTouchNote(data.snare, data.isSnareJudgeDone, data.time);
            	
            	if (judgeResult)
            	{
            		judgeFlag = true;
            		data.isSnareJudgeDone = true;
            	}                
            }

            // 判定も取れていないのにタップしたのだとしたら無駄押しミス
            if (judgeFlag == false)
            {
                this.uselessMissCount++;
                this.uselessMissCountText.text = this.uselessMissCount.ToString();
            }
        }
    }    
    
    private void onTouchDownHihatObject()
    {
    	this.hihatAudioSource.PlayOneShot(this.hihatAudioSource.clip);

    	if (this.gameState == GameState.PlayerCountDown || this.gameState == GameState.PlayerTurn)
        {
            bool judgeFlag = false;
            foreach (MasterMusicScoreRecordData data in RhythmGameDataManager.musicScoreRecordDataList.dataList)
            {
            	bool judgeResult = calcOneTouchNote(data.hihat, data.isHihatJudgeDone, data.time);
            	
            	if (judgeResult)
            	{
            		judgeFlag = true;
            		data.isHihatJudgeDone = true;
            	}                
            }

            // 判定も取れていないのにタップしたのだとしたら無駄押しミス
            if (judgeFlag == false)
            {
                this.uselessMissCount++;
                this.uselessMissCountText.text = this.uselessMissCount.ToString();
            }
        }
    }

    #endregion // Action


    #region InGameProcess

    private void inGameMainProcess()
    {
        switch(this.gameState)
        {
            case GameState.Init:
                {
                    initProcess();
                    break;
                }
            case GameState.EnemyReady:
                {
                    enemyReadyProcess();
                    break;
                }
            case GameState.EnemyCountDown:
                {
                    enemyCountDownProcess();
                    break;
                }
            case GameState.EnemyTurn:
                {
                    enemyTurnProcess();
                    break;
                }
            case GameState.PlayerReady:
                {
                    playerReadyProcess();
                    break;
                }
            case GameState.PlayerCountDown:
                {
                    playerCountDownProcess();
                    break;
                }
            case GameState.PlayerTurn:
                {
                    playerTurnProcess();
                    break;
                }
            case GameState.Clear:
                {
                    clearProcess();
                    break;
                }
        }
    }

    private void initProcess()
    {
        this.perfectCountText.text = "0";
        this.greatCountText.text = "0";
        this.throughMissCountText.text = "0";
        this.uselessMissCountText.text = "0";

        float bpm = float.Parse(RhythmGameDataManager.masterStageRecordData.bpm);

        float oneProgressTime = 60.0f / (bpm * 48.0f);

        foreach (MasterMusicScoreRecordData data in RhythmGameDataManager.musicScoreRecordDataList.dataList)
        {
            if (data.drum > 0 || data.snare > 0 || data.hihat > 0)
            {
                data.time = oneProgressTime * (float)data.position;
            }            
        }

        this.timerText.color = new Color(1.0f, 1.0f, 1.0f);
        this.countDownTimer = 2.0f;
        changeState(GameState.EnemyReady);
    }

    private void enemyReadyProcess()
    {
        this.timerText.text = "ENEMY\nTURN";

        this.countDownTimer -= Time.deltaTime;
        if (this.countDownTimer <= 0.0f)
        {
            this.countDownTimer = 3.0f;
            this.timerText.color = new Color(1.0f, 0.2f, 0.2f);
            changeState(GameState.EnemyCountDown);            
        }
    }

    private void enemyCountDownProcess()
    {
        this.timerText.text = Mathf.CeilToInt(this.countDownTimer).ToString();

        this.countDownTimer -= Time.deltaTime;
        if (this.countDownTimer <= 0.0f)
        {
            this.enemyTimer = 0;
            changeState(GameState.EnemyTurn);

            this.musicScoreProgressIndex = 0;

            this.timerText.color = new Color(0.2f, 1.0f, 0.2f);
        }
    }

    private void enemyTurnProcess()
    {
        this.timerText.text = this.enemyTimer.ToString("0.00");

        for (; this.musicScoreProgressIndex < RhythmGameDataManager.musicScoreRecordDataList.dataList.Count; this.musicScoreProgressIndex++)
        {
            MasterMusicScoreRecordData data = RhythmGameDataManager.musicScoreRecordDataList.dataList[this.musicScoreProgressIndex];
            
            // ノートがあったらbreakする
            bool isExistNote = false;
            
            // ノートがあって、さらにそれについて音を鳴らす場合
            bool isPlaySound = false;
            
            if (data.drum > 0 || data.snare > 0 || data.hihat > 0)                
            {
            	isExistNote = true;
                if (data.time <= this.enemyTimer)
                {
                	isPlaySound = true;
                }
            }           
                        
            if (isPlaySound)
            {            	                   
            	this.musicScoreProgressIndex++;
            	
            	if (data.drum > 0)
            	{
                	this.drumObject.OnTouchDown();
            	}
            	if (data.snare > 0)
            	{
            		this.snareObject.OnTouchDown();
            	}
            	if (data.hihat > 0)
            	{
            		this.hihatObject.OnTouchDown();
            	}
            	break;
            }
            
            if (isExistNote)
            {
            	break;
            }
        }

        this.enemyTimer += Time.deltaTime;

        if (this.musicScoreProgressIndex >= RhythmGameDataManager.musicScoreRecordDataList.dataList.Count)
        {
            this.timerText.color = new Color(1.0f, 1.0f, 1.0f);
            this.countDownTimer = 2.0f;
            changeState(GameState.PlayerReady);
        }

    }

    private void playerReadyProcess()
    {
        this.timerText.text = "PLAYER\nTURN";
        this.countDownTimer -= Time.deltaTime;
        if (this.countDownTimer <= 0.0f)
        {
            this.countDownTimer = 3.0f;
            this.progressTimer = -3.0f;
            this.timerText.color = new Color(0.2f, 0.2f, 1.0f);
            changeState(GameState.PlayerCountDown);
        }
    }

    private void playerCountDownProcess()
    {
        this.timerText.text = Mathf.CeilToInt(this.countDownTimer).ToString();
        this.countDownTimer -= Time.deltaTime;
        this.progressTimer += Time.deltaTime;
        if (this.countDownTimer <= 0.0f)
        {
            this.playerTimer = 0;
            changeState(GameState.PlayerTurn);

            this.musicScoreProgressIndex = 0;

            this.timerText.color = new Color(0.2f, 1.0f, 0.2f);
        }
    }

    private void playerTurnProcess()
    {
        this.timerText.text = this.playerTimer.ToString("0.00");

        foreach (MasterMusicScoreRecordData data in RhythmGameDataManager.musicScoreRecordDataList.dataList)
        {
            if (data.drum > 0 && data.isDrumJudgeDone == false)
            {
                if (data.time + GREAT_INTERVAL < this.playerTimer)
                {
                    this.throughMissCount++;
                    this.throughMissCountText.text = this.throughMissCount.ToString();
                    data.isDrumJudgeDone = true;
                }                
            }           
                  
            if (data.snare > 0 && data.isSnareJudgeDone == false)
            {
                if (data.time + GREAT_INTERVAL < this.playerTimer)
                {
                    this.throughMissCount++;
                    this.throughMissCountText.text = this.throughMissCount.ToString();
                    data.isSnareJudgeDone = true;
                }                
            }
                      
            if (data.hihat > 0 && data.isHihatJudgeDone == false)
            {
                if (data.time + GREAT_INTERVAL < this.playerTimer)
                {
                    this.throughMissCount++;
                    this.throughMissCountText.text = this.throughMissCount.ToString();
                    data.isHihatJudgeDone = true;
                }                
            }
        }


        this.progressTimer += Time.deltaTime;
        this.playerTimer += Time.deltaTime;

        // クリア判定
        int lastIndex = RhythmGameDataManager.musicScoreRecordDataList.dataList.Count-1;
        if (RhythmGameDataManager.musicScoreRecordDataList.dataList[lastIndex].time + 5 < this.playerTimer)
        {
        	changeState(GameState.Clear);
        }

    }

    private void clearProcess()
    {          
    	SceneManager.LoadScene("Title");
    }

    #endregion // InGameProcess



    #region Private

    private void changeState(GameState state)
    {
        Debug.Log_lightblue("changeState > " + this.gameState + " -> " + state, this);
        this.gameState = state;
    }
    
    private bool calcOneTouchNote(uint note, bool isJudgeDone, float time)
    {     
    	bool returnJudgeDone = false;
    	if (note > 0 && isJudgeDone == false)
        {        
    		if (this.progressTimer - PERFECT_INTERVAL <= time && time <= this.progressTimer + PERFECT_INTERVAL)
            {
                this.perfectCount++;             
                this.perfectCountText.text = this.perfectCount.ToString();
                returnJudgeDone = true;
    		}
    		else if (this.progressTimer - GREAT_INTERVAL <= time && time <= this.progressTimer + GREAT_INTERVAL)
    		{                        
    			this.greatCount++;
    			this.greatCountText.text = this.greatCount.ToString();      
    			returnJudgeDone = true;
    		}
    	}
    	return returnJudgeDone;
    }

    #endregion // Private
}
