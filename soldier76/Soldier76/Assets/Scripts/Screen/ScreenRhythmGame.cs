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
    [SerializeField] private AudioSource drumAudioSource;
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

        changeState(GameState.Init);
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Title");
        }
#endif

        this.drumObject.transform.Rotate(0.05f, -0.01f, 0.001f);

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
                if (data.drum > 0 && data.isJudgeDone == false)
                {
                    if (this.progressTimer - PERFECT_INTERVAL <= data.time && data.time <= this.progressTimer + PERFECT_INTERVAL)
                    {
                        this.perfectCount++;
                        this.perfectCountText.text = this.perfectCount.ToString();
                        data.isJudgeDone = true;
                        judgeFlag = true;
                    }
                    else if (this.progressTimer - GREAT_INTERVAL <= data.time && data.time <= this.progressTimer + GREAT_INTERVAL)
                    {
                        this.greatCount++;
                        this.greatCountText.text = this.greatCount.ToString();
                        data.isJudgeDone = true;
                        judgeFlag = true;
                    }
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
            if (data.drum > 0)
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
            if (data.drum > 0)                
            {
                if (data.time <= this.enemyTimer)
                {
                    this.musicScoreProgressIndex++;

                    this.drumObject.OnTouchDown();
                    break;
                }
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
            if (data.drum > 0 && data.isJudgeDone == false)
            {
                if (data.time + GREAT_INTERVAL < this.playerTimer)
                {
                    this.throughMissCount++;
                    this.throughMissCountText.text = this.throughMissCount.ToString();
                    data.isJudgeDone = true;
                }                
            }
        }


        this.progressTimer += Time.deltaTime;
        this.playerTimer += Time.deltaTime;


       // TODO:kondo
    }

    private void clearProcess()
    {

    }

    #endregion // InGameProcess



    #region Private

    private void changeState(GameState state)
    {
        Debug.Log_lightblue("changeState > " + this.gameState + " -> " + state, this);
        this.gameState = state;
    }

    #endregion // Private
}
