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
        EnemyTurn,
        PlayerReady,
        PlayerTurn,
        Clear,
    }


    #region SerializeField

    [SerializeField] private DrumObject drumObject;
    [SerializeField] private AudioSource drumAudioSource;
    [SerializeField] private Text countdownText;

    #endregion // SerializeField


    #region Variables

    private GameState gameState;
    private float countDownTimer;

    private float enemyTimer;
    private float playerTimer;

    private int musicScoreProgressIndex;

    

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
    }

    #endregion // Action


    #region InGameProcess

    private void inGameMainProcess()
    {
        switch(this.gameState)
        {
            case GameState.Init:
                {
                    this.countDownTimer = 3.0f;
                    initProcess();
                    changeState(GameState.EnemyReady);
                    break;
                }
            case GameState.EnemyReady:
                {
                    enemyReadyProcess();
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
        float bpm = float.Parse(RhythmGameDataManager.masterStageRecordData.bpm);

        float oneProgressTime = 60.0f / (bpm * 48.0f);

        foreach (MasterMusicScoreRecordData data in RhythmGameDataManager.musicScoreRecordDataList.dataList)
        {
            if (data.drum > 0)
            {
                data.time = oneProgressTime * (float)data.position;
                Debug.Log_cyan("time = " + data.time + ", posi = " + data.position, this);
            }


        }
    }

    private void enemyReadyProcess()
    {
        this.countdownText.text = this.countDownTimer.ToString("0.00");

        this.countDownTimer -= Time.deltaTime;
        if (this.countDownTimer <= 0.0f)
        {
            this.enemyTimer = 0;
            changeState(GameState.EnemyTurn);

            this.musicScoreProgressIndex = 0;

            this.countdownText.color = new Color(1.0f, 0.2f, 0.2f);
        }
    }

    private void enemyTurnProcess()
    {
        this.countdownText.text = this.enemyTimer.ToString("0.00");

        for (; this.musicScoreProgressIndex < RhythmGameDataManager.musicScoreRecordDataList.dataList.Count; this.musicScoreProgressIndex++)
        {
            Debug.Log_yellow("enemyTurnProcess > index = " + this.musicScoreProgressIndex);
            MasterMusicScoreRecordData data = RhythmGameDataManager.musicScoreRecordDataList.dataList[this.musicScoreProgressIndex];
            if (data.drum > 0)                
            {
                if (data.time <= this.enemyTimer)
                {
                    Debug.Log_lime("data.time = " + data.time);
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
            this.countdownText.color = new Color(0.0f, 0.4f, 1.0f);
            this.countDownTimer = 3.0f;
            changeState(GameState.PlayerReady);
        }

        Debug.Log_orange("timer = " + enemyTimer);

    }

    private void playerReadyProcess()
    {
        this.countdownText.text = this.countDownTimer.ToString("0.00");
        this.countDownTimer -= Time.deltaTime;
        if (this.countDownTimer <= 0.0f)
        {
            this.playerTimer = 0;
            changeState(GameState.PlayerTurn);

            this.musicScoreProgressIndex = 0;

            this.countdownText.color = new Color(1.0f, 0.2f, 0.2f);
        }
    }

    private void playerTurnProcess()
    {
        this.countdownText.text = this.playerTimer.ToString("0.00");

       
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
