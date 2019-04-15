using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenTitle : MonoBehaviour
{
    void Start()
    {
        Debug.Log_cyan("起動", this, 3);
    }

    #region Button

    // ボタンをクリックするとingame.unityに移動します
    public void OnClickGameStartButton () 
    {
        SceneManager.LoadScene("ingame");
	}

    public void OnClickRGStartButton()
    {
        SceneManager.LoadScene("RhythmGame");
    }

    #endregion // Button

}
