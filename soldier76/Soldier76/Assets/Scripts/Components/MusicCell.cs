using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicCell : MonoBehaviour
{

    [SerializeField] private Text stageNameText;
    [SerializeField] private Text practiceClearRatioText;
    [SerializeField] private Text standardClearRatioText;

    private System.Action<string> practiceButtonCallback;
	private System.Action<string> standardButtonCallback;
		
	
	public void Setup(string text, PlayRecordSaveDataDictionary data)
	{
		this.stageNameText.text = text;
        this.practiceClearRatioText.text = 0.ToString("D") + "%";
        this.standardClearRatioText.text = 0.ToString("D") + "%";

        if (data != null)
        {
            if (data.practicePlayRecordSaveDataDictionary.ContainsKey(text))
            {
                PlayRecordSaveData practiceData = data.practicePlayRecordSaveDataDictionary[text];

                Debug.Log_cyan("perfect = " + practiceData.perfect);

                float clearRatio = 100*(practiceData.perfect * 100 + practiceData.great * 10) / ((practiceData.perfect + practiceData.great + practiceData.throughMiss) * 100);
                this.practiceClearRatioText.text = ((int)clearRatio).ToString("D") + "%" ;
            }

            if (data.standardPlayRecordSaveDataDictionary.ContainsKey(text))
            {
                PlayRecordSaveData standardData = data.standardPlayRecordSaveDataDictionary[text];
                float clearRatio = 100*(standardData.perfect * 100 + standardData.great * 10) / ((standardData.perfect + standardData.great + standardData.throughMiss) * 100);

                this.standardClearRatioText.text = ((int)clearRatio).ToString("D") + "%";
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
