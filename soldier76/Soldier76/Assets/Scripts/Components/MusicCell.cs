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
