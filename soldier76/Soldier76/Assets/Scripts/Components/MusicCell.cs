using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicCell : MonoBehaviour
{
  
	[SerializeField] private Text stageNameText;
	
	private System.Action<string> practiceButtonCallback;
	private System.Action<string> standardButtonCallback;
		
	
	public void Setup(string text)
	{
		this.stageNameText.text = text;

        PlayRecordSaveDataDictionary data = DataManager.Load<PlayRecordSaveDataDictionary>(DataManager.PLAY_RECORD_DATA);
        if (data != null)
        {
            if (data.practicePlayRecordSaveDataDictionary.ContainsKey(text))
            {
                PlayRecordSaveData practiceData = data.practicePlayRecordSaveDataDictionary[text];

                Debug.Log_cyan("perfect = " + practiceData.perfect);
                // TODO:kondo
            }

            if (data.standardPlayRecordSaveDataDictionary.ContainsKey(text))
            {
                PlayRecordSaveData standardData = data.standardPlayRecordSaveDataDictionary[text];

                // TODO:kondo
            }

        }
        else
        {
            Debug.Log_red("data is null " + text, this);
        }
    }
	
	public void RegisterCallbackPracticeButton(System.Action<string> callback)
	{
		this.practiceButtonCallback = callback;
	}
	
	
	public void RegisterCallbackStandardButton(System.Action<string> callback)
	{
		this.standardButtonCallback = callback;
	}
	
	public void OnClickPracticeButton()
	{
		this.practiceButtonCallback(this.stageNameText.text);
	}
	
	public void OnClickStandardButton()
	{
		this.standardButtonCallback(this.stageNameText.text);		
	}
}
