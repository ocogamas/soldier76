using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MusicScoreSaveDataDictionary 
{
  
	// key = stageName
	public Dictionary<string, MusicScoreSaveData> dataDictionary = new Dictionary<string, MusicScoreSaveData>();
}
