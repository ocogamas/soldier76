using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenTitle : MonoBehaviour
{
    void Start()
    {
        Debug.Log("起動");

        Debug.Log_cyan("起動", this, 3);
    }
    
// ボタンをクリックするとgamen1に移動します
    public void ButtonClicked () {
    	Debug.Log("bottun");
        SceneManager.LoadScene("ingame");
	}

}
