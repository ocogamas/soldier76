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
                    break;
                }
            case GameState.PlayerReady:
                {
                    break;
                }
            case GameState.PlayerTurn:
                {
                    break;
                }
            case GameState.Clear:
                {
                    break;
                }
        }
    }

    private void enemyReadyProcess()
    {
        this.countdownText.text = this.countDownTimer.ToString("0.0");

        this.countDownTimer -= Time.deltaTime;
        if (this.countDownTimer <= 0.0f)
        {
            changeState(GameState.EnemyTurn);
        }
    }

    #endregion // InGameProcess



    #region Private

    private void changeState(GameState state)
    {
        this.gameState = state;
    }

    #endregion // Private
}
