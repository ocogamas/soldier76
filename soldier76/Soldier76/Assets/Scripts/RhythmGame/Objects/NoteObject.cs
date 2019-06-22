using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    // 担当するボタン
    private DrumObject drumObject;

    private NoteState noteState;
    
    private float timer;

    #region Public

    public void Setup()
    {
        this.noteState = NoteState.Inactive;
        this.timer = 0;

    }

    public void ExecUpdate(float judgeY, float arriveTime)
    {
    	float ratio = this.timer / arriveTime;
    	
    	Vector3 pos = this.transform.localPosition;
    	pos.y = ratio * judgeY;
    	this.transform.localPosition = pos;
        
    	this.timer += Time.deltaTime;
    }
    
    public bool IsInactive()
    {
    	return this.noteState == NoteState.Inactive;
    }
    
    public void Use()
    {
    	this.noteState = NoteState.Moving;
    }
    
    public bool IsMoving()
    {
    	return this.noteState == NoteState.Moving;
    }

    #endregion // Public



}
