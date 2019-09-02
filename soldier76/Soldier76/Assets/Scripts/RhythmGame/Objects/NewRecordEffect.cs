using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewRecordEffect : MonoBehaviour
{
	[SerializeField] private ScreenRhythmGame rg;
	
	public void OnFinishAnimation()
	{
		rg.OnFinishNewRecordAnimation();
	}
}
