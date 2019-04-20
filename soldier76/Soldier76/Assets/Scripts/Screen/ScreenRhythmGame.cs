using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenRhythmGame : MonoBehaviour
{
    [SerializeField] private DrumObject drumObject;
    [SerializeField] private AudioSource drumAudioSource;

    private void Start()
    {
        this.drumObject.RegisterCallbackOnTouchDown(onTouchDownDrumObject);
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

    }

    private void onTouchDownDrumObject()
    {
        this.drumAudioSource.PlayOneShot(this.drumAudioSource.clip);
    }

}
