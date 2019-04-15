using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenTitle : MonoBehaviour
{
    [SerializeField] private AudioSource kickAudioSource;

    void Start()
    {
        Debug.Log_cyan("起動", this, 3);
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

    #endregion // Private
}
