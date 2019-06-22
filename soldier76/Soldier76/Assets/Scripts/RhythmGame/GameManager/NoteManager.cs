using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
	#region Constant
	
	private const float NoteZ = -1.2f;
	private const float NoteJudgeY = -10.35f;
	
	#endregion
	
	
	
	
	
	[SerializeField] private GameObject noteRoot;
	[SerializeField] private GameObject[] noteObjectPositions;
	
	[SerializeField] private NoteObject notePrefab;
	
	#region Variables
	
	private List<NoteObject> noteObjectList;
	
	// ハイスピのこと
	private float noteArriveTime;
	
	// 敵の譜面の配列参照index
	private int enemyProgressIndex;
	
	#endregion // Variables
	
	
	#region Public
	
	public void Setup()		
	{
		this.noteArriveTime = 1.0f;
		this.enemyProgressIndex = 0;
		this.noteObjectList = new List<NoteObject>();
		
		for (int i=0; i<120; i++)	
		{
		    NoteObject noteObject = Object.Instantiate<NoteObject>(this.notePrefab, this.noteRoot.transform);
		    noteObject.Setup();
		    Vector3 pos = noteObject.transform.localPosition;
		    pos.z = NoteZ;
		    noteObject.transform.localPosition = pos;
		    this.noteObjectList.Add(noteObject);
		}
	}
	
	public void UpdateEnemy(float progressTimer)
	{
		// NOTEの時間 は 譜面の時間 + NOTE到達時間
		float noteTimer = progressTimer + this.noteArriveTime;
		
		for (; this.enemyProgressIndex < RhythmGameDataManager.musicScoreRecordDataList.dataList.Count; this.enemyProgressIndex++)
		{
			MasterMusicScoreRecordData data = RhythmGameDataManager.musicScoreRecordDataList.dataList[this.enemyProgressIndex];
           
			bool isExistNote = false;
			bool isCreateNote = false;
			
			if (data.drum > 0)
			{
				isExistNote = true;
				
				if (data.time <= noteTimer)
				{
					isCreateNote = true;
					createNote(data, NoteSoundType.drum);
				}			
			}
			
			if (data.snare > 0)
			{						
				isExistNote = true;
				
				if (data.time <= noteTimer)
				{
					isCreateNote = true;
					createNote(data, NoteSoundType.snare);
				}
			}
			
			if (data.hihat > 0)
			{					
				isExistNote = true;
				
				if (data.time <= noteTimer)
				{
					isCreateNote = true;
					createNote(data, NoteSoundType.hihat);
				}
			}

			if (isCreateNote)
			{
				this.enemyProgressIndex++;
			}
			
			if (isExistNote)
			{
				break;
			}
		}
		
		updateNote();
	}
	
	#endregion // Public
	
	
	#region Private
	
	private void createNote(MasterMusicScoreRecordData data, NoteSoundType soundType)
	{
		NoteObject noteObject = getInactiveNote();
		
		int soundIndex = (int)soundType;
		
		GameObject posObject = this.noteObjectPositions[soundIndex];
		Vector3 pos = noteObject.transform.localPosition;
		pos.x = posObject.transform.localPosition.x;
		pos.y = 0; // 初期値
		noteObject.transform.localPosition = pos;
	}
	
	private void updateNote()
	{
		foreach (NoteObject noteObject in noteObjectList)
		{
			if (noteObject.IsMoving())
			{
				noteObject.ExecUpdate(NoteJudgeY, this.noteArriveTime);
			}
		}
	}
	
	/// <summary>
	/// 空いてるNoteを貰う
	/// </summary>
	private NoteObject getInactiveNote()
	{
		foreach (NoteObject noteObject in this.noteObjectList)
		{
			if (noteObject.IsInactive())
			{
				noteObject.Use();
				return noteObject;
			}
		}
		// ERROR.
		return null;
	}
	
	#endregion
	
	
}
